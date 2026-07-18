using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipSelection : MonoBehaviour
{
    public event Action<Ship> ShipClicked;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask shipsLayer;
    [SerializeField] private GameLoop gameLoop;
    
    private bool _isActive;
    private bool _isShipHovered;
    private Ship _hoveredShip;

    private void Start()
    {
        gameLoop.GameStateChanged += HandleGameLoopChanged;
    }

    private void Update()
    {
        if (_isActive)
        {
            DoRaycast();
        }
        else if (_isShipHovered)
        {
            ResetHover();
        }

        if (_isShipHovered)
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                ShipClicked?.Invoke(_hoveredShip);
                ResetHover();
                SwitchSelection(false);
            }
        }
    }

    private void DoRaycast()
    {
        Vector2 mousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, shipsLayer))
        {
            Ship ship = hit.collider.GetComponent<Ship>();
            if (ship != null)
            {
                if (_hoveredShip != ship)
                {
                    _hoveredShip = ship;
                    _isShipHovered = true;
                    
                    _hoveredShip.PlayHoveredAnimation();
                }
                return;
            }
        }
        
        ResetHover();
    }

    private void ResetHover()
    {
        if (_isShipHovered)
        {
            _isShipHovered = false;
            _hoveredShip.ResetHoveredAnimation();
            _hoveredShip = null;
        }
    }

    private void HandleGameLoopChanged(GameState gameState)
    {
        if (gameState != GameState.ShipSelect)
        {
            SwitchSelection(false);
            return;
        }
        

        SwitchSelection(true);
    }

    private void SwitchSelection(bool isOn)
    {
        _isActive = isOn;
    }

    private void OnDestroy()
    {
        gameLoop.GameStateChanged -= HandleGameLoopChanged;
    }
}
