using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class PlayerInput : BaseInput
    {
        [Serializable]
        public enum MovementState
        {
            Walk,
            Run,
            Sprint
        }

        [SerializeField] private float walkSpeed = 1.2f;
        [SerializeField] private float runSpeed = 4.5f;
        [SerializeField] private float sprintSpeed = 6.3f;
        [SerializeField] private MovementState movementState = MovementState.Run;

        private Vector2 inputMovement;

        private void Update()
        {
            inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            switch (movementState)
            {
                case MovementState.Walk:
                    inputMovement = Vector2.ClampMagnitude(inputMovement * walkSpeed, walkSpeed);
                    break;
                case MovementState.Run:
                    inputMovement = Vector2.ClampMagnitude(inputMovement * runSpeed, runSpeed);
                    break;
                case MovementState.Sprint:
                    inputMovement = Vector2.ClampMagnitude(inputMovement * sprintSpeed, sprintSpeed);
                    break;
                default:
                    break;
            }

            OnInputMovement?.Invoke(inputMovement);
        }
    } 
}
