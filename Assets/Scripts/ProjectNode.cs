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
    }
}