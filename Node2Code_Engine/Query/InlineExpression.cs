﻿using BH.Engine.Reflection;
using BH.oM.Node2Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Node2Code
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Dictionary<Guid, Variable> IInlineExpression(this INode node, Dictionary<Guid, Variable> variables)
        {
            return InlineExpression(node as dynamic, variables);
        }

        /***************************************************/

        public static Dictionary<Guid, Variable> InlineExpression(this TypeNode node, Dictionary<Guid, Variable> variables)
        {
            Guid id = node.Outputs.First().BHoM_Guid;

            Variable variable = new Variable
            {
                Expression = SyntaxFactory.TypeOfExpression(SyntaxFactory.ParseTypeName(node.Type.FullName)),
                SourceId = id,
                Type = node.Outputs.First().DataType
            };

            return new Dictionary<Guid, Variable> {
                { id, variable }
            };
        }

        /***************************************************/

        public static Dictionary<Guid, Variable> InlineExpression(this GetPropertyNode node, Dictionary<Guid, Variable> variables)
        {
            List<ExpressionSyntax> arguments = node.Inputs.Select(x => Query.ArgumentValue(x, variables)).ToList();

            Guid id = node.Outputs.First().BHoM_Guid;

            Variable variable = new Variable
            {
                Expression = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    arguments[0],
                    SyntaxFactory.IdentifierName(arguments[1].ToFullString().Replace("\"", ""))
                ),
                SourceId = id,
                Type = node.Outputs.First().DataType
            };

            return new Dictionary<Guid, Variable> {
                { id, variable }
            };
        }

        /***************************************************/

        public static Dictionary<Guid, Variable> InlineExpression(this ExplodeNode node, Dictionary<Guid, Variable> variables)
        {
            List<ExpressionSyntax> arguments = node.Inputs.Select(x => Query.ArgumentValue(x, variables)).ToList();

            IEnumerable<DataParam> validOutputs = node.Outputs.Where(x => x.TargetIds.Count > 0);
            if (validOutputs.Count() == 0)
                return new Dictionary<Guid, Variable>();
            else
                return validOutputs.ToDictionary(
                    output =>   output.BHoM_Guid,
                    output => new Variable
                    {
                        Expression = SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            arguments[0],
                            SyntaxFactory.IdentifierName(output.Name)
                        ) as ExpressionSyntax,
                        SourceId = output.BHoM_Guid,
                        Type = output.DataType
                    }
                );
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Dictionary<Guid, Variable> InlineExpression(this INode node, Dictionary<Guid, Variable> variables)
        {
            return new Dictionary<Guid, Variable>();
        }

        /***************************************************/
    }
}
