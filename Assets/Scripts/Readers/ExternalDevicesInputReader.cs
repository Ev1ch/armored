using UnityEngine;
using Players.Abstracts;
using UnityEngine.EventSystems;
using Core.Services.Updaters;
using System;

namespace Readers
{
    public class ExternalDevicesInputReader : IEntityInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw(Axe.Horizontal);
        public float VerticalDirection => Input.GetAxisRaw(Axe.Vertical);
        public bool Jump { get; private set; }
        public bool Attack { get; private set; }

        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Attack = false;
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
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

        private bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject();
    }
}
