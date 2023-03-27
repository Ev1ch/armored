using System.Collections.Generic;
using System.Linq;
using Players.Abstracts;

namespace Players
{
    public class PlayerBrain
    {
        private readonly Player _player;
        private readonly List<IEntityInputSource> _inputSources;

        public PlayerBrain(Player player, List<IEntityInputSource> inputSources)
        {
            _player = player;
            _inputSources = inputSources;
        }

        public void OnFixedUpdate()
        {
            _player.MoveHorizontally(GetHorizontalDirection());
            _player.MoveVertically(GetVerticalDirection());

            if (IsJump)
            {
                _player.Jump();
            }

            if (IsAttack)
            {
                _player.StartAttack();
            }

            foreach (var inputSource in _inputSources)
            {
                inputSource.ResetOneTimeActions();
            }
        }

        private float GetHorizontalDirection()
        {
            foreach (var inputSource in _inputSources)
            {
                if (inputSource.HorizontalDirection == 0)
                {
                    continue;
                }

                return inputSource.HorizontalDirection;
            }

            return 0;
        }

        private float GetVerticalDirection()
        {
            foreach (var inputSource in _inputSources)
            {
                if (inputSource.VerticalDirection == 0)
                {
                    continue;
                }

                return inputSource.VerticalDirection;
            }

            return 0;
        }

        private bool IsJump => _inputSources.Any(inputSource => inputSource.Jump);

        private bool IsAttack => _inputSources.Any(inputSource => inputSource.Attack);
    }
}
