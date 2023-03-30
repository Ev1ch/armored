using UnityEngine;
using Core.Enums;
using System;

namespace Core.Movement.Data
{
    [Serializable]
    public class DirectionalMoveData
    {
        [field: SerializeField] public float HorizontalSpeed { get; private set; }
        [field: SerializeField] public Direction Direction { get; private set; }

        [field: SerializeField] public float VerticalSpeed { get; private set; }
        [field: SerializeField] public float MaximumSize { get; private set; }
        [field: SerializeField] public float MaximumVerticalPosition { get; private set; }
        [field: SerializeField] public float MinimumSize { get; private set; }
        [field: SerializeField] public float MinimumVerticalPosition { get; private set; }
    }
}
