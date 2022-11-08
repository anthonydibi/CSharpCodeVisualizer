using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;
using System;
using System.Linq;
using CSharpCodeVisualizer.Syntax.Walkers;

namespace CSharpCodeVisualizer.Syntax
{
    public class ProjectTree
    {
        private ProjectNode _root = new ProjectNode();
        private Dictionary<string, ProjectNode> _keyToProjectNodeMap = new Dictionary<string, ProjectNode>();

        public ProjectNode Root
        {
            get
            {
                return _root;
            }
        }

        public Dictionary<string, ProjectNode> KeyNodeMap
        {
            get
            {
                return _keyToProjectNodeMap;
            }
        }

        public ProjectTree(){}

        public void PopulateTreeFromProjectPath(string path)
        {
            Queue<string> q = new Queue<string>();
            q.Enqueue(path);
            while (q.Count > 0)
            {
                string curPath = q.Dequeue();
                try
                {
                    foreach (string dir in Directory.GetDirectories(curPath))
                    {
                        q.Enqueue(dir);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                string[] filesInCur = null;
                try
                {
                    filesInCur = Directory.GetFiles(curPath);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }

                if (filesInCur != null)
                {
                    foreach (string file in filesInCur)
                    {
                        if (file.Substring(file.Length - 3).Equals(".cs"))  //only parse nodes from C# files
                        {
                            List<ClassNode> classNodes = ClassNodesFromPath(file);
                            foreach (var classNode in classNodes)
                            {
                                InsertClassNode(classNode);
                            }
                        }
                    }
                }
            }
        }

        private void CreateNamespaceInTree(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            string namespaceTitle = namespaceDeclaration.Name.ToString();
            string[] namespaceElements = namespaceTitle.Split('.');
            string curNamespace = "";
            ProjectNode curNode = _root;
            for (var i = 0; i < namespaceElements.Length; i++)
            {
                string namespaceElement = namespaceElements[i];
                if (i == 0)
                {
                    curNamespace += namespaceElement;
                }
                else
                {
                    curNamespace += "." + namespaceElement;
                }

                ProjectNode namespaceNode;
                bool namespaceWasFoundInMap = _keyToProjectNodeMap.TryGetValue(curNamespace, out namespaceNode);
                if (!namespaceWasFoundInMap)
                {
                    namespaceNode = new NamespaceNode(namespaceDeclaration);
                    curNode.Children.Add(namespaceNode);
                    _keyToProjectNodeMap.Add(curNamespace, namespaceNode);
                }
                curNode = namespaceNode;
            }
        }

        private void InsertClassNode(ClassNode classNode)
        {
            NamespaceDeclarationSyntax classNamespace =
                classNode.ClassDef.Ancestors().OfType<NamespaceDeclarationSyntax>().First();
            if (classNamespace == null)  //if the class doesn't have a namespace, place it in the root namespace
            {
                _root.Children.Add(classNode);
                _keyToProjectNodeMap.Add(classNode.Key, classNode);
                return;
            }
            if (!this._keyToProjectNodeMap.ContainsKey(classNamespace.Name.ToString()))
            {
                CreateNamespaceInTree(classNamespace);
            }
            ProjectNode namespaceNode = _keyToProjectNodeMap[classNamespace.Name.ToString()];
            namespaceNode.Children.Add(classNode);
        }

        private List<ClassNode> ClassNodesFromPath(string path)
        {
            List<ClassNode> classNodes = new List<ClassNode>();
            string programText = System.IO.File.ReadAllText(path);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var collector = new ClassCollector();
            collector.Visit(root);
            foreach (ClassDeclarationSyntax classDef in collector.Classes)
            {
                ClassNode classNode = new ClassNode(classDef);
                InsertClassNode(classNode);
            }
            return classNodes;
        }
    }
}
