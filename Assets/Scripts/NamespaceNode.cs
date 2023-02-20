using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ThreeSharp.Parsing
{
    public class NamespaceNode : ProjectNode
    {
        private readonly string _namespace;

        public override string Key
        {
            get
            {
                return this._namespace;
            }
        }

        public NamespaceNode(string namespaceName) : base()
        {
            this._namespace = namespaceName;
        }
    }
}
