using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupsManager : MonoBehaviour
{
    public enum POPUP_KEY 
    { 
        PAUSE = 0,
        CHANGE_SCENE = 1
    };

    [System.Serializable]
    public class  dict
    {
        public POPUP_KEY key;
        public GameObject value;
    };
    public List<dict> m_PopupsList;
    public List<GameObject> m_PopupsShown = new List<GameObject>();

    public GameObject GetPopupByKey(POPUP_KEY key)
    {
        foreach(dict popup in m_PopupsList)
        {
            if (popup.key == key) return popup.value;
        }
        Debug.LogError("There's no popup with the given key");
        return null;
    }

    public bool IsShown(POPUP_KEY key)
    {
        return m_PopupsShown.Contains(GetPopupByKey(key));
    }

    public void ShowPausePopup()
    {
        GameObject popup = GetPopupByKey(POPUP_KEY.PAUSE);
        if (popup == null) return;
        if (!m_PopupsShown.Contains(popup))
        {
            m_PopupsShown.Add(popup);
            popup.SetActive(true);
        }
    }

    public void ShowChangeScenePopup()
    {
        GameObject popup = GetPopupByKey(POPUP_KEY.CHANGE_SCENE);
        if (popup == null) return;
        if (!m_PopupsShown.Contains(popup))
        {
            m_PopupsShown.Add(popup);
            popup.SetActive(true);
        }
    }

    public void CloseAllPopups()
    {
        foreach(GameObject popup in m_PopupsShown)
        {
            ClosePopup(popup);
        }
    }
    public void ClosePopup(GameObject popup)
    {
        if (popup != null)
        {
            popup.SetActive(false);
            m_PopupsShown.Remove(popup);
        }
    }

    public void ClosePopup(POPUP_KEY key)
    {
        GameObject popup = GetPopupByKey(key);
        if (m_PopupsShown.Contains(popup))
        {
            m_PopupsShown.Remove(popup);
            popup.GetComponent<PopupHandler>().ClosePopup();
        }
    }
}
