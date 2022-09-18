using TMPro;

namespace Galcon
{
    namespace UI
    {
        public sealed class ObjectTextUpdater : TextUpdater
        {
            private TextMeshPro _textField;

            public override void Initialize() => _textField = GetComponent<TextMeshPro>();

            public override void Initialize(string text)
            {
                this.Initialize();
                OnTextUpdateEvent?.Invoke(text);
            }

            private void OnEnable() => OnTextUpdateEvent.AddListener(UpdateText);

            private void OnDisable() => OnTextUpdateEvent.RemoveListener(UpdateText);

            protected override void UpdateText(string text)
            {
                if (_textField == null) { return; }

                _textField.text = text;
            }
        }
    }
}
