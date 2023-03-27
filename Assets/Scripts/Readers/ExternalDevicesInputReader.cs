using UnityEngine;
using Players.Abstracts;
using UnityEngine.EventSystems;

namespace Readers
{
    public class ExternalDevicesInputReader : IEntityInputSource
    {
        public float HorizontalDirection => Input.GetAxisRaw(Axe.Horizontal);
        public float VerticalDirection => Input.GetAxisRaw(Axe.Vertical);
        public bool Jump { get; private set; }
        public bool Attack { get; private set; }

        public void OnUpdate()
        {
            if (Input.GetButtonDown(ControlButton.Jump))
            {
                Jump = true;
            }

            if (!IsPointerOverUi() && Input.GetButtonDown(ControlButton.Fire1))
            {
                Attack = true;
            }
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Attack = false;
        }

        private bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject();
    }
}
