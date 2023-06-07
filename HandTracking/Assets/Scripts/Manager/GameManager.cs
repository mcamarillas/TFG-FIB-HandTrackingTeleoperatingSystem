using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BackendManager m_UDPRecieve;
    public bool m_Movement1;

    [Header("Reference - Enable Movement")]
    public bool m_EnableMovementX;
    public bool m_EnableMovementY;
    public bool m_EnableMovementZ;

    [Header("Reference - Enable Rotations")]
    public bool m_EnableRotationX;
    public bool m_EnableRotationY;
    public bool m_EnableRotationZ;

    [HideInInspector]
    public float m_LockedXLeft;
    [HideInInspector]
    public float m_LockedYLeft;
    [HideInInspector]
    public float m_LockedZLeft;
    [HideInInspector]
    public float m_LockedXRight;
    [HideInInspector]
    public float m_LockedYRight;
    [HideInInspector]
    public float m_LockedZRight;

    [Header("Reference - Hands")]
    public GameObject[] m_LandmarksRight;
    public GameObject[] m_LandmarksLeft;
    public int[] m_LandmarksUsed;
    public float m_Speed;
    public float zThreshold;

    [Header("Reference - Camera")]
    public Vector3 m_CameraPivot;
    public Transform m_Camera;

    [Header("Reference - Scene")]
    public GameObject m_Cube;
    // Hands private Variables
    bool firstLeft = true;
    bool firstRight = true;
    string[] dataSplitRightPrevious;
    string[] dataSplitLeftPrevious;
    [HideInInspector]
    public int m_Scale;
    [HideInInspector]
    public Vector3 m_PreviousHandDirection = new Vector3(0, 0, 0);
    [HideInInspector]
    public int[] m_PreviousLandmarks;

    public int fact;
    // Camera private Variables
    float m_PreviousRotation;
    bool firstCamera = true;
    bool firstPauseLeft = false;
    bool firstPauseRight = false;
    Vector3 m_OffsetRight;
    Vector3 m_OffsetLeft;


    // Start is called before the first frame update
    void Start()
    {
        m_OffsetRight = new Vector3(0, 0, 0);
        m_OffsetLeft = new Vector3(0, 0, 0);
        m_Scale = 50;
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
        string[] hands = m_UDPRecieve.m_data.Split("Left:");
        if (hands.Length > 1)
        {
            string[] dataSplitRight = hands[0].Replace("[", " ").Replace("]", " ").Split(",");
            string[] dataSplitLeft = hands[1].Replace("[", " ").Replace("]", " ").Split(",");
            
            // Check if it's the first time
            if (firstLeft && dataSplitLeft.Length > 1)
            {
                firstLeft = false;
                dataSplitLeftPrevious = dataSplitLeft;
            }
            if (firstRight && dataSplitRight.Length > 1)
            {
                firstRight = false;
                dataSplitRightPrevious = dataSplitRight;
            }

            // Move Left Arm
            if (dataSplitLeft.Length > 1 && Input.GetKey(KeyCode.LeftAlt))
            {
                for (int i = 0; i < m_LandmarksUsed.Length; ++i)
                {
                    int landmarkIndex = 3 * m_LandmarksUsed[i];
                    //m_Landmarks[i].transform.localPosition = new Vector3(float.Parse(dataSplit[landmarkIndex])/m_Scale, float.Parse(dataSplit[landmarkIndex + 1])/m_Scale, float.Parse(dataSplit[landmarkIndex + 2])/m_Scale);
                    float zz = (Mathf.Abs(float.Parse(dataSplitLeft[landmarkIndex + 2]) - float.Parse(dataSplitLeftPrevious[landmarkIndex + 2])) > zThreshold) ? float.Parse(dataSplitLeftPrevious[landmarkIndex + 2]) : (float.Parse(dataSplitLeft[landmarkIndex + 2]));

                    float x = (m_EnableMovementX) ? float.Parse(dataSplitLeft[landmarkIndex]) : m_LockedXLeft;
                    float y = (m_EnableMovementY) ? float.Parse(dataSplitLeft[landmarkIndex + 1]) : m_LockedYLeft;
                    float z = (m_EnableMovementZ) ? float.Parse(dataSplitLeft[landmarkIndex + 2]) : m_LockedZLeft;

                    if (firstPauseLeft)
                    {
                        firstPauseLeft = false;
                        Vector3 prev = new Vector3(float.Parse(dataSplitLeftPrevious[landmarkIndex]), float.Parse(dataSplitLeftPrevious[landmarkIndex + 1]), float.Parse(dataSplitLeftPrevious[landmarkIndex + 2]));
                        m_OffsetLeft += prev - new Vector3(x, y, z);
                    }

                    if (m_EnableMovementX) m_LockedXLeft = x;
                    if (m_EnableMovementY) m_LockedYLeft = y;
                    if (m_EnableMovementZ) m_LockedZLeft = z;

                    z = (m_Movement1) ? (z + m_OffsetLeft.z) / m_Scale - 2 :  30 - (z + m_OffsetLeft.z) / m_Scale;

                    MoveTowardsTarget(m_LandmarksLeft[i], new Vector3((x + m_OffsetLeft.x)/ m_Scale, (y + m_OffsetLeft.y) / m_Scale, z));
                }
                dataSplitLeftPrevious = dataSplitLeft;
            }

            // Move Right Arm
            if (dataSplitRight.Length > 1 && Input.GetKey(KeyCode.RightAlt))
            {
                for (int i = 0; i < m_LandmarksUsed.Length; ++i)
                {
                    int landmarkIndex = 3 * m_LandmarksUsed[i];
                    
                    //m_Landmarks[i].transform.localPosition = new Vector3(float.Parse(dataSplit[landmarkIndex])/m_Scale, float.Parse(dataSplit[landmarkIndex + 1])/m_Scale, float.Parse(dataSplit[landmarkIndex + 2])/m_Scale);
                    float zz = (Mathf.Abs(float.Parse(dataSplitRight[landmarkIndex + 2]) - float.Parse(dataSplitRightPrevious[landmarkIndex + 2])) > zThreshold) ? float.Parse(dataSplitRightPrevious[landmarkIndex + 2]) : (float.Parse(dataSplitRight[landmarkIndex + 2]));

                    float x = (m_EnableMovementX) ? float.Parse(dataSplitRight[landmarkIndex]) : m_LockedXRight;
                    float y = (m_EnableMovementY) ? float.Parse(dataSplitRight[landmarkIndex + 1]) : m_LockedYRight;
                    float z = (m_EnableMovementZ) ? float.Parse(dataSplitRight[landmarkIndex + 2]) : m_LockedZRight;

                    if (firstPauseRight)
                    {
                        firstPauseRight = false;
                        Vector3 prev = new Vector3(float.Parse(dataSplitRightPrevious[landmarkIndex]), float.Parse(dataSplitRightPrevious[landmarkIndex+1]), float.Parse(dataSplitRightPrevious[landmarkIndex+2]));
                        m_OffsetRight +=  prev - new Vector3(x, y, z);
                        print(m_OffsetRight);
                    }


                    if (m_EnableMovementX) m_LockedXRight = x;
                    if (m_EnableMovementY) m_LockedYRight = y;
                    if (m_EnableMovementZ) m_LockedZRight = z;

                    z = (m_Movement1) ? (z + m_OffsetRight.z) / m_Scale - 2 : 30 - (z + m_OffsetRight.z) / m_Scale;
                    int scale = m_Scale;
                    MoveTowardsTarget(m_LandmarksRight[i], new Vector3((x + m_OffsetRight.x) / scale, (y + m_OffsetRight.y) / scale, z));
                }
                dataSplitRightPrevious = dataSplitRight;

            }

            // Move the Camera
            if (!Input.GetKey(KeyCode.RightAlt) && !Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
            {
                if (dataSplitRight.Length > 1)
                {
                    float rotationAngle = float.Parse(dataSplitRight[0]) / 1280 * 360;
                    if (firstCamera)
                    {
                        firstCamera = false;
                        m_PreviousRotation = rotationAngle;
                    }
                    Vector3 rotationAxis = Vector3.up;
                    m_Camera.RotateAround(m_CameraPivot, rotationAxis, rotationAngle - m_PreviousRotation);
                    m_PreviousRotation = rotationAngle;
                }
            }
        }
    }

    void MoveTowardsTarget(GameObject point, Vector3 target)
    {
        var cc = point.GetComponent<CharacterController>();
        var offset = target - point.transform.position;

        if (offset.magnitude > .0f)
        {
            cc.Move(offset * Time.deltaTime * m_Speed);
        }
    }

    public Vector3 GetRotationAngle(Vector3 point1, Vector3 point2)
    {
        Vector3 currentDirection = point1 - point2;
        float cosTheta = 0;
        if (m_PreviousHandDirection.magnitude * currentDirection.magnitude != 0)
            cosTheta = Vector3.Dot(m_PreviousHandDirection, currentDirection) / (m_PreviousHandDirection.magnitude * currentDirection.magnitude);
        
        m_PreviousHandDirection = currentDirection;
        float angleZ;
        if (currentDirection.y > 0) angleZ = Vector3.Angle(new Vector3(1, 0, 0), currentDirection) - 180;
        else angleZ = (!m_EnableRotationZ) ? 0 : Vector3.Angle(new Vector3(-1, 0, 0), currentDirection);

        float angleY;
        if (currentDirection.x > 0) angleY = Vector3.Angle(new Vector3(0, 0, 1), currentDirection) -180;
        else angleY = (!m_EnableRotationY) ? 0 : Vector3.Angle(new Vector3(0, 0, -1), currentDirection);

        float angleX;
        if (currentDirection.z > 0) angleX = Vector3.Angle(new Vector3(0, 1, 0), currentDirection) -180;
        else angleX = Vector3.Angle(new Vector3(0, -1, 0), currentDirection);

        Vector3 prevRotation = m_Cube.transform.eulerAngles;
        return new Vector3((!m_EnableRotationX) ? prevRotation.x : angleX, (!m_EnableRotationY) ? prevRotation.y : angleY, (!m_EnableRotationZ) ? prevRotation.z : angleZ);
    }
    public Vector3 GetRotationAngle2(Vector3 point1, Vector3 point2, Vector3 right)
    {
        Vector3 rotation = point2 - point1;
        Vector3 prevRotation = right;
        return new Vector3((!m_EnableRotationX) ? prevRotation.x : rotation.x, (!m_EnableRotationY) ? prevRotation.y : rotation.y, (!m_EnableRotationZ) ? prevRotation.z : rotation.z);
    }

    private void PauseGame()
    {
        // Show & Close Pause popup
        if (!PersistentsManager.Instance.m_PopupsManager.IsShown(PopupsManager.POPUP_KEY.CHANGE_SCENE) && !Input.GetKey(KeyCode.RightAlt) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.LeftControl))
        {
            firstPauseLeft = true;
            firstPauseRight = true;
            PersistentsManager.Instance.m_PopupsManager.ShowPausePopup();
        }
        if (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftControl))
        {
            PersistentsManager.Instance.m_PopupsManager.ClosePopup(PopupsManager.POPUP_KEY.PAUSE);
            PersistentsManager.Instance.m_PopupsManager.ClosePopup(PopupsManager.POPUP_KEY.CHANGE_SCENE);
        }
    }

    public void ResetCube()
    {
        m_Cube.transform.position = new Vector3(8.5f,6.75f,1.5f);
        m_Cube.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
