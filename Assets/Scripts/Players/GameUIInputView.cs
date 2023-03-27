using Players.Abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    internal class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _attackButton;
        
        public float HorizontalDirection => _joystick.Horizontal;
        public float VerticalDirection => _joystick.Vertical;
        public bool Jump { get; private set; }
        public bool Attack { get; private set; }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Attack = false;
        } 

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jump = true);
            _attackButton.onClick.AddListener(() => Attack = true);
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
            _attackButton.onClick.RemoveAllListeners();
        }
    }
}
