using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpCodeVisualizer.Syntax
{
    public class ProjectNode
    {

        protected List<ProjectNode> _children;
        private TypeDeclarationSyntax _typeDeclaration;

        public virtual string Key
        {
            get
            {
                return this.ToString();
            }
        }

        public virtual TypeDeclarationSyntax TypeDeclaration
        {
            get
            {
                return _typeDeclaration;
            }
        }

        public int Height
        {
            get
            {
                return CalculateHeight(this);
            }
        }

        public List<ProjectNode> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

        public ProjectNode()
        {
            this._children = new List<ProjectNode>();
        }

        public int CalculateHeight(ProjectNode node)
        {
            if(node.Children.Count == 0)
            {
                return 1;
            }
            int maxHeight = 0;
            node.Children.ForEach(node => maxHeight = Mathf.Max(maxHeight, CalculateHeight(node)));
            return 1 + maxHeight;
        }
    }
}