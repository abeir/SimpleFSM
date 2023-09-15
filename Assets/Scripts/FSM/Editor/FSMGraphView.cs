using System.Collections.Generic;
using FSM.Editor.Node;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class FSMGraphView : GraphView
    {

        public EntranceNode EntranceNode { get; }
        public AnyNode AnyNode { get; }

        public List<StateNode> StateNodes = new();

        public Vector2 MousePosition { get; private set; }
        public FSMEditorWindow Window { get; private set; }

        public FSMGraphView(FSMEditorWindow win)
        {
            Window = win;
            
            RegisterCallback<MouseUpEvent>(OnMouseUp);
            
            this.StretchToParentSize();
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            Insert(0, new GridBackground());
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            EntranceNode = new EntranceNode
            {
                style =
                {
                    left = FSMEditorWindow.Width / 2 - 250,
                    top = FSMEditorWindow.Height / 2 - 100,
                    minWidth = 120
                }
            };
            AddElement(EntranceNode);

            AnyNode = new AnyNode
            {
                style =
                {
                    left = FSMEditorWindow.Width / 2,
                    top = 50,
                    minWidth = 120
                }
            };
            AddElement(AnyNode);
            
            
            var search = ScriptableObject.CreateInstance<FSMSearchWindowProvider>();
            search.Initialize(this);
            
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), search);
            };
            
        }


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();
            foreach (var port in ports.ToList())
            {
                if (startPort.node == port.node || startPort.direction == port.direction || startPort.portType != port.portType)
                {
                    continue;
                }
                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }


        private void OnMouseUp(MouseUpEvent e)
        {
            MousePosition = e.mousePosition;
        }

        public void Execute(out List<NodeInfo> nodeInfos, out Dictionary<string, List<string>> translates, out List<string> anyTranslates)
        {
            nodeInfos = new List<NodeInfo>();
            foreach (var node in this.nodes)
            {
                if (node is not FSMNode fsmNode)
                {
                    continue;
                }
                
                NodeInfo info;
                info.Node = node;
                info.Value = fsmNode.Value;
                nodeInfos.Add(info);
            }
            
            translates = new Dictionary<string, List<string>>();
            foreach (var edge in EntranceNode.OuputPort.connections)
            {
                if (edge.input.node is not StateNode node)
                {
                    continue;
                }

                if (!translates.TryGetValue(EntranceNode.Value, out var list))
                {
                    list = new List<string>();
                }
                list.Add(node.Value);
                translates[EntranceNode.Value] = list;
                
                RecursionStateNode(node, translates);
            }

            anyTranslates = new List<string>();
            foreach (var edge in AnyNode.OuputPort.connections)
            {
                if (edge.input.node is not StateNode node)
                {
                    continue;
                }
                anyTranslates.Add(node.Value);
            }
        }


        private void RecursionStateNode(StateNode stateNode, Dictionary<string, List<string>> translates)
        {
            foreach (var edge in stateNode.OutputPort.connections)
            {
                if (edge.input.node is not StateNode node)
                {
                    continue;
                }
                if (!translates.TryGetValue(stateNode.Value, out var list))
                {
                    list = new List<string>();
                }
                list.Add(node.Value);
                translates[stateNode.Value] = list;

                RecursionStateNode(node, translates);
            }
        }
    }
}