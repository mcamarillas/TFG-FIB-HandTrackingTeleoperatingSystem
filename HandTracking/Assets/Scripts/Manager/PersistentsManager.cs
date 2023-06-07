using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentsManager : Singleton<PersistentsManager>
{
    // Start is called before the first frame update

    public GameManager m_GameManager;
    public PopupsManager m_PopupsManager;
    public BackendManager m_BackendManager;
    public SceneManager m_SceneManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
