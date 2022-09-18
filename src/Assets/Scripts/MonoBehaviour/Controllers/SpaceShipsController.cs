using UnityEngine;
using Galcon.Objects;
using Galcon.Loaders;
using System.Collections.Generic;

namespace Galcon
{
    namespace Controllers
    {
        public sealed class SpaceShipsController : MonoBehaviour, IController
        {
            [SerializeField] private List<SpaceShip> _shipsList;
            private PoolPlanetsMonoBehaviour<SpaceShip> _poolShips;
            private const float _posY = 9.4f;

            public static SpaceShipsController Instance { get; private set; }
            private GameController GameController => GameController.Instance;

            public void Initialize()
            {
                InstanceInitialize();

                SpaceShip ship;

                ship = Loader.LoadMonoBehaviour<SpaceShip>("Objects/SpaceShip");
                _poolShips = new PoolPlanetsMonoBehaviour<SpaceShip>(_shipsList, Vector3.zero, false);
            }

            public void MoveShips(Planet srcPlanet, Planet targetPlanet)
            {
                SpaceShip ship;
                Vector3 movePosition;
                float posX, posZ;
                int maxAmount;

                maxAmount = srcPlanet.ShipAmount / GameController.GameData.DividerShipSentForCapture;
                posX = srcPlanet.transform.position.x - srcPlanet.Radius;
                posZ = srcPlanet.transform.position.z - srcPlanet.Radius;
                movePosition = new Vector3(posX, _posY, posZ);

                for (int i = 0; i < maxAmount; ++i)
                {
                    ship = _poolShips.GetObject(true);
                    ship.transform.position = movePosition;

                    ship.SetTarget(srcPlanet, targetPlanet);
                }

                srcPlanet.DecreaseShipAmount(srcPlanet.ShipAmount / GameController.GameData.DividerShipSentForCapture);
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
