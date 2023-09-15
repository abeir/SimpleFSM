using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace FSM.Editor.Node
{
    public class StateNode : FSMNode
    {
        public override string Value => _textField.value;
        
        public Port InputPort { get; }
        public Port OutputPort { get; }

        private readonly TextField _textField;

        public StateNode()
        {
            name = GetType().Name;
            
            _textField = new TextField
            {
                value = "State",
                style =
                {
                    minWidth = 120,
                    marginLeft = 10,
                    marginRight = 10,
                    marginTop = 6,
                    marginBottom = 10
                }
            };
            titleContainer.Add(_textField);
            
            InputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                typeof(PortValue));
            InputPort.portName = "In";
            inputContainer.Add(InputPort);

            OutputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                typeof(PortValue));
            OutputPort.portName = "Out";
            outputContainer.Add(OutputPort);
        }
        
        public override void Execute()
        {
        }
    }
}