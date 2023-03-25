using Unity.VisualScripting;
using UnityEngine;

using Players.Abstracts;
using Core.Tools;
using Core.Enums;
using Players.Animation;
using System;

namespace Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IFullyMovable
    {
        [SerializeField] private AnimationController _animator;

        [Header("Horizontal Movement")]
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private Direction _direction;

        [Header("Vertical Movement")]
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _maximumSize;
        [SerializeField] private float _maximumVerticalPosition;
        [SerializeField] private float _minimumSize;
        [SerializeField] private float _minimumVerticalPosition;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityScale;
        [SerializeField] private SpriteRenderer _shadow;
        [SerializeField][Range(0, 1)] private float _shadowSizeModificator;
        [SerializeField][Range(0, 1)] private float _shadowAlphaModificator;

        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;
        private float _sizeModificator;
        private bool _isJumping;
        private float _startJumpVerticalPostion;
        private Vector2 _shadowLocalPosition;
        private float _shadowVerticalPosition;
        private Vector2 _shadowLocalScale;
        private Color _shadowColor;

        private Vector2 _movement;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _shadowLocalPosition = _shadow.transform.localPosition;
            _shadowLocalScale = _shadow.transform.localScale;
            _shadowColor = _shadow.color;
            var positionDifference = _maximumVerticalPosition - _minimumVerticalPosition;
            var sizeDifference = _maximumSize - _minimumSize;
            _sizeModificator = sizeDifference / positionDifference;
            UpdateSize();
        }

        private void Update()
        {
            if (_isJumping)
            {
                UpdateJump();
            }

            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            if (!_animator)
            {
                return;
            }

            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Run, _movement.magnitude > 0);
            _animator.PlayAnimation(AnimationType.Jump, _isJumping);
        }

        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            var velocity = _rigidbody.velocity;
            velocity.x = direction * _horizontalSpeed;
            _rigidbody.velocity = velocity;
        }

        public void MoveVertically(float direction)
        {
            if (_isJumping)
            {
                return;
            }

            _movement.y = direction;
            var velocity = _rigidbody.velocity;
            velocity.y = direction * _verticalSpeed;
            _rigidbody.velocity = velocity;

            if (direction == 0)
            {
                return;
            }

            var verticalPosition = Mathf.Clamp(transform.position.y, _minimumVerticalPosition, _maximumVerticalPosition);
            _rigidbody.position = new Vector2(_rigidbody.position.x, verticalPosition);
            UpdateSize();
        }

        public void Jump()
        {
            if (_isJumping)
            {
                return;
            }

            _isJumping = true;
            var jumpModificator = transform.localScale.y / _maximumSize;
            _rigidbody.AddForce(_jumpForce * jumpModificator * Vector2.up);
            _rigidbody.gravityScale = _gravityScale * jumpModificator;
            _startJumpVerticalPostion = transform.position.y;
            _shadowVerticalPosition = _shadow.transform.position.y;
        }

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

        private void UpdateSize()
        {
            var verticalDelta = _maximumVerticalPosition - transform.position.y;
            var currentSizeModificator = _minimumSize + _sizeModificator * verticalDelta;
            transform.localScale = Vector2.one * currentSizeModificator;
        }

        private void SetDirection(float direction)
        {
            if (_direction == Direction.Right && direction < 0 || _direction == Direction.Left && direction > 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;

            foreach (var cameraPair in _cameras.DirectionCameras)
            {
                cameraPair.Value.enabled = cameraPair.Key == _direction;
            }
        }

        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _rigidbody.position.y <= _startJumpVerticalPostion)
            {
                ResetJump();
                return;
            }

            _shadow.transform.position = new Vector2(_shadow.transform.position.x, _shadowVerticalPosition);
            var distance = transform.position.y - _startJumpVerticalPostion;
            _shadow.color = _shadowColor.WithAlpha(_shadowColor.a - distance * _shadowAlphaModificator);
            _shadow.transform.localScale = _shadowLocalScale * (1 + _shadowSizeModificator * distance);
        }

        private void ResetJump()
        {
            _isJumping = false;
            _shadow.transform.localPosition = _shadowLocalPosition;
            _shadow.transform.localScale = _shadowLocalScale;
            _shadow.color = _shadowColor;
            _rigidbody.position = new Vector2(_rigidbody.position.x, _startJumpVerticalPostion);
            _rigidbody.gravityScale = 0;
        }
    }
}