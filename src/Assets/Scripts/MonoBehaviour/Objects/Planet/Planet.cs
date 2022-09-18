using System.Collections;
using Galcon.Controllers;
using Galcon.Loaders;
using Galcon.SO;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Galcon
{
    namespace Objects
    {
        public enum PlanetState
        {
            Neutral,
            Captured,
            Hostiled
        }

        [RequireComponent(typeof(SpaceShipCounter))]
        public sealed class Planet : MonoBehaviour
        {
            private PlanetsData _data;
            private Outline _outline;
            private SpaceShipCounter _shipCounter;
            private MeshRenderer _meshRenderer;

            public PlanetState State { get; private set; }
            public int ShipAmount => _shipCounter.ShipAmount;
            public float Radius => transform.localScale.x / 2;
            public bool isSelect => (_outline.OutlineWidth == _data.SelectWidth) ? true : false;
            private GameController GameController => GameController.Instance;
            private MenuController MenuController => MenuController.Instance;
            private PlanetsController Controller => PlanetsController.Instance;

            public void OnClick()
            {
                if (MenuController.IsPause || GameController.IsGameOver) { return; }

                if (State == PlanetState.Neutral || State == PlanetState.Hostiled)
                {
                    if (Controller.HasSelectedPlanet(out Planet selectedPlanet))
                    {
                        if (selectedPlanet.State == PlanetState.Captured)
                        {
                            StartCoroutine(SelectRoutine());
                            SpaceShipsController.Instance.MoveShips(selectedPlanet, this);
                        }
                    }
                }

                if (State == PlanetState.Captured)
                {
                    if (Controller.HasSelectedPlanet(out Planet selectedPlanet))
                    {
                        if (selectedPlanet != this)
                        {
                            Controller.DeselectTheAllPlanet();
                        }
                    }

                    ChangeOutlineWidth();
                }

                void ChangeOutlineWidth()
                {
                    if (_outline.OutlineWidth == _data.DeselectWidth)
                    {
                        Select();
                    }
                    else
                    {
                        Deselect();
                    }
                }
            }

            public void DecreaseShipAmount(int amount = 1) => _shipCounter.DecreaseShipAmount(amount);

            public void IncreaseShipAmount() => _shipCounter.IncreaseShipAmount();

            public void MakeCaptured()
            {
                State = PlanetState.Captured;
                _meshRenderer.material = _data.CapturedMaterial;

                Controller.OnCapturingPlanetEvent?.Invoke();
            }

            public void MakeNeutral()
            {
                State = PlanetState.Neutral;
                _meshRenderer.material = _data.NeutralMaterial;
            }

            public void MakeHostile()
            {
                State = PlanetState.Hostiled;
                _meshRenderer.material = _data.HostileMaterial;

                gameObject.AddComponent(typeof(Enemy));
            }

            public void Deselect()
            {
                if (_outline == null || _data == null) { return; }

                _outline.OutlineWidth = _data.DeselectWidth;
            }

            private void Select()
            {
                if (_outline == null || _data == null) { return; }

                _outline.OutlineWidth = _data.SelectWidth;
            }

            private IEnumerator SelectRoutine()
            {
                Select();
                yield return new WaitForSecondsRealtime(0.15f);
                Deselect();
                yield break;
            }

            private void Start()
            {
                if (State == PlanetState.Captured || State == PlanetState.Hostiled)
                {
                    _shipCounter.OnStartSetShipAmount(GameController.GameData.ShipAmountOnStart);
                }
                else if (State == PlanetState.Neutral)
                {
                    _shipCounter.OnStartSetShipAmount(Random.Range(1, _shipCounter.MaxShipAmount));
                }
            }

            private void OnEnable() => Initialize();

            private void Initialize()
            {
                GetComponents();

                _data = Loader.LoadScriptableObject<PlanetsData>("Data/PlanetsData");
                State = PlanetState.Neutral;

                if (_outline != null && _data != null)
                {
                    _outline.OutlineWidth = _data.DeselectWidth;
                }

                _shipCounter?.Initialize();

                void GetComponents()
                {
                    _outline = GetComponent<Outline>();
                    _shipCounter = GetComponent<SpaceShipCounter>();
                    _meshRenderer = GetComponentInChildren<MeshRenderer>();
                }
            }
        }
    }
}
