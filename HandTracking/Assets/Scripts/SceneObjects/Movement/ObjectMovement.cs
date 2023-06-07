using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private int m_CurrentCollisions = 0;
    public float m_Speed;
    private Rigidbody m_Rigidbody;
    private bool m_ReduceVelocity = false;
    private Vector3 m_StaticRotation = new Vector3(0,0,0);

    private Vector3 initialForward;
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        initialForward = transform.forward;
    }
    private void FixedUpdate()
    {
        if (m_CurrentCollisions == 0) Move(Vector3.zero, false);
        //transform.forward = initialForward;
    }

    public void Move(Vector3 direction,bool isRight)
    {
        switch (m_CurrentCollisions)
        {
            case 0:
                m_StaticRotation = transform.rotation.eulerAngles;
                m_Rigidbody.isKinematic = false;
                m_ReduceVelocity = true;
                m_Rigidbody.freezeRotation = false;
                //StartCoroutine(ReduceVelocity(m_Rigidbody.velocity));
                break;
            case 1:
                m_StaticRotation = transform.rotation.eulerAngles;
                m_Rigidbody.isKinematic = false;
                m_Rigidbody.freezeRotation = true;
                m_ReduceVelocity = false;
                direction.y = 0;
                m_Rigidbody.velocity = direction * m_Speed;
                break;
            case 2:
                PersistentsManager persistentsManager = PersistentsManager.Instance;
                Vector3 point1 = (isRight) ? persistentsManager.m_GameManager.m_LandmarksRight[0].transform.position : persistentsManager.m_GameManager.m_LandmarksLeft[0].transform.position;
                Vector3 point2 = (isRight) ? persistentsManager.m_GameManager.m_LandmarksRight[1].transform.position : persistentsManager.m_GameManager.m_LandmarksLeft[1].transform.position;
                m_Rigidbody.isKinematic = true;
                m_Rigidbody.freezeRotation = true;
                Vector3 rotation = persistentsManager.m_GameManager.GetRotationAngle2(point1, point2,transform.right);
                transform.position = ObjectMove(point1, point2);
                //transform.rotation = Quaternion.Euler(rotation - m_StaticRotation);
                /*
                RotateAround(transform.position,Vector3.right,rotation.x - m_StaticRotation.x);
                RotateAround(transform.position,Vector3.up, rotation.y - m_StaticRotation.y);
                RotateAround(transform.position,Vector3.forward, rotation.z - m_StaticRotation.z);
                */
                transform.right = rotation;
                m_ReduceVelocity = false;
                m_StaticRotation = rotation;
                break;
        }
    }

    IEnumerator ReduceVelocity(Vector3 initialVelocity)
    {
        Vector3 velocityReduction = initialVelocity * 0.1f;
        while(m_Rigidbody.velocity.magnitude > 0 && m_ReduceVelocity)
        {
            m_Rigidbody.velocity -= velocityReduction;
            yield return null;
        }
        m_Rigidbody.velocity = new Vector3(0,0,0);
    }

    private Vector3 ObjectMove(Vector3 point1, Vector3 point2)
    {
        return (point1 + point2) / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Right") || other.CompareTag("Left")) m_CurrentCollisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Right") || other.CompareTag("Left")) m_CurrentCollisions--;
    }

    public void RotateAround(Vector3 point, Vector3 axis, float angle)
    {
        Vector3 direction = transform.position - point;
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);

        // Calculate the new position after rotation
        Vector3 newPosition = point + rotation * direction;
        Vector3 translation = newPosition - transform.position;

        // Apply the rotation and translation
        
        Debug.Log(axis + " " + angle + " " + Quaternion.ToEulerAngles(rotation) + " " + Quaternion.ToEulerAngles(transform.rotation) + " " + Quaternion.ToEulerAngles(rotation * transform.rotation));
        transform.rotation = rotation * transform.rotation;
        transform.position += translation;
    }
}
