using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class ExitButton : MonoBehaviour
    {
        [SerializeField] private Button exitButton;

        private void Start()
        {
            exitButton.onClick.AddListener(HandleExitButton);
        }

        private void HandleExitButton()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            exitButton.onClick.RemoveListener(HandleExitButton);
        }
    }
}
