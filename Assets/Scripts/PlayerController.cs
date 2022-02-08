using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : BaseController
    {
        private bool isWalking;
        private bool isSprinting;
        private Vector2 inputMovement;
        private Vector2 lastMovement;
        private CharacterController controller;
        private bool IsGrounded { get { return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, .3f); } }

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
            controller.Move(finalMovement * Time.fixedDeltaTime);

            OnInputMovement?.Invoke(movement);
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
