using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private HintPanel panel;
        [SerializeField] private float animationDuration = 0.3f;

        private bool _isShown;
        private Coroutine _rotationCoroutine;
        
        private void Start()
        {
            button.onClick.AddListener(OnButtonClicked);
            image.rectTransform.localRotation = Quaternion.Euler(0, 0, _isShown ? 0f : 180f);
        }
        
        private void OnButtonClicked()
        {
            if (_isShown)
            {
                _isShown = false;
                panel.Hide();
            }
            else
            {
                _isShown = true;
                panel.Show();
            }
            
            if (_rotationCoroutine != null)
            {
                StopCoroutine(_rotationCoroutine);
            }
            _rotationCoroutine = StartCoroutine(AnimateImage());
        }

        private IEnumerator AnimateImage()
        {
            float elapsed = 0f;
            Quaternion startRotation = image.rectTransform.localRotation;
            
            float targetZ = _isShown ? 0f : 180f;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetZ);

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float clamp = Mathf.Clamp01(elapsed / animationDuration);
                float easedClamp = Mathf.SmoothStep(0f, 1f, clamp);
                
                image.rectTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, easedClamp);
                yield return null;
            }

            image.rectTransform.localRotation = targetRotation;
            _rotationCoroutine = null;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}