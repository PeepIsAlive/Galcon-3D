using Galcon.Controllers;
using System;
using UnityEngine;

namespace Galcon
{
    namespace UI
    {
        [RequireComponent(typeof(TimeInvoker))]
        public sealed class GameTimer : MonoBehaviour
        {
            private DisplayTextUpdater _textUpdater;

            public float TotalSecond { get; private set; }
            private TimeInvoker _timeInvoker => TimeInvoker.Instance;
            private MenuController MenuController => MenuController.Instance;
            private GameController GameController => GameController.Instance;

            public void Initialize()
            {
                _textUpdater = GetComponent<DisplayTextUpdater>();
                _textUpdater.Initialize();

                TotalSecond = 0f;
            }

            private void OnEnable()
            {
                _timeInvoker.OnSecondTimeUpdateEvent += IncreaseTotalSeconds;
                _timeInvoker.OnSecondTimeUpdateEvent += UpdateText;
            }

            private void OnDisable()
            {
                _timeInvoker.OnSecondTimeUpdateEvent -= IncreaseTotalSeconds;
                _timeInvoker.OnSecondTimeUpdateEvent -= UpdateText;
            }

            private void IncreaseTotalSeconds()
            {
                if (MenuController.IsPause || GameController.IsGameOver) { return; }

                ++TotalSecond;
            }

            public static TimeSpan SecondsToTimeSpan(float totalSeconds)
            {
                int minutes = (int) totalSeconds / 60;
                int seconds = (int) totalSeconds - minutes * 60;

                return new TimeSpan(0, minutes, seconds);
            }

            private void UpdateText()
            {
                if (_textUpdater == null) { return; }

                _textUpdater.OnTextUpdateEvent?.Invoke($"{SecondsToTimeSpan(TotalSecond)}");
            }
        }
    }
}
