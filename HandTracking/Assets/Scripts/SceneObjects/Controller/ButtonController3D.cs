using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController3D : MonoBehaviour
{

    public GameObject m_PortalEffect;
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
        if(other.CompareTag("SceneObject"))
        {
            m_PortalEffect.SetActive(true);
        }
    }
}
