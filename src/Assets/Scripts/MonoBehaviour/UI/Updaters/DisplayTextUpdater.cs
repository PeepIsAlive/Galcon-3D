using TMPro;

namespace Galcon
{
    namespace UI
    {
        public sealed class DisplayTextUpdater : TextUpdater
        {
            private TextMeshProUGUI _textField;

            public override void Initialize() => _textField = GetComponent<TextMeshProUGUI>();

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
