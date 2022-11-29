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
        private ClassDeclarationSyntax _typeDeclaration;
        private SemanticModel _semanticModel;

        public override string Key
        {
            get
            {
                return this._typeDeclaration.Identifier.ToString();
            }
        }

        public override TypeDeclarationSyntax TypeDeclaration
        {
            get
            {
                return _typeDeclaration;
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
                var classSymbol = _semanticModel.GetDeclaredSymbol(_typeDeclaration) as ITypeSymbol;  //need to consult semantic model to make sure the base type isn't actually an interface
                var baseTypeName = classSymbol.BaseType.Name;
                return baseTypeName;
            }
        }

        public ClassNode(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel) : base()
        {
            this._typeDeclaration = classDeclaration;
            this._semanticModel = semanticModel;
        }
    }
}

