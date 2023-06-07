using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ObjectMovement objectMovement = hit.collider.GetComponent<ObjectMovement>();
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (objectMovement != null)
        {
            
            bool isRight = this.CompareTag("Right");   
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            objectMovement.Move(forceDirection,isRight);
        }
    }
}
