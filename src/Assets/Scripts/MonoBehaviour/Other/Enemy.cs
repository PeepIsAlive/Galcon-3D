using Galcon.Controllers;
using System.Collections;
using Galcon.Objects;
using UnityEngine;

namespace Galcon
{
    public sealed class Enemy : MonoBehaviour
    {
        private Planet _planet;

        private PlanetsController PlanetsController => PlanetsController.Instance;
        private SpaceShipsController SpaceShipsController => SpaceShipsController.Instance;

        private void OnEnable()
        {
            _planet = GetComponent<Planet>();

            if (_planet.State == PlanetState.Hostiled)
            {
                StartCoroutine(AttackPlanetRoutine());
            }
        }

        private IEnumerator AttackPlanetRoutine()
        {
            Planet srcPlanet, targetPlanet;
            float seconds = Random.Range(3f, 10f);

            yield return new WaitForSecondsRealtime(seconds);

            GetPlanets();
            SpaceShipsController.MoveShips(srcPlanet, targetPlanet);

            if (_planet.State == PlanetState.Hostiled)
            {
                yield return StartCoroutine(AttackPlanetRoutine());
            }
            else
            {
                yield break;
            }

            void GetPlanets()
            {
                PlanetsController.TryGetPlanet(PlanetState.Hostiled, out srcPlanet);
                if (PlanetsController.HasPlanet(PlanetState.Neutral))
                {
                    PlanetsController.TryGetPlanet(PlanetState.Neutral, out targetPlanet);
                }
                else
                {
                    PlanetsController.TryGetPlanet(PlanetState.Captured, out targetPlanet);
                }
            }
        }
    }
}
