using System;

namespace Core.Services.Updaters
{
    public interface IProjectUpdater
    {
        public event Action UpdateCalled;
        public event Action FixedUpdatedCalled;
        public event Action LateUpdateCalled;
    }
}
