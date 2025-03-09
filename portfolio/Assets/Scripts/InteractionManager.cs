using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionManager : MonoBehaviour
{
    private Camera cam;
    private bool IsActive;
    private InteractMap inputActions;
    public event EventHandler<onSelectChangeEventArgs> onSelectChange;

    public class onSelectChangeEventArgs : EventArgs
    {
        public Transform selection;
    }
    [SerializeField] private LayerMask layer;
    private Transform lastSelected = null;

    private void Awake()
    {
        inputActions = new InteractMap();
        inputActions.Enable();
        inputActions.InteractionMap.Interact.performed += Interact_performed;
        cam = Camera.main;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(lastSelected != null)
        {
            lastSelected.GetComponent<LightSelected>().enabled = false;
            lastSelected = null;
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = cam.ScreenToWorldPoint(mousePos);
            Physics.Raycast(transform.position, mousePos - transform.position, out RaycastHit raycast, 15f, layer); ;
            if (raycast.transform != lastSelected)
            {
                lastSelected = raycast.transform;
                onSelectChange?.Invoke(this, new onSelectChangeEventArgs
                {
                    selection = lastSelected
                });
            }
            Debug.DrawRay(transform.position, mousePos-transform.position,Color.green);

        }

    }
    public InteractMap GetInputActions()
    {
        return inputActions;
    }

    public void SetState(bool value)
    {
        IsActive = value;
    }
}
