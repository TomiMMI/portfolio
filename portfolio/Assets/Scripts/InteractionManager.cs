using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class InteractionManager : MonoBehaviour
{
    private Camera cameraAnimation;
    private bool IsActive;
    private InteractMap inputActions;
    public event EventHandler<onSelectChangeEventArgs> onSelectChange;

    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject cameraPrincipale;
    [SerializeField] private GameObject cameraBibliothèque;

    public class onSelectChangeEventArgs : EventArgs
    {
        public Transform selection;
    }
    [SerializeField] private LayerMask layer;
    private Transform lastSelected = null;
    private Camera activeCamera;
    public Vector3 mousePos;
    private void Awake()
    {
        inputActions = new InteractMap();
        inputActions.Enable();
        inputActions.InteractionMap.Interact.performed += Interact_performed;
        cameraAnimation = Camera.main;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(lastSelected != null)
        {
            if (lastSelected.CompareTag("Bibli"))
            {
                SwitchToEtagere();
            }
            lastSelected = null;
            onSelectChange?.Invoke(this, new onSelectChangeEventArgs
            {
                selection = lastSelected
            });
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = cameraAnimation.ScreenToWorldPoint(mousePos);
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
        SwitchCamera();
    }
    private void SwitchCamera()
    {
        camera.SetActive(true);
        cameraAnimation.GetComponent<Animator>().enabled = false;
    }
    public void SwitchToEtagere()
    {
        cameraPrincipale.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        cameraBibliothèque.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        layer = LayerMask.GetMask("Projet");
        cameraAnimation.transform.position = cameraBibliothèque.transform.position;
        transform.position = cameraBibliothèque.transform.position;
        transform.eulerAngles = cameraBibliothèque.transform.eulerAngles;
        cameraAnimation.transform.eulerAngles = cameraBibliothèque.transform.eulerAngles;
    }
    public void SwitchToPrincipale()
    {
        cameraPrincipale.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        cameraBibliothèque.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        layer = LayerMask.GetMask("Interactible");
        cameraAnimation.transform.position = cameraPrincipale.transform.position;
        cameraAnimation.transform.eulerAngles = cameraPrincipale.transform.eulerAngles;
        transform.position = cameraPrincipale.transform.position;
        transform.eulerAngles = cameraPrincipale.transform.eulerAngles;
    }
}
