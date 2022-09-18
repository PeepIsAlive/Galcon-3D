using UnityEngine;
using UnityEngine.Events;

namespace Galcon
{
    namespace UI
    {
        public abstract class TextUpdater : MonoBehaviour
        {
            [HideInInspector] public UnityEvent<string> OnTextUpdateEvent;

            public abstract void Initialize();
            public virtual void Initialize(string text) { }
            protected abstract void UpdateText(string text);
        }
    }
}
