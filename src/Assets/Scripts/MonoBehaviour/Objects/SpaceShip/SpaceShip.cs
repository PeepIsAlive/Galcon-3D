using Galcon.Loaders;
using Galcon.SO;
using UnityEngine;
using UnityEngine.AI;

namespace Galcon
{
    namespace Objects
    {
        [RequireComponent(typeof(NavMeshAgent))]
        public sealed class SpaceShip : MonoBehaviour
        {
            private MeshRenderer _meshRenderer;
            private NavMeshAgent _navMeshAgent;
            private Planet _targetPlanet;
            private PlanetsData _data;

            public PlanetState State { get; private set; }

            public void FixedUpdate() => CheckDistanceToTarget();

            public void SetTarget(Planet srcPlanet, Planet targetPlanet)
            {
                State = srcPlanet.State;
                _targetPlanet = targetPlanet;

                SetMaterial();
                _navMeshAgent.SetDestination(targetPlanet.transform.position);

                void SetMaterial()
                {
                    if (_meshRenderer == null) { return; }

                    if (State == PlanetState.Captured)
                    {
                        _meshRenderer.material = _data.CapturedMaterial;
                    }
                    else if (State == PlanetState.Hostiled)
                    {
                        _meshRenderer.material = _data.HostileMaterial;
                    }
                }
            }

            private void OnEnable() => Initialize();

            private void Initialize()
            {
                GetComponents();

                _data = Loader.LoadScriptableObject<PlanetsData>("Data/PlanetsData");
                State = PlanetState.Captured;

                void GetComponents()
                {
                    _meshRenderer = GetComponentInChildren<MeshRenderer>();
                    _navMeshAgent = GetComponent<NavMeshAgent>();
                }
            }

            private void CheckDistanceToTarget()
            {
                if (_targetPlanet == null) { return; }

                float distance = Vector3.Distance(_targetPlanet.transform.position, transform.position);
                distance = Mathf.Abs(distance);

                if (distance <= ((_targetPlanet.Radius <= 0.9f) ? 1.2f : 1.5f))
                {
                    HandleTheCollision();
                }

                void HandleTheCollision()
                {
                    if (_targetPlanet.ShipAmount == 0)
                    {
                        if (State == PlanetState.Captured)
                        {
                            _targetPlanet.MakeCaptured();
                        }
                        else if (State == PlanetState.Hostiled)
                        {
                            _targetPlanet.MakeHostile();
                        }
                    }

                    if (_targetPlanet.State == PlanetState.Captured)
                    {
                        if (State == PlanetState.Captured)
                        {
                            _targetPlanet.IncreaseShipAmount();
                        }
                        else if (State == PlanetState.Hostiled)
                        {
                            _targetPlanet.DecreaseShipAmount();
                        }
                    }
                    else if (_targetPlanet.State == PlanetState.Hostiled)
                    {
                        if (State == PlanetState.Captured)
                        {
                            _targetPlanet.DecreaseShipAmount();
                        }
                        else if (State == PlanetState.Hostiled)
                        {
                            _targetPlanet.IncreaseShipAmount();
                        }
                    }
                    else
                    {
                        _targetPlanet.DecreaseShipAmount();
                    }

                    gameObject.SetActive(false);
                }
            }
        }
    }
}
