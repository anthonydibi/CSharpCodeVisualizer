using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSharpCodeVisualizer.Syntax
{
    public class ProjectNode
    {

        protected List<ProjectNode> _children;
        protected GameObject _gameObject;

        public virtual string Key
        {
            get
            {
                return this.ToString();
            }
        }

        public int Height
        {
            get
            {
                return CalculateHeight(this);
            }
        }

        public GameObject GameObject
        {
            get
            {
                return this._gameObject;
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