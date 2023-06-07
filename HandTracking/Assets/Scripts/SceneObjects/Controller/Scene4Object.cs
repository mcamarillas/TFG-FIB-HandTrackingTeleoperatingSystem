using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4Object : MonoBehaviour
{
    int m_count = 0;
    public GameObject m_Effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_count > 20)
            m_Effect.SetActive(true);
            ;
    }

    private void OnTriggerEnter(Collider other)
    {
        m_count++;
    }
}
