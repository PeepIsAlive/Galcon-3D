using Galcon.Loaders;
using Galcon.SO;
using UnityEngine;

namespace Galcon
{
    namespace Controllers
    {
        public sealed class GameController : MonoBehaviour, IController
        {
            private UIController _uiController;
            private PlanetsController _planetsController;
            private SpaceShipsController _shipsController;

            public static GameController Instance { get; private set; }
            public bool IsGameOver { get; private set; }
            public GameData GameData { get; private set; }
            private MenuController MenuController => MenuController.Instance;

            public void Initialize()
            {
                if (Instance == null)
                {
                    Instance = this;
                }
                else if (Instance == this)
                {
                    Destroy(gameObject);
                }

                GetComponents();
                LoadDifficultyLevel();
                InitializeComponents();

                void GetComponents()
                {
                    _uiController = GetComponent<UIController>();
                    _planetsController = GetComponent<PlanetsController>();
                    _shipsController = GetComponent<SpaceShipsController>();
                }

                void InitializeComponents()
                {
                    _planetsController.Initialize();
                    _shipsController.Initialize();
                }
            }

            public void Retry() => MenuController.BackToMenu();

            private void Awake() => Initialize();

            private void OnEnable() => _planetsController.OnCapturingPlanetEvent.AddListener(FinishTheGame);

            private void OnDisable() => _planetsController.OnCapturingPlanetEvent.RemoveListener(FinishTheGame);

            private void LoadDifficultyLevel()
            {
                GameData = Loader.LoadScriptableObject<GameData>(MenuController.DownloadableDifficultyLevel);
            }

            private void FinishTheGame()
            {
                IsGameOver = _planetsController.IsAllPlanetCaptured;

                if (IsGameOver)
                {
                    _uiController.ShowGameOverCanvas();
                }
            }
        }
    }
}
