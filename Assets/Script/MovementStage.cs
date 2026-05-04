using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace Script
{
    public class MovementStage : MonoBehaviour
    {
        private InputActions _inputAction;
        private Animator animator;
        private Vector2 moveInput;

        void Awake()
        {
            _inputAction = new InputActions();
            animator = GetComponent<Animator>();
        }
        void Start()
        {
            _inputAction.Player.Move.performed += OnMove;
            animator = GetComponent<Animator>();
            Debug.Log(animator);
        }
        private void FixedUpdate()
        {
            if (moveInput.y > 0) 
            {
                Debug.Log("Moving Forward");
            }
        }

        private void OnMove(InputAction.CallbackContext context) 
        {
            moveInput = context.ReadValue<Vector2>();
            
            bool isMoving = moveInput.magnitude > 0;
            animator.SetBool("isWalking", isMoving);
        }
        
        void OnEnable()
        {
            _inputAction.Enable();
            _inputAction.Player.Move.performed += OnMove;
            _inputAction.Player.Move.canceled += OnMove;
        }

        void OnDisable()
        {
            _inputAction.Player.Move.performed -= OnMove;
            _inputAction.Player.Move.canceled -= OnMove;
            _inputAction.Disable();
        }
    }
}
