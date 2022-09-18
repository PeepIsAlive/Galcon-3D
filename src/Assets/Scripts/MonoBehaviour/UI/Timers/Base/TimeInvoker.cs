using System;
using UnityEngine;

namespace Galcon
{
    namespace UI
    {
        public sealed class TimeInvoker : MonoBehaviour
        {
            [HideInInspector] public event Action OnSecondTimeUpdateEvent;
            private float _oneSecondTime;

            public static TimeInvoker Instance { get; private set; }

            private void Awake() => InstanceInitialize();

            private void Update() => CountSecond();

            private void CountSecond()
            {
                _oneSecondTime += Time.deltaTime;

                if (_oneSecondTime >= 1f)
                {
                    _oneSecondTime -= 1f;
                    OnSecondTimeUpdateEvent?.Invoke();
                }
            }

            private void InstanceInitialize()
            {
                if (Instance == null)
                {
                    Instance = this;
                }
                else if (Instance == this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
