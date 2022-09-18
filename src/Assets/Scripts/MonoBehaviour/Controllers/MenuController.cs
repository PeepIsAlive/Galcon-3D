using Galcon.Managers;
using UnityEngine;

namespace Galcon
{
    namespace Controllers
    {
        public sealed class MenuController : MonoBehaviour
        {
            [SerializeField] private Canvas _pauseMenuCanvas;
            private GameControls _controls;
            private const string _mainSceneName = "Main";
            private const string _menuSceneName = "Menu";
            private const string _difficultyLevelsPath = "Data/GameData/";
            private readonly string[] _difficultyLevelArray = {"Default", "Medium", "MorePlanets"};

            public static MenuController Instance { get; private set; }
            public bool IsPause { get; private set; }
            public static string DownloadableDifficultyLevel { get; private set; }
            private ScenesManager ScenesManager => ScenesManager.Instance;
            private GameController GameController => GameController.Instance;

            public void SetDefaultDiffLevel()
            {
                DownloadableDifficultyLevel = _difficultyLevelsPath + _difficultyLevelArray[0];
            }

            public void SetMediumDiffLevel()
            {
                DownloadableDifficultyLevel = _difficultyLevelsPath + _difficultyLevelArray[1];
            }

            public void SetMorePlanetsDiffLevel()
            {
                DownloadableDifficultyLevel = _difficultyLevelsPath + _difficultyLevelArray[2];
            }

            public void Play() => ScenesManager.AsyncLoadScene(_mainSceneName);

            public void BackToMenu()
            {
                IsPause = false;
                ScenesManager.AsyncLoadScene(_menuSceneName);
            }

            public void Quit() => ScenesManager.Quit();

            private void Awake() => Initialize();

            private void OnEnable()
            {
                _controls = new GameControls();
                _controls.Game.OpenPauseMenu.performed += _ => OpenPauseMenu();
                _controls.Enable();
            }

            private void OnDisable()
            {
                _controls.Game.OpenPauseMenu.performed -= _ => OpenPauseMenu();
                _controls.Disable();
            }

            private void OpenPauseMenu()
            {
                if (_pauseMenuCanvas == null) { return; }

                if (!ScenesManager.InMenu && !GameController.IsGameOver)
                {
                    IsPause = !IsPause;
                    _pauseMenuCanvas.gameObject.SetActive(IsPause);
                }
            }

            private void Initialize()
            {
                if (Instance == null)
                {
                    Instance = this;

                    DontDestroyOnLoad(gameObject);

                    if (_pauseMenuCanvas != null)
                    {
                        DontDestroyOnLoad(_pauseMenuCanvas);
                    }
                }
                else if (Instance == this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
