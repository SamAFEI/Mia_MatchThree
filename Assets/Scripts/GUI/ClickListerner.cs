using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class ClickListerner : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Button[] Buttons;

        private void Awake()
        {
            Text = GetComponentInChildren<TextMeshProUGUI>();
            Buttons = GetComponentsInChildren<Button>();
        }
        private void Start()
        {
            foreach (var button in Buttons)
            {
                button.onClick.AddListener(() => OnClick(button));
            }
        }

        private void OnClick(Button _button)
        {
            Text.text += _button.name + "\n";
        }
    }
}
