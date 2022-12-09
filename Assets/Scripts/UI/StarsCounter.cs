using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using TMPro;
using System.IO;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StarsCounter : MonoBehaviour
{
    [SerializeField] private Image starSprite; 
    private TextMeshProUGUI _starsCounterText;
    private SignalBus _signalBus;
    private Camera _camera;
    private int _starsCounter;

    public Vector3 StarUiPos { get; private set; }


    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    private void Awake()
    {
        _starsCounterText = GetComponent<TextMeshProUGUI>();
        _signalBus.Subscribe<StarChangeSignal>(OnStarChange);
        LoadStarResult();
        UpdateBestResult();
        starSprite = GetComponentInChildren<Image>();
        _camera = FindObjectOfType<Camera>().GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<StarChangeSignal>(OnStarChange);
    }

    private void FixedUpdate()
    {
        
        StarUiPos = _camera.ScreenToWorldPoint(starSprite.transform.position);
    }

    private void UpdateBestResult()
    {
        _starsCounterText.text = _starsCounter.ToString();
    }

    private void OnStarChange()
    {
        _starsCounter++;
        SaveStarResult();
        UpdateBestResult();
    }
    
    [Serializable]
    class SaveData
    {
        [FormerlySerializedAs("StarsPoints")] public int starsPoints;
    }

    private void SaveStarResult()
    {
        var data = new SaveData();
        data.starsPoints = _starsCounter;

        var json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile1.json", json);
    }

    private void LoadStarResult()
    {
        var path = Application.persistentDataPath + "/savefile1.json";
        
        if (!File.Exists(path)) return;
        
        var json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);
        _starsCounter = data.starsPoints;
    }
}
