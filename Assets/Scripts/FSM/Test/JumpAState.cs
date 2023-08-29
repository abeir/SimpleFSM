using UnityEngine;

namespace FSM.Test
{
    public class JumpAState : AStateBase
    {
        public override StateDefine State { get; } = new StateDefine()
        {
            ID = (int)PlayerStateID.Jump,
            Name = "Jump",
        };


        private bool _jumping;
        
        private PlayerController _player;

        public JumpAState(PlayerController player)
        {
            _player = player;
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return _player.IsOnGround;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log("JumpState.OnEnter");

            _player.Rigidbody.velocity = Vector3.up * _player.jumpForce;
            _jumping = true;
        }

        public override void OnExit(StateDefine next)
        {
            Debug.Log("JumpState.OnExit");

            _jumping = false;
        }

        public override void OnStay()
        {
            if (_jumping && _player.IsOnGround && _player.Rigidbody.velocity.y <= 0)
            {
                Debug.Log("JumpState.OnStay => Idle");
                StateMachine.Translate((int)PlayerStateID.Idle);
            }
        }
        
        
        
    }
}