using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        public BaseInput input;

        private readonly int f_speed_forward = Animator.StringToHash("f_speed_forward");
        private readonly int f_speed_right = Animator.StringToHash("f_speed_right");

        private Animator animator;

        private void OnEnable() => input.OnInputMovement += OnMovement;

        private void OnDisable() => input.OnInputMovement -= OnMovement;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnMovement(Vector2 input)
        {
            animator.SetFloat(f_speed_right, input.x);
            animator.SetFloat(f_speed_forward, input.y);
        }
    }
}
