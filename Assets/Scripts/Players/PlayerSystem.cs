using System.Collections.Generic;
using Players.Abstracts;

namespace Players
{
    internal class PlayerSystem
    {
        private Player _player;
        private PlayerBrain _playerBrain;

        public PlayerSystem(Player player, List<IEntityInputSource> inputSources)
        {
            _player = player;
            _playerBrain = new PlayerBrain(_player, inputSources);
        }
    }
}
