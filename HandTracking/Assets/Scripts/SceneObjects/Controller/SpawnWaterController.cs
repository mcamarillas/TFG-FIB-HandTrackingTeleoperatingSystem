using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaterController : MonoBehaviour
{
    public GameObject m_WaterPrefab;
    public Transform m_Spawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localEulerAngles.z >= 45 && transform.localEulerAngles.z <= 180)
        {
            Instantiate(m_WaterPrefab,m_Spawn.position,m_Spawn.rotation,m_Spawn);
        }
    }
}
