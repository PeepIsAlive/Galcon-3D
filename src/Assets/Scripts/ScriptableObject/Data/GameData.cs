using UnityEngine;

namespace Galcon
{
    namespace SO
    {
        [CreateAssetMenu(fileName = "New game data", menuName = "Data/Game data", order = 52)]
        public sealed class GameData : ScriptableObject
        {
            [SerializeField] private int _totalPlanets;
            [SerializeField] private int _shipsPerSecond;
            [SerializeField] private int _capturedPlanetOnStart;
            [SerializeField] private int _hostiledPlanetOnStart;
            [SerializeField] private int _shipAmountOnStart;
            [SerializeField] private int _dividerShipSentForCapture;

            public int TotalPlanets => _totalPlanets;
            public int ShipsPerSecond => _shipsPerSecond;
            public int CapturedPlanetOnStart => _capturedPlanetOnStart;
            public int HostilePlanetOnStart => _hostiledPlanetOnStart;
            public int ShipAmountOnStart => _shipAmountOnStart;
            public int DividerShipSentForCapture => _dividerShipSentForCapture;
        }
    }
}
