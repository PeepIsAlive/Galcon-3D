using UnityEngine;
using Galcon.Objects;
using Galcon.Loaders;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Galcon
{
    namespace Controllers
    {
        public sealed class PlanetsController : MonoBehaviour, IController
        {
            [HideInInspector] public UnityEvent OnCapturingPlanetEvent;
            private PoolPlanetsMonoBehaviour<Planet> _poolPlanets;
            private const float _posY = -9.4f;
            private const float _maxSize = 2.5f;
            private const float _minSize = 1.2f;

            public static PlanetsController Instance { get; private set; }
            public bool IsAllPlanetCaptured => CheckAllPlanetForCapture();
            private Camera _mainCamera => Camera.main;
            private Vector3 _mainCameraPos => _mainCamera.transform.position;
            private GameController GameController => GameController.Instance;

            public void Initialize()
            {
                InstanceInitialize();

                Planet planet;

                planet = Loader.LoadMonoBehaviour<Planet>("Objects/Planet");
                _poolPlanets = new PoolPlanetsMonoBehaviour<Planet>(planet, Vector3.zero, GameController.GameData.TotalPlanets, true);

                ResizePlanets();
                RelocatePlanets();
                MakeCapturePlanetsOnStart();
                MakeHostilePlanetOnStart();
            }

            public bool TryGetPlanet(PlanetState state, out Planet planet)
            {
                planet = null;
                List<int> indexesArray = new List<int>();
                int index = 0;

                for (int i = 0; i < _poolPlanets.ObjectsAmount; ++i)
                {
                    planet = _poolPlanets[i].GetComponent<Planet>();

                    if (planet.State == state)
                    {
                        indexesArray.Add(i);
                    }
                }

                index = Random.Range(1, indexesArray.Count) - 1;

                return _poolPlanets[index].GetComponent<Planet>();
            }

            public bool HasPlanet(PlanetState state)
            {
                foreach (Planet planet in _poolPlanets.List)
                {
                    if (planet.State == state)
                    {
                       return true;
                    }
                }

                return false;
            }

            public bool HasSelectedPlanet(out Planet selectedPlanet)
            {
                selectedPlanet = null;

                foreach (Planet planet in _poolPlanets.List)
                {
                    if (planet.State == PlanetState.Captured)
                    {
                        if (planet.isSelect)
                        {
                            selectedPlanet = planet;
                            return true;
                        }
                    }
                }

                return false;
            }

            public void DeselectTheAllPlanet()
            {
                foreach (Planet planet in _poolPlanets.List)
                {
                    if (planet.State == PlanetState.Captured)
                    {
                        planet.Deselect();
                    }
                }
            }

            private bool CheckAllPlanetForCapture()
            {
                int amountCapturedPlanet = 0;

                foreach (Planet planet in _poolPlanets.List)
                {
                    if (planet.State == PlanetState.Neutral)
                    {
                        return false;
                    }

                    ++amountCapturedPlanet;
                }

                return true;
            }

            private void RelocatePlanets()
            {
                float maxPosZ = _mainCamera.orthographicSize - _minSize;
                float maxPosX = (maxPosZ * 2) - _minSize;

                for (int i = 0; i < _poolPlanets.ObjectsAmount; ++i)
                {
                    float radius = 0f, posX = 0f, posZ = 0f;
                    Vector3 spawnPosition = Vector3.zero;

                    if (i > 0)
                    {
                        bool isCountPosition = true;
                        int numberOfMismatches = 0;

                        do
                        {
                            CalculatePosition(i, ref radius, ref posZ, ref posX, ref spawnPosition);

                            for (int j = 0; j <= i; ++j)
                            {
                                float distance = Vector3.Distance(_poolPlanets[j].transform.position, spawnPosition);
                                float sumRadius = (_poolPlanets[j].Radius + _poolPlanets[i].Radius);

                                if (distance >= sumRadius)
                                {
                                    ++numberOfMismatches;

                                    if (numberOfMismatches == i)
                                    {
                                        isCountPosition = false;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        } while (isCountPosition);
                    }
                    else
                    {
                        CalculatePosition(i, ref radius, ref posZ, ref posX, ref spawnPosition);
                    }

                    _poolPlanets[i].transform.position = spawnPosition;
                    _poolPlanets[i].GetComponent<Planet>().MakeNeutral();
                }

                void CalculatePosition(int i, ref float radius, ref float posZ, ref float posX, ref Vector3 spawnPosition)
                {
                    radius = _poolPlanets[i].Radius;
                    posZ = Random.Range(_mainCameraPos.z - (maxPosZ - radius), _mainCameraPos.z + (maxPosZ - radius));
                    posX = Random.Range(_mainCameraPos.x - (maxPosX - radius), _mainCameraPos.x + (maxPosX - radius));
                    spawnPosition = new Vector3(posX, _posY, posZ);
                }
            }

            private void ResizePlanets()
            {
                foreach (Planet planet in _poolPlanets.List)
                {
                    float size;

                    size = Random.Range(_minSize, _maxSize);
                    size = (float)System.Math.Round(size, 2);

                    planet.transform.localScale = new Vector3(size, size, size);
                }
            }

            private void MakeCapturePlanetsOnStart()
            {
                int sizeArray = GameController.GameData.CapturedPlanetOnStart;
                int[] indexPlanetsArray = new int[sizeArray];

                SelectingIndexes();

                for (int i = 0; i < sizeArray; ++i)
                {
                    _poolPlanets[indexPlanetsArray[i]].GetComponent<Planet>().MakeCaptured();
                }

                void SelectingIndexes()
                {
                    for (int i = 0; i < sizeArray; ++i)
                    {
                        if (i > 0)
                        {
                            bool isSelectingIndex = true;
                            int numberOfMismatches = 0;

                            do
                            {
                                int index = Random.Range(0, GameController.GameData.TotalPlanets - 1);

                                for (int j = 0; j <= i; ++j)
                                {
                                    if (index != indexPlanetsArray[j])
                                    {
                                        ++numberOfMismatches;
                                        indexPlanetsArray[i] = index;

                                        if (numberOfMismatches == i)
                                        {
                                            isSelectingIndex = false;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            } while (isSelectingIndex);
                        }
                    }
                }
            }

            private void MakeHostilePlanetOnStart()
            {
                int planetsAmount = GameController.GameData.HostilePlanetOnStart;

                do
                {
                    int index = Random.Range(0, GameController.GameData.TotalPlanets - 1);
                    Planet planet = _poolPlanets[index].GetComponent<Planet>();

                    if (planet.State == PlanetState.Neutral)
                    {
                        planet.MakeHostile();
                        --planetsAmount;
                    }

                } while (planetsAmount > 0);
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