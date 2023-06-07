using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneSlotHandler : MonoBehaviour, IPointerClickHandler
{
    public SceneSlotDef m_SceneSlotDef;
    public TextMeshProUGUI m_Title;
    public Image m_Image;


    // Start is called before the first frame update
    public void Setup(SceneSlotDef sceneSlotDef)
    {
        m_SceneSlotDef = sceneSlotDef;
        m_Title.text = sceneSlotDef.m_Title;
        m_Image.sprite = sceneSlotDef.m_Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupChangeSceneHandler changeScenePopup = PersistentsManager.Instance.m_PopupsManager.GetPopupByKey(PopupsManager.POPUP_KEY.CHANGE_SCENE).GetComponent<PopupChangeSceneHandler>();
        changeScenePopup.SetupScenePreview(m_SceneSlotDef);
        changeScenePopup.ShowScenePreview(true);
        changeScenePopup.SetCurrentSlot(this);
    }
}
