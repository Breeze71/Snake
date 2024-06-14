using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateToTarget : MonoBehaviour
{
    public float rotationSpeed;
    public float moveSpeed;
    private Vector2 direction;
    private KaofishActions inputActions;

    void Awake()
    {
        inputActions = new KaofishActions();
    }

    void OnEnable()
    {
        inputActions.KPlayer.Enable();
    }

    void OnDisable()
    {
        inputActions.KPlayer.Disable();
    }

    void Update()
    {
        Vector2 mousePosition = inputActions.KPlayer.Look.ReadValue<Vector2>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        direction = (Vector2)(worldPosition - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = Vector2.MoveTowards(transform.position, cursorPos, moveSpeed * Time.deltaTime);
    }
}