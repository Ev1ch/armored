﻿using UnityEngine;

using Players;

namespace Readers
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private float _horizontalDirection;
        private float _verticalDirection;

        private void Update()
        {
            _horizontalDirection = Input.GetAxisRaw(Axe.Horizontal);
            _verticalDirection = Input.GetAxisRaw(Axe.Vertical);

            if (Input.GetButtonDown(Button.Jump))
            {
                _player.Jump();
            }
        }

        private void FixedUpdate()
        {
            _player.MoveHorizontally(_horizontalDirection);
            _player.MoveVertically(_verticalDirection);
        }
    }
}
