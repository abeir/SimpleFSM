using UnityEditor.Experimental.GraphView;

namespace FSM.Editor.Node
{
    public class EntranceNode : FSMNode
    {
        public sealed override string Value => "Entrance";
        
        public Port OuputPort { get; }
        
        public EntranceNode()
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