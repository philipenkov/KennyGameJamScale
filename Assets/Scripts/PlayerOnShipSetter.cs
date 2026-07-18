using System.Collections;
using UnityEngine;

public class PlayerOnShipSetter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform cameraDefaultParent;
    [SerializeField] private float flightDuration = 2.0f;
    [SerializeField] private ShipSelection shipSelection;
    [SerializeField] private GameLoop gameLoop;

    private PlayerFPS _playerFPS;
    private Vector3 _cameraOnPlayerPlacePosition;
    private void Awake()
    {
        _playerFPS = player.GetComponent<PlayerFPS>();
        player.SetActive(false);
        shipSelection.ShipClicked += PlacePlayerOnShip;
    }

    private void PlacePlayerOnShip(Ship ship)
    {
        player.transform.position = ship.PlayerSpawnTransform.position;
        StartCoroutine(CameraFlight());
    }

    private IEnumerator CameraFlight()
    {
        Transform cameraTransform = Camera.main != null ? Camera.main.transform : null;
        if (cameraTransform == null)
        {
            yield break;
        }

        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;
        
        float elapsed = 0f;

        while (elapsed < flightDuration)
        {
            elapsed += Time.deltaTime;
            float clamp = Mathf.Clamp01(elapsed / flightDuration);
            float easedClamp = Mathf.SmoothStep(0f, 1f, clamp);

            _cameraOnPlayerPlacePosition = _playerFPS.CameraPlace.position;
            Quaternion targetRotation = _playerFPS.CameraPlace.rotation;

            cameraTransform.position = Vector3.Lerp(startPosition, _cameraOnPlayerPlacePosition, easedClamp);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedClamp);

            yield return null;
        }

        cameraTransform.position = _playerFPS.CameraPlace.position;
        cameraTransform.rotation = _playerFPS.CameraPlace.rotation;
        cameraTransform.SetParent(_playerFPS.CameraPlace);

        player.SetActive(true);
        gameLoop.GoToNextState();
    }

    private void OnDestroy()
    {
        shipSelection.ShipClicked -= PlacePlayerOnShip;
    }
}