using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] private InputActionReference movementControl;
        private CharacterController controller;
        private Transform cameraMainTransform;

        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float rotationSpeed = 4f;

        private Vector3 playerVelocity;
        private bool groundedPlayer;

        void OnEnable()
        {
            movementControl.action.Enable();
        }

        void OnDisable()
        {
            movementControl.action.Disable();
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

            Vector2 movement = movementControl.action.ReadValue<Vector2>();
            Vector3 move = new Vector3(movement.x, 0, movement.y);
            move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
            move.y = 0;
            controller.Move(move * Time.deltaTime * playerSpeed);
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


