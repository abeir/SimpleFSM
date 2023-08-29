namespace FSM.State
{
    public class AnyState : AStateBaseAdapter
    {
        public override StateDefine State => AnyStateDefine.Instance;

        public override bool CanEnter(StateDefine pre)
        {
            return true;
        }
    }
}