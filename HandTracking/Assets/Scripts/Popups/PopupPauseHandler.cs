using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPauseHandler : PopupHandler
{
    public SliderController m_Slider;

    [Header("Movement Toggles")]
    public ToggleController m_MovementX;
    public ToggleController m_MovementY;
    public ToggleController m_MovementZ;

    [Header("Rotation Toggles")]
    public ToggleController m_RotationX;
    public ToggleController m_RotationY;
    public ToggleController m_RotationZ;

    void Start()
    {
        m_Slider.SetUp("Scale","1:50", 50);
        SetupToggles();
    }

    public void SetupToggles()
    {
        m_MovementX.Setup(true);
        m_MovementY.Setup(true);
        m_MovementZ.Setup(true);

        m_RotationX.Setup(false);
        m_RotationY.Setup(false);
        m_RotationZ.Setup(false);
    }
    // Called by UI
    public void ChangeScale()
    {
        PersistentsManager.Instance.m_GameManager.m_Scale = (int) (m_Slider.m_Value * 40 + 50);
    }

    public void OnResetButtonClicked()
    {
        PersistentsManager.Instance.m_GameManager.ResetCube();
    }

    public void OnChangeSceneButtonClicked()
    {
        PersistentsManager persistentsManager = PersistentsManager.Instance;
        persistentsManager.m_PopupsManager.ClosePopup(PopupsManager.POPUP_KEY.PAUSE);
        PopupChangeSceneHandler popup = persistentsManager.m_PopupsManager.GetPopupByKey(PopupsManager.POPUP_KEY.CHANGE_SCENE).GetComponent<PopupChangeSceneHandler>();
        popup.Setup();
        persistentsManager.m_PopupsManager.ShowChangeScenePopup();
    }

    public void OnEnableRotation(int axis)
    {
        switch (axis)
        {
            case 0:
                m_RotationX.OnToggle();
                PersistentsManager.Instance.m_GameManager.m_EnableRotationX = m_RotationX.m_IsOn;
                break;
            case 1:
                m_RotationY.OnToggle();
                PersistentsManager.Instance.m_GameManager.m_EnableRotationY = m_RotationY.m_IsOn;
                break;
            case 2:
                m_RotationZ.OnToggle();
                PersistentsManager.Instance.m_GameManager.m_EnableRotationZ = m_RotationZ.m_IsOn;
                break;
        }
    }
    public void OnEnableMovement(int axis)
    {
        switch (axis)
        {
            case 0:
                m_MovementX.OnToggle();
                PersistentsManager.Instance.m_GameManager.m_EnableMovementX = m_MovementX.m_IsOn;
                break;
            case 1:
                m_MovementY.OnToggle();
                PersistentsManager.Instance.m_GameManager.m_EnableMovementY = m_MovementY.m_IsOn;
                break;
            case 2:
                m_MovementZ.OnToggle();
                PersistentsManager.Instance.m_GameManager.m_EnableMovementZ = m_MovementZ.m_IsOn;
                break;
        }
    }
}
