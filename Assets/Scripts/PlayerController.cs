using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : BaseController
    {
        [Header("Camera")]
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float freeLookSensitivity = 12;
        [Range(0.1f, 1f)]
        [SerializeField] private float aimLookSensitivityMultiplier = .5f;

        [Space(10)]
        [SerializeField] private float TopClamp = 80f;
        [SerializeField] private float BottomClamp = -80f;

        [Space(10)]
        [SerializeField] private bool invertVerticalLook = false;
        [SerializeField] private bool invertHorizontalLook = false;

        private CharacterController controller;

        // Movement
        private bool isWalking;
        private bool isSprinting;
        private Vector2 inputMovement;
        private Vector2 inputLook;
        private Vector2 lastMovement;
        private float minMovement;
        
        // Camera
        private float cameraTargetEulerYaw; // Camera rotation on X Axis
        private float cameraTargetEulerPitch; // Camera rotation on Y Axis

        private bool IsGrounded { get { return Physics.Raycast(transform.position, Vector3.down, .3f); } }

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            minMovement = controller.minMoveDistance;
        }

        private void Update()
        {
            GetInputs();
        }

        private void FixedUpdate()
        {
            HandleStates();
            HandleCameraLook();
            HandleMovement();
        }

        private void GetInputs()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ToggleLockMovementState();

            if (Input.GetKeyDown(KeyCode.CapsLock))
                isWalking = !isWalking;

            isSprinting = Input.GetButton("Run");
            inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            inputLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        private void HandleCameraLook()
        {
            float lookSensitivity;

            switch (weaponState)
            {
                case WeaponState.Away:
                    lookSensitivity = freeLookSensitivity;
                    break;
                case WeaponState.Down:
                    lookSensitivity = freeLookSensitivity;
                    break;
                case WeaponState.Ready:
                    lookSensitivity = freeLookSensitivity * aimLookSensitivityMultiplier;
                    break;
                case WeaponState.Aiming:
                    lookSensitivity = freeLookSensitivity * aimLookSensitivityMultiplier;
                    break;
                default:
                    lookSensitivity = freeLookSensitivity;
                    break;
            }

            cameraTargetEulerYaw += (invertVerticalLook ? inputLook.y : -inputLook.y) * 1 * lookSensitivity;
            cameraTargetEulerYaw = Mathf.Clamp(cameraTargetEulerYaw, BottomClamp, TopClamp);

            cameraTargetEulerPitch += (invertHorizontalLook ? -inputLook.x : inputLook.x) * 1 * lookSensitivity;

            cameraTarget.rotation = Quaternion.Euler(cameraTargetEulerYaw, cameraTargetEulerPitch, 0);
            
            //cameraTarget.rotation *= Quaternion.AngleAxis(inputLook.x * lookSensitivity, Vector3.up);
        }

        private void HandleMovement()
        {
            float targetSpeed;

            switch (movementState)
            {
                case MovementState.Walk:
                    targetSpeed = walkSpeed;
                    break;
                case MovementState.Run:
                    targetSpeed = runSpeed;
                    break;
                case MovementState.Sprint:
                    targetSpeed = sprintSpeed;
                    break;
                case MovementState.Locked:
                    targetSpeed = 0;
                    break;
                default:
                    targetSpeed = 0;
                    break;
            }

            Vector2 targetMovement = Vector2.ClampMagnitude(inputMovement * targetSpeed, targetSpeed);
            Vector2 movement = Vector2.Lerp(lastMovement, targetMovement, acceleration * Time.fixedDeltaTime);
            lastMovement = movement;


            Vector3 finalMovement = new Vector3(movement.x, IsGrounded ? 0 : Physics.gravity.y, movement.y);

            if (finalMovement.magnitude > minMovement)
            {
                controller.Move(finalMovement * Time.fixedDeltaTime);
            }
            else
            {
                finalMovement = Vector3.zero;
            }

            OnInputMovement?.Invoke(finalMovement);
        }

        private void HandleStates()
        {
            HandleMovementState();
            HandleStanceState();
            HandleEquipmentState();
        }

        protected override void HandleMovementState()
        {
            if (movementState == MovementState.Locked) return;

            if (isSprinting)
            {
                isWalking = false;
                movementState = MovementState.Sprint;
                return;
            }

            if (isWalking)
            {
                movementState = MovementState.Walk;
                return;
            }

            movementState = MovementState.Run;
        }

        private bool debug_isStanceWarningShown;
        protected override void HandleStanceState()
        {
            if (!debug_isStanceWarningShown)
            {
                Debug.LogWarning("HandleStanceState not implemented yet");
                debug_isStanceWarningShown = true;
            }
        }

        private bool debug_isWeaponWarningShown;
        protected override void HandleEquipmentState()
        {
            if (!debug_isWeaponWarningShown)
            {
                Debug.LogWarning("HandleEquipmentState not implemented yet");
                debug_isWeaponWarningShown = true;
            }
        }
    }
}
