using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDoorAnimation : MonoBehaviour
{
    [SerializeField]private Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("aaa");
        doorAnimator.SetTrigger("Door Reached");
    }
}
