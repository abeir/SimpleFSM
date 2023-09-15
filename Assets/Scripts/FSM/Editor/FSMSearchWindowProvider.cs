using System;
using System.Collections.Generic;
using FSM.Editor.Node;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class FSMSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        
        private FSMGraphView _view;

        public void Initialize(FSMGraphView view)
        {
            _view = view;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new()
            {
                new SearchTreeGroupEntry(new GUIContent("Create State"))
            };

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(FSMNode)) 
                        && type != typeof(EntranceNode) && type != typeof(AnyNode))
                    {
                        entries.Add(new SearchTreeEntry(new GUIContent(type.Name))
                        {
                            level = 1, userData = type
                        });
                    }
                }
            }
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is not Type type)
            {
                return false;
            }
            var node = Activator.CreateInstance(type) as FSMNode;
            // node.style.left = _view.MousePosition.x;
            // node.style.top = _view.MousePosition.y;
            var windowRoot = _view.Window.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, context.screenMousePosition - _view.Window.position.position);
            var graphMousePosition = _view.contentViewContainer.WorldToLocal(windowMousePosition);

            var r = new Rect(graphMousePosition, Vector2.zero);
            node.style.left = r.x;
            node.style.top = r.y;
            
            _view.AddElement(node);

            return true;
        }
    }
}