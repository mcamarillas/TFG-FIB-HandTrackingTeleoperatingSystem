using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    public List<SceneSlotDef> m_SceneSlots;
    [SerializeField]
    public List<GameObject> m_Scenes;

    private GameObject m_CurrentScene;

    private void Start()
    {
        m_CurrentScene = m_Scenes[0]; 
    }
    public void ChangeScene(int index)
    {
        m_CurrentScene.SetActive(false);
        m_CurrentScene = m_Scenes[index];
        m_CurrentScene.SetActive(true);
    }
}
