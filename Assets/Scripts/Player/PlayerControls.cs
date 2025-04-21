using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private InputActionReference movementControl;
        [SerializeField] private InputActionReference runControl; // Reference to Run action
        [SerializeField] private Animator playerAnimator;
        private CharacterController controller;
        private Transform cameraMainTransform;

        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float runningSpeed = 8.0f; // Running speed
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float rotationSpeed = 4f;

        private Vector3 playerVelocity;
        private bool groundedPlayer;

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
            controller = this.GetComponent<CharacterController>();
            cameraMainTransform = Camera.main.transform;
        }

        void Update()
        {
            HandleMovement();
            HandleRotation();
            ApplyGravity();
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
                bool isRunning = runControl.action.ReadValue<float>() > 0; // Check if Run action is active
                float currentSpeed = isRunning ? runningSpeed : playerSpeed;

                Vector3 moveDirection = CalculateMoveDirection(movementInput);
                controller.Move(moveDirection * Time.deltaTime * currentSpeed);

                float speedValue = isRunning ? 1.0f : 0.5f; // Set Speed for blend tree
                playerAnimator.SetFloat("Speed", speedValue);
            }
            else
            {
                playerAnimator.SetFloat("Speed", 0.0f); // Idle
            }
        }

        private Vector3 CalculateMoveDirection(Vector2 movementInput)
        {
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
            move.y = 0; // Ensure no vertical movement
            return move;
        }

        private void HandleRotation()
        {
            Vector2 movement = movementControl.action.ReadValue<Vector2>();
            if (movement != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }

        private void ApplyGravity()
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}


