using TMPro;
using Galcon.UI;
using UnityEngine;

namespace Galcon
{
    namespace Controllers
    {
        public sealed class UIController : MonoBehaviour, IController
        {
            [SerializeField] private Canvas _gameOverCanvas;
            [SerializeField] private TextMeshProUGUI _gameOverText;
            [SerializeField] private GameTimer _gameTimer;

            public void Initialize() => _gameTimer?.Initialize();

            public void ShowGameOverCanvas()
            {
                if (_gameOverCanvas == null) { return; }

                _gameOverCanvas.gameObject.SetActive(true);
                _gameOverText.text = $"{GameTimer.SecondsToTimeSpan(_gameTimer.TotalSecond)}";
            }

            private void Awake() => Initialize();
        }
    }
}
