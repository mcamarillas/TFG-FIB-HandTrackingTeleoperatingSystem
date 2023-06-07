using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("LeftArm") == 1)
        {
            print("hola");
        }
        if (Input.GetAxis("RightArm") == 1)
        {
            print("hola");
        }
        if (Input.GetAxis("CameraArm") == 1)
        {
            print("hola");
        }
    }
}
