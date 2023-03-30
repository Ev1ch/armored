using Core.Movement.Data;
using UnityEngine;

namespace Core.Movement.Controllers
{
    public class Jumper
    {
        private readonly JumpData _jumpData;
        private readonly Rigidbody2D _rigidbody;
        private readonly float _maxVerticalSize;
        private readonly Transform _transform;
        private readonly Transform _shadowTransform;
        private readonly Color _shadowColor;
        private readonly Vector3 _shadowLocalPosition;
        private readonly Vector3 _shadowLocalScale;

        private float _startJumpingVerticalPosition;
        private float _shadowVerticalPosition;

        public bool IsJumping { get; private set; }

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData, float maxVerticalSize)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
            _maxVerticalSize = maxVerticalSize;
            _shadowTransform = _jumpData.Shadow.transform;
            _shadowColor = _jumpData.Shadow.color;
            _shadowLocalPosition = _shadowTransform.localPosition;
            _shadowLocalScale = _shadowTransform.localScale;
            _transform = _rigidbody.transform;
        }

        public void Jump()
        {
            if (IsJumping)
            {
                return;
            }

            IsJumping = true;
            _startJumpingVerticalPosition = _rigidbody.position.y;
            var jumpModificator = _transform.localScale.y / _maxVerticalSize;
            var currentJumpForce = _jumpData.JumpForce * jumpModificator;
            _rigidbody.gravityScale = _jumpData.GravityScale * jumpModificator;
            _rigidbody.AddForce(Vector2.up * currentJumpForce);
            _shadowVerticalPosition = _shadowTransform.position.y;
        }

        public void UpdateJump()
        {
            if (IsOnGround())
            {
                ResetJump();
                return;
            }

            var distance = _rigidbody.transform.position.y - _startJumpingVerticalPosition;
            _shadowTransform.position = new Vector2(_shadowTransform.position.x, _shadowVerticalPosition);
            _shadowTransform.localScale = _shadowLocalScale * (1 + _jumpData.ShadowSizeModificator * distance);
            var updatedShadowColor = new Color(_shadowColor.r, _shadowColor.g, _shadowColor.b, _shadowColor.a - distance * _jumpData.ShadowAlphaModificator);
            _jumpData.Shadow.color = updatedShadowColor;
        }

        private bool IsOnGround() => Physics2D.Raycast(_transform.position, Vector2.down, 1.2f, _jumpData.GroundMask.value);
            
        private void ResetJump()
        {
            _rigidbody.gravityScale = 0;
            _transform.position = new Vector2(_transform.position.x, _startJumpingVerticalPosition);

            _shadowTransform.localPosition = _shadowLocalPosition;
            _shadowTransform.localScale = _shadowLocalScale;
            _jumpData.Shadow.color = _shadowColor;

            IsJumping = false;
        }
    }
}
