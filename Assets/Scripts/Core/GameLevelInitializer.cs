using UnityEngine;
using System.Collections.Generic;
using Players;
using Readers;
using Players.Abstracts;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameUIInputView _gameUIInputView;

        private ExternalDevicesInputReader _externalDevicesInputReader;
        private PlayerBrain _playerBrain;

        private bool _onPause;

        private void Awake()
        {
            _externalDevicesInputReader = new ExternalDevicesInputReader();
            _playerBrain = new PlayerBrain(_player, new List<IEntityInputSource>()
            {
                _gameUIInputView,
                _externalDevicesInputReader,
            });
        }

        private void Update()
        {
            if (_onPause)
            {
                return;
            }

            _externalDevicesInputReader.OnUpdate();
        }

        private void FixedUpdate()
        {
            _playerBrain.OnFixedUpdate();
        }
    }
}
