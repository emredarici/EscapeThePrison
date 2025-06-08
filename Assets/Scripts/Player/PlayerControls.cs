using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] public InputActionReference movementControl;
        [SerializeField] private InputActionReference runControl;
        [HideInInspector] public CharacterController controller;
        private CapsuleCollider capsuleCollider;
        private Transform cameraMainTransform;
        public Transform startPosition;
        [HideInInspector] public AudioSource audioSource;
        public GameObject trayObject;

        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float runningSpeed = 8.0f;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float rotationSpeed = 4f;
        [SerializeField] private float sensitivity = 1.0f;

        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private bool isInputEnabled = true; // Hareket girişlerini kontrol eden bayrak

        public IPlayerAnimationHandler animationHandler;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            controller = GetComponent<CharacterController>();
            animationHandler = GetComponent<IPlayerAnimationHandler>();
            trayObject.SetActive(false);
        }

        void OnEnable()
        {
            movementControl.action.Enable();
            runControl.action.Enable();
        }

        void OnDisable()
        {
            movementControl.action.Disable();
            runControl.action.Disable();
        }

        private void Start()
        {
            this.transform.position = startPosition.position;
            cameraMainTransform = Camera.main.transform;
        }

        void Update()
        {
            if (!controller.enabled)
            {
                return;
            }

            if (isInputEnabled)
            {
                HandleMovement();
                HandleRotation();
            }
            else
            {
                animationHandler.SetMovementSpeed(0.0f);
            }
            ApplyGravity();
        }

        public void DisableInput()
        {
            Debug.Log("Input Disabled");
            AudioManager.Instance.StopAudio(audioSource);
            PlayerAnimationHandler.Instance.SetMovementSpeed(0.0f);
            isInputEnabled = false;
        }

        public void EnableInput()
        {
            Debug.Log("Input Enabled");
            isInputEnabled = true;
        }

        public void isTriggerTrue()
        {
            this.capsuleCollider.isTrigger = true;
        }

        public void isTriggerFalse()
        {
            this.capsuleCollider.isTrigger = false;
        }

        private void HandleMovement()
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector2 movementInput = movementControl.action.ReadValue<Vector2>();
            if (movementInput != Vector2.zero)
            {
                bool isRunning = runControl.action.ReadValue<float>() > 0;
                float currentSpeed = isRunning ? runningSpeed : playerSpeed;

                Vector3 moveDirection = CalculateMoveDirection(movementInput);
                controller.Move(moveDirection * Time.deltaTime * currentSpeed);

                float speedValue = isRunning ? 1.0f : 0.5f;
                animationHandler.SetMovementSpeed(speedValue);
                AudioData desiredAudio = isRunning ? AudioManager.Instance.runSource : AudioManager.Instance.walkSource;
                AudioManager.Instance.PlayLoopingAudio(desiredAudio, audioSource);
            }
            else
            {
                animationHandler.SetMovementSpeed(0.0f);
                AudioManager.Instance.StopAudio(audioSource);
            }
        }

        private Vector3 CalculateMoveDirection(Vector2 movementInput)
        {
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
            move.y = 0;
            return move;
        }

        private void HandleRotation()
        {
            Vector2 movement = movementControl.action.ReadValue<Vector2>();
            if (movement != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed * sensitivity);
            }
        }

        private void ApplyGravity()
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        public float Sensitivity
        {
            get => sensitivity;
            set => sensitivity = value;
        }
    }
}


