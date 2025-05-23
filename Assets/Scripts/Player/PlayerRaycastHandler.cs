using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Player
{
    public class PlayerRaycastHandler : Singleton<PlayerRaycastHandler>
    {
        [Header("Settings Raycast")]
        [SerializeField] private Transform rayOrigin;
        private Vector3 rayDirection = Vector3.forward;
        [SerializeField] private float rayDistance = 5f;
        [SerializeField] private LayerMask raycastLayerMask = Physics.DefaultRaycastLayers;

        [Header("Debug")]
        [SerializeField] private bool showDebugRay = true;
        [SerializeField] private Color rayColor = Color.red;

        [Header("Controls")]
        [SerializeField] public InputActionReference interactionControl;


        private RaycastHit currentHit;
        private bool hasHit;

        public RaycastHit CurrentHit => currentHit;
        public bool HasHit => hasHit;


        protected override void Awake()
        {
            interactionControl.action.performed += ctx => OnMinigameDetection();
            interactionControl.action.performed += ctx => OnSleepDetection();
        }

        private void FixedUpdate()
        {
            PerformRaycast();
        }

        void OnEnable()
        {
            interactionControl.action.Enable();
        }

        void OnDisable()
        {
            interactionControl.action.Disable();
        }

        private void PerformRaycast()
        {
            if (rayOrigin == null)
            {
                DebugToolKit.LogWarning("Ray origin atanmadı!");
                hasHit = false;
                return;
            }

            Vector3 worldDir = rayOrigin.TransformDirection(rayDirection);

            if (showDebugRay)
                DebugToolKit.DrawRay(rayOrigin.position, worldDir, rayDistance, rayColor);

            hasHit = Physics.Raycast(rayOrigin.position, worldDir, out currentHit, rayDistance, raycastLayerMask);
        }
        public bool HitHasTag(string tag)
        {
            return hasHit && currentHit.collider.CompareTag(tag);
        }

        public void SetRayOrigin(Transform origin)
        {
            rayOrigin = origin;
        }

        public void SetRayDirection(Vector3 direction)
        {
            rayDirection = direction.normalized;
        }

        private void OnMinigameDetection()
        {
            if (hasHit && currentHit.collider.CompareTag("Door"))
            {
                DebugToolKit.Log("Minigame Trigger detected!");
                MinigameManager.Instance.RegisterMinigame("OpenDoor", GetComponent<OpenDoorMG>());
                MinigameManager.Instance.StartMinigame("OpenDoor");
            }
            if (hasHit && currentHit.collider.CompareTag("BrakeDoor"))
            {
                DebugToolKit.Log("Minigame Trigger detected!");
                MinigameManager.Instance.StartMinigame("BrakeDoor");
            }
        }

        private void OnSleepDetection()
        {
            if (hasHit && currentHit.collider.CompareTag("Bed") && DailyRoutineManager.Instance.dayTimeManager.IsDayTime(DayTimeManager.TimeOfDay.Night))
            {
                DebugToolKit.Log("Sleep Trigger detected!");
                PlayerAnimationHandler.Instance.SleepPlayer();
            }
        }
    }
}