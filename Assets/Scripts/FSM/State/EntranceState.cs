namespace FSM.State
{
    public class EntranceState : AStateBaseAdapter
    {
        public override StateDefine State => EntranceStateDefine.Instance;
        public override bool CanEnter(StateDefine pre)
        {
            return true;
        }
    }
}