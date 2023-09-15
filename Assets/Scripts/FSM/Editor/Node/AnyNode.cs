using UnityEditor.Experimental.GraphView;

namespace FSM.Editor.Node
{
    public class AnyNode : FSMNode
    {

        public sealed override string Value => "Any";
        
        public Port OuputPort { get; }
        
        public AnyNode()
        {
            name = GetType().Name;
            title = Value;
            
            capabilities -= Capabilities.Deletable;

            OuputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                typeof(PortValue));
            OuputPort.portName = "Out";
            outputContainer.Add(OuputPort);
        }


        public override void Execute()
        {
        }
    }
}