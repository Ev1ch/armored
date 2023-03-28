using System;
using UnityEngine;

using Players.Abstracts;
using Core.Tools;
using Core.Animation;
using Core.Movement.Data;
using Core.Movement.Controllers;

namespace Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IFullyMovable
    {
        [SerializeField] private AnimationController _animator;
        
        [SerializeField] private DirectionalMoveData _directionalMovementData;

        [SerializeField] private JumpData _jumpData;

        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;
        private DirectionalMover _directionalMovement;
        private Jumper _jumper;

        public void MoveHorizontally(float direction) => _directionalMovement.MoveHorizontally(direction);

        public void MoveVertically(float direction)
        {
            if (_jumper.IsJumping)
            {
                return;
            }

            _directionalMovement.MoveVertically(direction);
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _directionalMovement = new DirectionalMover(_rigidbody, _directionalMovementData);
            _jumper = new Jumper(_rigidbody, _jumpData, _directionalMovementData.MaximumSize);
        }

        private void Update()
        {
            if (_jumper.IsJumping)
            {
                _jumper.UpdateJump();
            }

            UpdateAnimations();
            UpdateCameras();
        }

        private void UpdateAnimations()
        {
            if (!_animator)
            {
                return;
            }

            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _directionalMovement.IsMoving);
            _animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);
        }

        private void UpdateCameras()
        {
            foreach(var cameraPair in _cameras.DirectionCameras)
            {
                cameraPair.Value.enabled = cameraPair.Key == _directionalMovement.Direction;
            }
        }

        public void Jump() => _jumper.Jump();

        public void StartAttack()
        {
            if (!_animator.PlayAnimation(AnimationType.Attack, true))
            {
                return;
            }

            _animator.ActionRequested += Attack;
            _animator.AnimationEnded += EndAttack;
        }

        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void EndAttack()
        {
            _animator.ActionRequested -= Attack;
            _animator.AnimationEnded -= EndAttack;
            _animator.PlayAnimation(AnimationType.Attack, false);
        }
    }
}