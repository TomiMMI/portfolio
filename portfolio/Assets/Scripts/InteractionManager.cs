using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Cinemachine;

public class InteractionManager : MonoBehaviour
{
    private Camera cameraAnimation;
    private bool IsActive;
    private InteractMap inputActions;
    public event EventHandler<onSelectChangeEventArgs> onSelectChange;
    public GameObject infoUI;

    public TMP_Text UI;

    public GameObject boutonRetourBibli;

    [SerializeField] private GameObject camera;

    [SerializeField] private GameObject cameraPrincipale;
    [SerializeField] private GameObject cameraBibliothèque;
    [SerializeField] private GameObject cameraBac;
    [SerializeField] private GameObject cameraCible;
    [SerializeField] private GameObject cameraMiroir;
    [SerializeField] private GameObject cameraFenetre;

    public class onSelectChangeEventArgs : EventArgs
    {
        public Transform selection;
    }
    [SerializeField] private LayerMask layer;
    private Transform lastSelected = null;
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
            if (lastSelected.CompareTag("Cible"))
            {
                SwitchToCible();
                infoUI = lastSelected.GetComponent<ShowUI>().infoUI;
                StartCoroutine("WaitAndShow");
            }
            if (lastSelected.CompareTag("Bac"))
            {
                SwitchToBac();
                infoUI = lastSelected.GetComponent<ShowUI>().infoUI;
                StartCoroutine("WaitAndShow");
            }
            if (lastSelected.CompareTag("Fenetre"))
            {
                SwitchToFenetre();
                infoUI = lastSelected.GetComponent<ShowUI>().infoUI;
                StartCoroutine("WaitAndShow");
            }
            if (lastSelected.CompareTag("Miroir"))
            {
                SwitchToMiroir();
                infoUI = lastSelected.GetComponent<ShowUI>().infoUI;
                StartCoroutine("WaitAndShow");
            }
            if (lastSelected.CompareTag("Pharminov") || lastSelected.CompareTag("Folio"))
            {
                infoUI = lastSelected.GetComponent<ShowUI>().infoUI;
                ShowProject();
            }
            lastSelected.GetComponent<ShowName>().UI.transform.parent.gameObject.SetActive(false);
            lastSelected = null;
            onSelectChange?.Invoke(this, new onSelectChangeEventArgs
            {
                selection = lastSelected
            });
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToPrincipale();
        }
        if (IsActive)
        {
            mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = cameraAnimation.ScreenToWorldPoint(mousePos);
            Physics.Raycast(transform.position, mousePos - transform.position, out RaycastHit raycast, 15f, layer); ;
            if (raycast.transform != lastSelected)
            {
                if(raycast.transform == null) {
                    if (lastSelected.GetComponent<ShowName>())
                    {
                        lastSelected.GetComponent<ShowName>().UI.transform.parent.gameObject.SetActive(false);
                    }
                }
                if (lastSelected == null)
                {
                    if (raycast.transform.GetComponent<ShowName>())
                    {
                        raycast.transform.GetComponent<ShowName>().UI.transform.parent.gameObject.SetActive(true);
                    }
                }
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
        boutonRetourBibli.SetActive(true);
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
        if (infoUI)
        {
            Hide();
        }
        boutonRetourBibli.SetActive(false);
        cameraPrincipale.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        cameraBibliothèque.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        cameraBac.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        cameraCible.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        layer = LayerMask.GetMask("Interactible");
        cameraAnimation.transform.position = cameraPrincipale.transform.position;
        cameraAnimation.transform.eulerAngles = cameraPrincipale.transform.eulerAngles;
        transform.position = cameraPrincipale.transform.position;
        transform.eulerAngles = cameraPrincipale.transform.eulerAngles;
    }
    public void SwitchToBac()
    {
        cameraPrincipale.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        cameraBac.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        layer = LayerMask.GetMask("Projet");
        cameraAnimation.transform.position = cameraBac.transform.position;
        transform.position = cameraBac.transform.position;
        transform.eulerAngles = cameraBac.transform.eulerAngles;
        cameraAnimation.transform.eulerAngles = cameraBac.transform.eulerAngles;
    }
    public void SwitchToCible()
    {
        cameraPrincipale.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        cameraCible.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        layer = LayerMask.GetMask("Projet");
        cameraAnimation.transform.position = cameraCible.transform.position;
        transform.position = cameraCible.transform.position;
        transform.eulerAngles = cameraCible.transform.eulerAngles;
        cameraAnimation.transform.eulerAngles = cameraCible.transform.eulerAngles;
    }
    public void SwitchToMiroir()
    {
        cameraPrincipale.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        cameraMiroir.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        layer = LayerMask.GetMask("Projet");
        cameraAnimation.transform.position = cameraMiroir.transform.position;
        transform.position = cameraMiroir.transform.position;
        transform.eulerAngles = cameraMiroir.transform.eulerAngles;
        cameraAnimation.transform.eulerAngles = cameraMiroir.transform.eulerAngles;
    }
    public void SwitchToFenetre()
    {
        cameraPrincipale.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        cameraFenetre.GetComponent<CinemachineVirtualCamera>().Priority = 1;
        layer = LayerMask.GetMask("Projet");
        cameraAnimation.transform.position = cameraFenetre.transform.position;
        transform.position = cameraFenetre.transform.position;
        transform.eulerAngles = cameraFenetre.transform.eulerAngles;
        cameraAnimation.transform.eulerAngles = cameraFenetre.transform.eulerAngles;
    }
    public IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("bbb");
        infoUI.SetActive(true);
    }
    public void Show()
    {
        infoUI.SetActive(true);
    }
    public void Hide()
    {
        infoUI.SetActive(false);
    }
    public void HideBibliButton()
    {
        boutonRetourBibli.SetActive(false);
    }
    public void ShowBibliButton()
    {
        boutonRetourBibli.SetActive(true);
    }
    public void ShowProject()
    {
        layer = LayerMask.GetMask("Nothing");
        Show();
        boutonRetourBibli.SetActive(false);
    }
    public void HideProject()
    {
        layer = LayerMask.GetMask("Projet");
        Hide();
        boutonRetourBibli.SetActive(true);
    }

    public void GoToPharminov()
    {
        Application.OpenURL("https://github.com/TomiMMI/Visite_Virtuelle_SAE");
    }
    public void GoToFolio()
    {
        Application.OpenURL("https://github.com/TomiMMI/portfolio");
    }
    public void GoToCodeMonkey()
    {
        Application.OpenURL("https://unitycodemonkey.com/index.php");
    }
    public void GoToUnity()
    {
        Application.OpenURL("https://unity.com/fr/roadmap/unity-platform");
    }
}
