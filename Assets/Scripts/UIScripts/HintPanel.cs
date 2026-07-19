using System.Collections;
using UnityEngine;

namespace UIScripts
{
    public class HintPanel : PoppingUIPanel
    {
        [SerializeField] private RectTransform startPosition;
        [SerializeField] private RectTransform targetPosition;
        [SerializeField] private RectTransform hidePosition;
        [SerializeField] private float movementDuration;

        private Coroutine _activeMovementCoroutine;
        private RectTransform _rectTransform;
        private bool _isShown;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform != null && startPosition != null)
            {
                _rectTransform.anchoredPosition = startPosition.anchoredPosition;
            }
        }
        
        public override void Show()
        {
            if (_activeMovementCoroutine != null)
            {
                StopCoroutine(_activeMovementCoroutine);
            }
            _activeMovementCoroutine = StartCoroutine(ShowAnimation());
            _isShown = true;
        }

        private IEnumerator ShowAnimation()
        {
            if (_rectTransform == null || startPosition == null || targetPosition == null)
                yield break;

            float elapsed = 0f;
            Vector2 startPos = startPosition.anchoredPosition;
            Vector2 targetPos = targetPosition.anchoredPosition;

            while (elapsed < movementDuration)
            {
                elapsed += Time.deltaTime;
                float clamp = Mathf.Clamp01(elapsed / movementDuration);
                float easedClamp = Mathf.SmoothStep(0f, 1f, clamp);
                
                _rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedClamp);
                yield return null;
            }

            _rectTransform.anchoredPosition = targetPos;
            _activeMovementCoroutine = null;
        }
        

        public override void Hide()
        {
            if (!_isShown)
                return;
            
            if (_activeMovementCoroutine != null)
            {
                StopCoroutine(_activeMovementCoroutine);
            }
            _activeMovementCoroutine = StartCoroutine(HideAnimation());
        }

        private IEnumerator HideAnimation()
        {
            if (_rectTransform == null || targetPosition == null || hidePosition == null || startPosition == null)
                yield break;

            float elapsed = 0f;
            Vector2 startPos = targetPosition.anchoredPosition;
            Vector2 endPos = hidePosition.anchoredPosition;

            while (elapsed < movementDuration)
            {
                elapsed += Time.deltaTime;
                float clamp = Mathf.Clamp01(elapsed / movementDuration);
                float easedClamp = Mathf.SmoothStep(0f, 1f, clamp);
                
                _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedClamp);
                yield return null;
            }

            _rectTransform.anchoredPosition = endPos;
            _rectTransform.anchoredPosition = startPosition.anchoredPosition;
            _activeMovementCoroutine = null;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}