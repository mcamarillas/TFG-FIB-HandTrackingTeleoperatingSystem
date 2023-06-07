using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    public virtual void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
