using System.Collections.Generic;
using System;
using UnityEngine;

namespace Core.Paralax
{
    public class InfiniteParalaxEffect : MonoBehaviour
    {
        [SerializeField] private List<ParalaxPart> _parts;
        [SerializeField] private Transform _target;

        private List<InfiniteParalaxLayer> _layers;
        private float _previousTargetPosition;

        private void Start()
        {
            _previousTargetPosition = _target.position.x;
            _layers = new();

            foreach (var part in _parts)
            {
                Transform layerParent = new GameObject().transform;
                layerParent.transform.parent = transform;
                part.SpriteRenderer.transform.parent = layerParent;
                InfiniteParalaxLayer infiniteParalaxLayer = new(part.SpriteRenderer, part.Speed, layerParent);
                _layers.Add(infiniteParalaxLayer);
            }
        }

        private void LateUpdate()
        {
            foreach (var layer in _layers)
            {
                layer.UpdateLayer(_target.position.x, _previousTargetPosition);
            }

            _previousTargetPosition = _target.position.x;
        }

        [Serializable]
        private class ParalaxPart
        {
            [field: SerializeField]
            public SpriteRenderer SpriteRenderer
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
