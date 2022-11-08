using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpCodeVisualizer.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ClassNode : ProjectNode
{
    private ClassDeclarationSyntax _classDeclaration;

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

    public ClassNode(ClassDeclarationSyntax classDeclaration) : base()
    {
        this._classDeclaration = classDeclaration;
    }
}
