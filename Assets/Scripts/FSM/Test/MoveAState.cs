using UnityEngine;

namespace FSM.Test
{
    public class MoveAState : AStateBase
    {
        public override StateDefine State { get; } = new()
        {
            ID = (int)PlayerStateID.Move,
            Name = "Move"
        };


        private readonly PlayerController _player;

        public  MoveAState(PlayerController player)
        {
            _player = player;
        }

        public override bool CanEnter(StateDefine pre)
        {
            return _player.IsOnGround;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log("MoveState.OnEnter");
        }

        public override void OnExit(StateDefine next)
        {
            Debug.Log("MoveState.OnExit");
        }

        public override void OnStay()
        {
        }
    }
}