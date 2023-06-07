using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupChangeSceneHandler : PopupHandler
{
    private List<SceneSlotHandler> m_SceneSlots;
    public GameObject m_SceneSlotPrefab;
    public GameObject m_Content;

    [Header("Reference - Scene Preview")]
    public GameObject m_ScenePreview;
    public TextMeshProUGUI m_ScenePreviewTitle;
    public TextMeshProUGUI m_ScenePreviewDescription;
    public Image m_ScenePreviewImage;

    private SceneSlotHandler m_CurrentSlot;

    public void Setup()
    {
        m_SceneSlots = new List<SceneSlotHandler>();
        foreach (Transform child in m_Content.transform)
        {
            Destroy(child.gameObject);
        }

        List<SceneSlotDef> slots = PersistentsManager.Instance.m_SceneManager.m_SceneSlots;
        foreach(SceneSlotDef slot in slots)
        {
            SceneSlotHandler instance = Instantiate(m_SceneSlotPrefab, m_Content.transform).GetComponent<SceneSlotHandler>();
            m_SceneSlots.Add(instance);
            instance.Setup(slot);
        }
    }
    public void SetupScenePreview(SceneSlotDef sceneSlotDef)
    {
        m_ScenePreviewTitle.text = sceneSlotDef.m_Title;
        m_ScenePreviewDescription.text = sceneSlotDef.m_Description;
        m_ScenePreviewImage.sprite = sceneSlotDef.m_Sprite;
    }

    public void ShowScenePreview(bool value)
    {
        m_ScenePreview.SetActive(value);
    }

    public void SetCurrentSlot(SceneSlotHandler slot)
    {
        m_CurrentSlot = slot;
    }

    public void OnChangeSceneButtonClicked()
    {
        int scene = m_CurrentSlot.m_SceneSlotDef.SceneNumber;
        PersistentsManager.Instance.m_SceneManager.ChangeScene(scene);
    }

    public override void ClosePopup()
    {
        ShowScenePreview(false);
        PersistentsManager.Instance.m_PopupsManager.ClosePopup(PopupsManager.POPUP_KEY.CHANGE_SCENE);
        base.ClosePopup();
    }
}
