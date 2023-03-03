
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Paralax
{
    public class ParalaxEffect : MonoBehaviour
    {
        [SerializeField] private List<ParalaxLayer> _layers;
        [SerializeField] private Transform _target;

        private float _previousTargetPosition;

        private void Start()
        {
            _previousTargetPosition = _target.position.x;
        }

        private void LateUpdate()
        {
            float deltaMovement = _previousTargetPosition - _target.position.x;

            foreach (var layer in _layers)
            {
                Vector2 layerPosition = layer.Transform.position;
                layerPosition.x += deltaMovement * layer.Speed;
                layer.Transform.position = layerPosition;
            }

            _previousTargetPosition = _target.position.x;
        }

        [Serializable]
        private class ParalaxLayer
        {
            [field: SerializeField]
            public Transform Transform
            {
                get; private set;
            }
            [field: SerializeField]
            public float Speed
            {
                get; private set;
            }
        }
    }
}
