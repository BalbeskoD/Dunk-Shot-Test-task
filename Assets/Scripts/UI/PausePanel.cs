using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;

public class PausePanel : MonoBehaviour
{
   
    private void OnEnable()
    {
        Time.timeScale = 0;
    }




}
