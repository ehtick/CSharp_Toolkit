﻿using BH.oM.Node2Code;
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
        /**** Interface Methods                         ****/
        /***************************************************/

        public static Dictionary<Guid, Type> IDataTypePerNode(this INode node)
        {
            return DataTypePerParam(node as dynamic);
        }


        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Dictionary<Guid, Type> DataTypePerParam(this ClusterContent content)
        {
            Dictionary<Guid, Type> types = new Dictionary<Guid, Type>();
            if (content == null)
                return types;

            content.InternalNodes = content.InternalNodes.Select(x => x.IPopulateTypes()).ToList();

            foreach (DataParam input in content.Inputs)
                types[input.BHoM_Guid] = input.DataType;

            foreach (ReceiverParam output in content.Outputs)
                types[output.BHoM_Guid] = output.DataType;

            foreach (DataParam intern in content.InternalParams)
                types[intern.BHoM_Guid] = intern.DataType;

            foreach (INode child in content.InternalNodes)
            {
                Dictionary<Guid, Type> childTypes = child.IDataTypePerNode();
                foreach (KeyValuePair<Guid, Type> kvp in childTypes)
                    types[kvp.Key] = kvp.Value;
            }

            return types;
        }

        /***************************************************/

        public static Dictionary<Guid, Type> DataTypePerParam(this INode node)
        {
            Dictionary<Guid, Type> types = new Dictionary<Guid, Type>();

            foreach (ReceiverParam input in node.Inputs)
                types[input.BHoM_Guid] = input.DataType;

            foreach (DataParam output in node.Outputs)
                types[output.BHoM_Guid] = output.DataType;

            return types;
        }

        /***************************************************/
    }
}
