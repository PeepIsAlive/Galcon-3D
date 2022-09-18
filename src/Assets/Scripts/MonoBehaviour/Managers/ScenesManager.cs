using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Galcon
{
    namespace Managers
    {
        public sealed class ScenesManager : MonoBehaviour
        {
            [SerializeField] private GameObject _loadCanvas;
            [SerializeField] private Image _progressBar;
            private float _targetFillAmount;

            public static ScenesManager Instance { get; private set; }
            public bool IsLoadingScene { get; private set; }
            public bool InMenu => (SceneManager.GetActiveScene().name == "Menu") ? true : false;
            public string CurrentScene => SceneManager.GetActiveScene().name;

            public async void AsyncLoadScene(string name)
            {
                if (!IsLoadingScene)
                {
                    IsLoadingScene = true;
                    ResetProgressBar();

                    var scene = SceneManager.LoadSceneAsync(name);

                    scene.allowSceneActivation = false;
                    _loadCanvas.SetActive(true);

                    do
                    {
                        await Task.Delay(250);
                        _targetFillAmount = scene.progress;

                    } while (scene.progress < 0.9f);

                    await Task.Delay(10);

                    scene.allowSceneActivation = true;

                    await Task.Delay(15);
                    _loadCanvas.SetActive(false);

                    IsLoadingScene = false;
                }

                void ResetProgressBar()
                {
                    if (_progressBar == null) { return; }

                    _targetFillAmount = 0f;
                    _progressBar.fillAmount = 0f;
                }
            }

            public void Quit() => Application.Quit();

            private void Awake() => Initialize();

            private void Update()
            {
                if (_progressBar == null) { return; }

                _progressBar.fillAmount = _targetFillAmount;
            }

            private void Initialize()
            {
                if (Instance == null)
                {
                    Instance = this;
                    IsLoadingScene = false;

                    DontDestroyOnLoad(gameObject);
                    DontDestroyOnLoad(_loadCanvas);
                }
                else if (Instance == this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
