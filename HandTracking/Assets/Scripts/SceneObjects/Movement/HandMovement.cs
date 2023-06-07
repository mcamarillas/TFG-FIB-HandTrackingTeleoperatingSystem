using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().material.SetColor("_HandColor", new Color(0, 0, 255, 255));
    }
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().material.SetColor("_HandColor", new Color(255, 0, 0, 255));
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.SetColor("_HandColor", new Color(0, 0, 255, 255));
    }
}
