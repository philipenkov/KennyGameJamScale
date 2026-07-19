using System.Collections;
using UnityEngine;

namespace UIScripts
{
    public class ResultPanel : PoppingUIPanel
    {
        [SerializeField] private Vector3 startScale = Vector3.zero;
        [SerializeField] private Vector3 endScale = Vector3.one;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private CanvasGroup canvasGroup;

        private Coroutine _animationCoroutine;
        private bool _isShown;

        private void Awake()
        {
            transform.localScale = startScale;
            canvasGroup.interactable = false;
        }

        public override void Show()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(AnimateScale(transform.localScale, endScale));
            _isShown = true;
            canvasGroup.interactable = true;
        }

        public override void Hide()
        {
            if (!_isShown)
                return;

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(AnimateScale(transform.localScale, startScale));
            _isShown = false;
            canvasGroup.interactable = false;
        }

        private IEnumerator AnimateScale(Vector3 fromScale, Vector3 toScale)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float clamp = Mathf.Clamp01(elapsed / duration);
                float easedClamp = Mathf.SmoothStep(0f, 1f, clamp);
                
                transform.localScale = Vector3.Lerp(fromScale, toScale, easedClamp);
                yield return null;
            }

            transform.localScale = toScale;
            _animationCoroutine = null;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}