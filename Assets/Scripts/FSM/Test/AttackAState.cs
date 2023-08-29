using System.Collections;
using UnityEngine;

namespace FSM.Test
{
    public class AttackAState : AStateBase
    {
        public override StateDefine State { get; } = new StateDefine()
        {
            ID = (int)PlayerStateID.Attack,
            Name = "Attack"
        };
        
        private readonly PlayerController _player;

        private const float Timeout = 1;

        public AttackAState(PlayerController player)
        {
            _player = player;
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return true;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log("AttackState.OnEnter");

            _player.StartCoroutine(ExitTimer(Timeout));
        }

        public override void OnExit(StateDefine next)
        {
            Debug.Log("AttackState.OnExit");
            
        }

        public override void OnStay()
        {
        }

        private IEnumerator ExitTimer(float timeout)
        {
            yield return new WaitForSeconds(timeout);
            
            StateMachine.Translate((int)PlayerStateID.Idle);
        }
    }
}