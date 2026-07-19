using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class MouseSensitivity : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private PlayerFPS player;
        [SerializeField] private TMP_Text text;

        private const string SensitivityKey = "MouseSensitivity";
        private const float DefaultSensitivity = 0.1f;

        private void Start()
        {
            float savedSensitivity = PlayerPrefs.GetFloat(SensitivityKey, DefaultSensitivity);

            if (slider != null)
            {
                slider.value = savedSensitivity;
                slider.onValueChanged.AddListener(OnSensitivityChanged);
            }

            UpdateSensitivityText(savedSensitivity);
        }

        private void OnSensitivityChanged(float value)
        {
            PlayerPrefs.SetFloat(SensitivityKey, value);

            if (player != null)
            {
                player.MouseSensitivity = value;
            }

            UpdateSensitivityText(value);
        }

        private void UpdateSensitivityText(float value)
        {
            if (text != null)
            {
                text.text = value.ToString("F2");
            }
        }

        public void SaveSettings()
        {
            PlayerPrefs.Save();
        }

        private void OnDestroy()
        {
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(OnSensitivityChanged);
            }
        
            PlayerPrefs.Save();
        }
    }
}