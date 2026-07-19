using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts
{
    public class GameRestartButton : MonoBehaviour
    {
        [SerializeField] private Button restartButton;

        private void Start()
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(RestartGame);
        }
    }
}
