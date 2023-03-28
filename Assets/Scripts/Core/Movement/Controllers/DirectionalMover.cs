using UnityEngine;
using Core.Enums;
using Core.Movement.Data;

namespace Core.Movement.Controllers
{
    internal class DirectionalMover
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly DirectionalMoveData _directionalMovementData;
        private readonly float _sizeModificator;

        private Vector2 _movement;

        public Direction Direction { get; private set; }
        public bool IsMoving => _movement.magnitude > 0;

        public DirectionalMover(Rigidbody2D rigidbody, DirectionalMoveData directionalMovementData)
        {
            _rigidbody = rigidbody;
            _transform = rigidbody.transform;
            _directionalMovementData = directionalMovementData;
            var positionDifference = _directionalMovementData.MaximumVerticalPosition - _directionalMovementData.MinimumVerticalPosition;
            var sizeDifference = _directionalMovementData.MaximumSize - _directionalMovementData.MinimumSize;
            _sizeModificator = sizeDifference / positionDifference;
            UpdateSize();
        }

        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            var velocity = _rigidbody.velocity;
            velocity.x = direction * _directionalMovementData.HorizontalSpeed;
            _rigidbody.velocity = velocity;
        }

        public void MoveVertically(float direction)
        {
            _movement.y = direction;
            var velocity = _rigidbody.velocity;
            velocity.y = direction * _directionalMovementData.VerticalSpeed;
            _rigidbody.velocity = velocity;

            if (direction == 0)
            {
                return;
            }

            var verticalPosition = Mathf.Clamp(_rigidbody.position.y, _directionalMovementData.MinimumVerticalPosition, _directionalMovementData.MaximumVerticalPosition);
            _rigidbody.position = new Vector2(_rigidbody.position.x, verticalPosition);
            UpdateSize();
        }

        private void UpdateSize()
        {
            var verticalDelta = _directionalMovementData.MaximumVerticalPosition - _transform.position.y;
            var currentSizeModificator = _directionalMovementData.MinimumSize + _sizeModificator * verticalDelta;
            _transform.localScale = Vector2.one * currentSizeModificator;
        }

        private void SetDirection(float direction)
        {
            if (Direction == Direction.Right && direction < 0 || Direction == Direction.Left && direction > 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            _transform.Rotate(0, 180, 0);
            Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
        }
    }
}
