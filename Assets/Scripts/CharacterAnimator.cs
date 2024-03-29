using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private BaseController controller;

        private readonly int f_speed_forward = Animator.StringToHash("f_speed_forward");
        private readonly int f_speed_right = Animator.StringToHash("f_speed_right");
        private readonly int b_grounded = Animator.StringToHash("b_grounded");

        private Animator animator;

        private void OnEnable()
        {
            controller.OnMovement += OnMovement;
            controller.OnGroundedChanged += OnGroundedChanged;
        }

        private void OnDisable()
        {
            controller.OnMovement -= OnMovement;
            controller.OnGroundedChanged -= OnGroundedChanged;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnMovement(Vector3 input)
        {
            animator.SetFloat(f_speed_right, input.x);
            animator.SetFloat(f_speed_forward, input.z);
        }

        private void OnGroundedChanged(bool isGrounded)
        {
            animator.SetBool(b_grounded, isGrounded);
        }
    }
}
