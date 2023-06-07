using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [Range(0, 1)]
    public float m_Value;
    public Image m_Fill;
    public GameObject m_Circle;

    public TextMeshProUGUI m_DescriptionText;
    public TextMeshProUGUI m_ValueText;

    float m_PreviousAngle = 0;

    // Update is called once per frame


    public void SetUp(string description, string text, float value)
    {
        m_DescriptionText.text = description;
        m_ValueText.text = text;

        m_Value = (value - 50) / 40;
        float angleDegrees = m_Value * 360;
        m_Fill.fillAmount = m_Value;
        SetCirclePosition(angleDegrees);
    }

    public void OnSliderMove(bool isDragging)
    {
        float angleDegrees = Vector3.Angle(Vector3.down, Input.mousePosition - transform.position);
        angleDegrees = (Input.mousePosition.x <= transform.position.x) ? angleDegrees : 360 - angleDegrees;

        // Can't pass from 0 to 360 degrees and conversely
        bool negativeOverflow = isDragging && m_PreviousAngle < 30 && angleDegrees > 90;
        bool positiveOverflow = isDragging && m_PreviousAngle > 330 && angleDegrees < 270;
        if (negativeOverflow) angleDegrees = 0;
        if (positiveOverflow) angleDegrees = 360;

        m_Value = (angleDegrees) / 360;
        m_Fill.fillAmount = m_Value;

        m_ValueText.text = "1:" + ((int)(m_Value * 40 + 50)).ToString();
        SetCirclePosition(angleDegrees);

        m_PreviousAngle = angleDegrees;
    }

    public void SetCirclePosition(float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.PI / 180.0f;
        float x = Mathf.Sin(angleRadians) * 180;
        float y = Mathf.Cos(angleRadians) * 180;

        m_Circle.transform.localPosition = new Vector3(-x, -y, transform.position.z);
    }
}
