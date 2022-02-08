using System;
using System.Collections;
using UnityEngine;

namespace Project
{
    public abstract class BaseController : MonoBehaviour
    {
        public Action<Vector2> OnInputMovement;

        [SerializeField] protected float acceleration = 5f;
        [SerializeField] protected float walkSpeed = 1.2f;
        [SerializeField] protected float runSpeed = 4.5f;
        [SerializeField] protected float sprintSpeed = 6.3f;
        [SerializeField] protected MovementState movementState = defaultMovementState;

        protected const MovementState defaultMovementState = MovementState.Run;
        protected MovementState previousMovementState = defaultMovementState;

        #region States
        public enum MovementState
        {
            Locked,
            Walk,
            Run,
            Sprint
        }

        public enum StanceState
        {
            Stand,
            Crouch,
            Prone
        }

        public enum WeaponState
        {
            Away,
            Down,
            Ready,
            Aiming
        } 
        #endregion

        public void LockMovementState()
        {
            previousMovementState = movementState;
            movementState = MovementState.Locked;
        }

        public void UnlockMovementState()
        {
            movementState = previousMovementState;
        }

        public void ToggleLockMovementState()
        {
            if (movementState == MovementState.Locked)
                UnlockMovementState();
            else
                LockMovementState();
        }

        protected abstract void HandleMovementState();
        protected abstract void HandleStanceState();
        protected abstract void HandleEquipmentState();

    }
}