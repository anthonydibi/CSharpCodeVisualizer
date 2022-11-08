using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpCodeVisualizer.Syntax
{
    public class NamespaceNode : ProjectNode
    {
        private readonly NamespaceDeclarationSyntax _namespaceDeclaration;

        public override string Key
        {
            get
            {
                return this._namespaceDeclaration.Name.ToString();
            }
        }

        public NamespaceNode(NamespaceDeclarationSyntax namespaceDeclaration) : base()
        {
            this._namespaceDeclaration = namespaceDeclaration;
        }
    }
}
