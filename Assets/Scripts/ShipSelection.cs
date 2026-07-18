using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipSelection : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask shipsLayer;
    
    private bool isActive;
    private bool isShipHovered;
    private Ship _hoveredShip;

    private void Update()
    {
        if (isActive)
        {
            DoRaycast();
        }
        else if (isShipHovered)
        {
            ResetHover();
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
                    isShipHovered = true;
                    
                    _hoveredShip.PlayHoveredAnimation();
                }
                return;
            }
        }
        
        ResetHover();
    }

    private void ResetHover()
    {
        if (isShipHovered)
        {
            isShipHovered = false;
            _hoveredShip.ResetHoveredAnimation();
            _hoveredShip = null;
            // Здесь при необходимости можно вызвать метод остановки анимации наведения, 
            // например: _hoveredShip.StopHoveredAnimation();
        }
    }

    public void SwitchSelection(bool isOn)
    {
        isActive = isOn;
    }
}
