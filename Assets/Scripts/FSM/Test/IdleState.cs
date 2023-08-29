using UnityEngine;

namespace FSM.Test
{
    public class IdleState : AStateBase
    {
        public override StateDefine State { get; } = new StateDefine()
        {
            ID = (int)PlayerStateID.Idle,
            Name = "Idle"
        };
        
        public override bool CanEnter(StateDefine pre)
        {
            return true;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log("IdleState.OnEnter");
        }

        public override void OnExit(StateDefine next)
        {
            Debug.Log("IdleState.OnExit");
        }

        public override void OnStay()
        {
        }
    }
}