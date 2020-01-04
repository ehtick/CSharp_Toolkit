﻿using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Node2Code
{
    public class ClusterNode : BHoMObject, INode
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public ClusterContent Content { get; set; } = null;

        public string Description { get; set; } = "";

        public List<DataParam> Outputs { get; set; } = new List<DataParam>();

        public List<ReceiverParam> Inputs { get; set; } = new List<ReceiverParam>();

        public bool IsInline { get; set; } = false;

        public bool IsDeclaration { get; set; } = false;

        /***************************************************/
    }
}
