using Galcon.Controllers;
using Galcon.UI;
using UnityEngine;

namespace Galcon
{
    namespace Objects
    {
        [RequireComponent(typeof(Planet))]
        public sealed class SpaceShipCounter : MonoBehaviour
        {
            private ObjectTextUpdater _textUpdater;
            private Planet _planet;
            private const int _maxShipsAmount = 70;

            public int ShipAmount { get; private set; }
            public int MaxShipAmount => _maxShipsAmount;
            private TimeInvoker _timeInvoker => TimeInvoker.Instance;
            private GameController GameController => GameController.Instance;
            private MenuController MenuController => MenuController.Instance;

            public void Initialize()
            {
                _planet = GetComponent<Planet>();
                _textUpdater = GetComponentInChildren<ObjectTextUpdater>();
                _textUpdater.Initialize(ShipAmount.ToString());
            }

            public void OnStartSetShipAmount(int amount)
            {
                ShipAmount = (amount > _maxShipsAmount) ? _maxShipsAmount : amount;
                _textUpdater.OnTextUpdateEvent?.Invoke(ShipAmount.ToString());
            }

            public void DecreaseShipAmount(int amount = 1)
            {
                ShipAmount -= amount;
                _textUpdater.OnTextUpdateEvent?.Invoke(ShipAmount.ToString());
            }

            public void IncreaseShipAmount()
            {
                ++ShipAmount;

                if (ShipAmount > _maxShipsAmount)
                {
                    ShipAmount = _maxShipsAmount;
                }

                _textUpdater.OnTextUpdateEvent?.Invoke(ShipAmount.ToString());
            }

            private void OnEnable() => _timeInvoker.OnSecondTimeUpdateEvent += ProduceShip;

            private void OnDisable() => _timeInvoker.OnSecondTimeUpdateEvent -= ProduceShip;

            private void ProduceShip()
            {
                if (MenuController.IsPause || GameController.IsGameOver) { return; }

                if (_planet.State == PlanetState.Captured || _planet.State == PlanetState.Hostiled)
                {
                    if ((ShipAmount + GameController.GameData.ShipsPerSecond) > _maxShipsAmount)
                    {
                        ShipAmount = _maxShipsAmount;
                    }
                    else
                    {
                        ShipAmount += GameController.GameData.ShipsPerSecond;
                    }

                    _textUpdater.OnTextUpdateEvent?.Invoke(ShipAmount.ToString());
                }
            }
        }
    }
}