using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public bool m_IsOn;
    [Header("Background")]
    public GameObject m_Background;
    public Color m_TurnOffColor;
    public Color m_TurnOnColor;

    [Header("Handle")]
    public GameObject m_Handle;
    public Color m_HandleColor;

    // Start is called before the first frame update
    public void Setup(bool isOn)
    {
        m_IsOn = isOn;
        Turn(true);
        m_Handle.GetComponent<Image>().color = m_HandleColor;
    }
    
    public void OnToggle()
    {
        m_IsOn = !m_IsOn;
        Turn();
    }

    public void Turn(bool setup = false)
    {
        RectTransform rt = m_Handle.GetComponent<RectTransform>();
        Vector2 currentPosition = rt.anchoredPosition;
        if (!setup) currentPosition.x = -currentPosition.x;
        else currentPosition.x = (m_IsOn) ? -currentPosition.x : currentPosition.x; 
        rt.anchoredPosition = currentPosition;

        Color color = (m_IsOn) ? m_TurnOnColor : m_TurnOffColor;
        m_Background.GetComponent<Image>().color = color;
    }
}
