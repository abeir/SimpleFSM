using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


namespace FSM.Test
{
    public class PlayerController : MonoBehaviour, InputActions.IGameplayActions
    {
        public TextMeshProUGUI statsDisplay;

        public Rigidbody Rigidbody { get; private set; }
        
        [Title("Inspect")]
        [ShowInInspector, ReadOnly]
        public Vector2 MoveDirection { get; private set; }
        
        [ShowInInspector, ReadOnly]
        public bool IsOnGround { get; private set; }
        


        [Title("Motion")]
        [SerializeField]
        public float moveSpeed;
        [SerializeField]
        public float jumpForce;
        

        [Title("Ground Check")]
        [SerializeField, LabelText("Origin")]
        private Vector3 groundCheckOrigin;
        [SerializeField, LabelText("Direction")]
        private Vector3 groundCheckDirection;
        [SerializeField, LabelText("Distance")]
        private float groundCheckDistance;
        [SerializeField, LabelText("Layer")]
        private LayerMask groundCheckLayer;
        [SerializeField, LabelText("Color")]
        private Color groundCheckColor = Color.cyan;
        
        private StateMachine _stateMachine;
        private InputActions _inputAction;

        
    
        private void Awake()
        {
            _stateMachine = new StateMachine();
            _stateMachine.AddState(new IdleState());
            _stateMachine.AddState(new MoveAState(this));
            _stateMachine.AddState(new JumpAState(this));
            _stateMachine.AddState(new AttackAState(this));
            
            _stateMachine.AddTranslate((int)PlayerStateID.Idle, (int)PlayerStateID.Move);
            _stateMachine.AddTranslate((int)PlayerStateID.Idle, (int)PlayerStateID.Jump);
            _stateMachine.AddTranslate((int)PlayerStateID.Idle, (int)PlayerStateID.Attack);
            
            _stateMachine.AddTranslate((int)PlayerStateID.Move, (int)PlayerStateID.Idle);
            _stateMachine.AddTranslate((int)PlayerStateID.Move, (int)PlayerStateID.Jump);
            _stateMachine.AddTranslate((int)PlayerStateID.Move, (int)PlayerStateID.Attack);
            
            _stateMachine.AddTranslate((int)PlayerStateID.Jump, (int)PlayerStateID.Idle);
            _stateMachine.AddTranslate((int)PlayerStateID.Jump, (int)PlayerStateID.Move);
            
            _stateMachine.AddTranslate((int)PlayerStateID.Attack, (int)PlayerStateID.Idle);
            
            _stateMachine.SetDefaultState((int)PlayerStateID.Idle);

            _inputAction = new InputActions();
            _inputAction.Gameplay.SetCallbacks(this);
        }

        private void Start()
        {
            _stateMachine.Init();
            
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _inputAction.Enable();
        }

        private void OnDisable()
        {
            _inputAction.Disable();
        }

        private void Update()
        {
            GroundCheck();
            
            statsDisplay.SetText(_stateMachine.Current.Name);
            
            _stateMachine.Update();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                MoveDirection = context.ReadValue<Vector2>();
            }
            if (context.performed)
            {
                _stateMachine.Translate((int)PlayerStateID.Move);
            }
            else if(context.canceled)
            {
                _stateMachine.Translate((int)PlayerStateID.Idle);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _stateMachine.Translate((int)PlayerStateID.Jump);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _stateMachine.Translate((int)PlayerStateID.Attack);
            }
        }


        private void GroundCheck()
        {
            IsOnGround = Physics.Raycast(transform.position + groundCheckOrigin, groundCheckDirection, groundCheckDistance,
                groundCheckLayer);
        }
        

        private void OnDrawGizmos()
        {
            Gizmos.color = groundCheckColor;
            var origin = transform.position + groundCheckOrigin;
            Gizmos.DrawLine(origin, origin + groundCheckDirection * groundCheckDistance);
        }
    }
}
