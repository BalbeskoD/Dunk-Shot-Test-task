using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public void CloseSettingsPanel()
    {
        gameObject.SetActive(false);
    }
}
