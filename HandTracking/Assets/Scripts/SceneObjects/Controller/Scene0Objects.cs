using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene0Objects : MonoBehaviour
{

    public Color m_InitialColor;
    public GameObject m_PortalEffect;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().material.color = m_InitialColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Right") || other.CompareTag("Left")) m_PortalEffect.SetActive(true);
    }
}
