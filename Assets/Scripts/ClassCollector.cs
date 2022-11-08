using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpCodeVisualizer.Syntax.Walkers
{
    class ClassCollector : CSharpSyntaxWalker
    {
        public List<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            this.Classes.Add(node);
        }
    }
}
