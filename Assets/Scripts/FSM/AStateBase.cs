namespace FSM
{
    public interface IStateBase
    {
        
        public StateDefine State { get; }
        
        public StateMachine StateMachine { set; }
        
        public bool CanEnter(StateDefine pre);

        public void OnEnter(StateDefine pre);
        public void OnExit(StateDefine next);

        public void OnStay();
    }


    public abstract class AStateBase : IStateBase
    {
        public abstract StateDefine State { get; }
        
        public StateMachine StateMachine { protected get; set; }

        public abstract bool CanEnter(StateDefine pre);

        public abstract void OnEnter(StateDefine pre);

        public abstract void OnExit(StateDefine next);

        public abstract void OnStay();
    }

    public abstract class AStateBaseAdapter : AStateBase
    {
        public override void OnEnter(StateDefine pre)
        {
        }

        public override void OnExit(StateDefine next)
        {
        }

        public override void OnStay()
        {
        }
    }
}