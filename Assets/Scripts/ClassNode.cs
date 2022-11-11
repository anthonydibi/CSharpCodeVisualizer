using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpCodeVisualizer.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpCodeVisualizer.Syntax
{
    public class ClassNode : ProjectNode
    {
        private ClassDeclarationSyntax _classDeclaration;
        private SemanticModel _semanticModel;

        public override string Key
        {
            get
            {
                return this._classDeclaration.Identifier.ToString();
            }
        }

        public ClassDeclarationSyntax ClassDef
        {
            get
            {
                return _classDeclaration;
            }
        }

        public SemanticModel SemanticModel
        {
            get
            {
                return this._semanticModel;
            }
        }

        public string BaseClass
        {
            get
            {
                var classSymbol = _semanticModel.GetDeclaredSymbol(_classDeclaration) as ITypeSymbol;  //need to consult semantic model to make sure the base type isn't actually an interface
                var baseTypeName = classSymbol.BaseType.Name;
                return baseTypeName;
            }
        }

        public ClassNode(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel) : base()
        {
            this._classDeclaration = classDeclaration;
            this._semanticModel = semanticModel;
        }
    }
}

