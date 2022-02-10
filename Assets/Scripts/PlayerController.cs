using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : BaseController
    {
        [SerializeField] private LayerMask groundLayers = 1;

        [Header("Camera")]
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float freeLookSensitivity = 12;
        [Range(0.1f, 1f)]
        [SerializeField] private float aimLookSensitivityMultiplier = .5f;
        [Tooltip("LayerMasks that are valid for aiming")]
        [SerializeField] private LayerMask aimLayerMask = 1;

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
        private Vector3 lastMovement;

        private bool IsGrounded { get { return controller.isGrounded; } }


        // Camera
        private float cameraTargetEulerYaw; // Camera rotation on X Axis
        private float cameraTargetEulerPitch; // Camera rotation on Y Axis


        //private bool IsGrounded { get { return Physics.Raycast(transform.position, Vector3.down, .3f); } }
        //private bool IsGrounded { get { return Physics.CheckSphere(transform.position, controller.radius, groundLayers, QueryTriggerInteraction.Ignore); } }

        private void Start()
        {
            controller = GetComponent<CharacterController>();
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

            //const float minInput = 0.001f;
            //if (Mathf.Abs(inputLook.x) < minInput) inputLook.x = 0;
            //if (Mathf.Abs(inputLook.y) < minInput) inputLook.y = 0;

            cameraTargetEulerYaw += (invertVerticalLook ? inputLook.y : -inputLook.y) * 1 * lookSensitivity;
            cameraTargetEulerYaw = Mathf.Clamp(cameraTargetEulerYaw, BottomClamp, TopClamp);

            cameraTargetEulerPitch += (invertHorizontalLook ? -inputLook.x : inputLook.x) * 1 * lookSensitivity;

            cameraTarget.localRotation = Quaternion.Euler(cameraTargetEulerYaw, cameraTargetEulerPitch, 0);
            
            //cameraTarget.rotation *= Quaternion.AngleAxis(inputLook.x * lookSensitivity, Vector3.up);
        }

        private void HandleMovement()
        {
            float targetSpeed;

            // note from unity's third person template: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            if (inputMovement == Vector2.zero)
                targetSpeed = 0;
            else
            {
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
            }

            Vector2 targetMovement = Vector2.ClampMagnitude(inputMovement * targetSpeed, targetSpeed);
            Vector2 lerpMovement = Vector2.Lerp(lastMovement, targetMovement, acceleration * Time.fixedDeltaTime);
            lastMovement = lerpMovement;

            Vector3 movement = new Vector3(lerpMovement.x, Physics.gravity.y, lerpMovement.y);

            controller.Move(movement * Time.fixedDeltaTime);

            OnMovement?.Invoke(controller.velocity);
            OnGroundedChanged?.Invoke(IsGrounded);
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
