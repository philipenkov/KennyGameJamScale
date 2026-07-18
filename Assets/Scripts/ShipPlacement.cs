using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipPlacement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Board board;
    [SerializeField] private LayerMask boardLayer;

    private bool isActive;

    private void Start()
    {
        isActive = true; //TODO: Заменить на вызов откуда-то
    }

    private void Update()
    {
        if (isActive)
            RaycastToField();
    }

    private void RaycastToField()
    {
        Vector2 mousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, boardLayer))
        {
            var cellPosition = board.WorldToCell(hit.point);
            Cell cell = board.GetCell(cellPosition);

            if (cell == null)
            {
                return;
            }
            
            Debug.Log(cellPosition);
        }
    }
}
