
namespace FSM.Editor.Node
{
    public abstract class FSMNode : UnityEditor.Experimental.GraphView.Node
    {
        public abstract string Value { get; }
        public abstract void Execute();
    }

    public struct PortValue
    {
    }
}