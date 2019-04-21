using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class TriggerValue
    {
        public bool _isAvailable = true;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            private set { _isAvailable = value; }
        }

        private float _timer = 0.0f;
        private float _maxTimer = 0.0f;

        public TriggerValue(float maxTimer)
        {
            _maxTimer = maxTimer;
            _timer = _maxTimer;
        }

        public void Start()
        {
            IsAvailable = false;
        }

        public void Update()
        {
            if (!IsAvailable)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    IsAvailable = true;
                    _timer = _maxTimer;
                }
            }
        }
    }
}
