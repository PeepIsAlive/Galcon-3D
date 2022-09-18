using UnityEngine;

namespace Galcon
{
    namespace SO
    {
        [CreateAssetMenu(fileName = "New planets data", menuName = "Data/Planets data", order = 52)]
        public sealed class PlanetsData : ScriptableObject
        {
            [SerializeField] private Material _neutralMaterial;
            [SerializeField] private Material _capturedMaterial;
            [SerializeField] private Material _hostileMaterial;
            public readonly float DeselectWidth = 0f;
            public readonly float SelectWidth = 3f;

            public Material NeutralMaterial => _neutralMaterial;
            public Material CapturedMaterial => _capturedMaterial;
            public Material HostileMaterial => _hostileMaterial;
        }
    }
}
