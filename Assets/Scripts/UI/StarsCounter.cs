using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class StarsCounter : MonoBehaviour
{
    [SerializeField] private Image starSprite; 
    private TextMeshProUGUI starsCounterText;
    private int starsCounter;
    private SignalBus _signalBus;
    private Camera camera;
    private Vector3 starUiPos;

    public int StarsCount => starsCounter;
    public Vector3 StarUiPos => starUiPos;

    

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    private void Awake()
    {
        starsCounterText = GetComponent<TextMeshProUGUI>();
        _signalBus.Subscribe<StarChangeSignal>(OnStarChange);
        LoadStarResult();
        UpdateBestResult();
        starSprite = GetComponentInChildren<Image>();
        camera = FindObjectOfType<Camera>().GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<StarChangeSignal>(OnStarChange);
    }

    private void FixedUpdate()
    {
        
        starUiPos = camera.ScreenToWorldPoint(starSprite.transform.position);
    }

    private void UpdateBestResult()
    {
        starsCounterText.text = starsCounter.ToString();
    }

    private void OnStarChange()
    {
        starsCounter++;
        SaveStarResult();
        UpdateBestResult();
    }
    

   

    

    [System.Serializable]
    class SaveData
    {
        public int StarsPoints;
    }

    public void SaveStarResult()
    {
        SaveData data = new SaveData();
        data.StarsPoints = starsCounter;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile1.json", json);
    }

    public void LoadStarResult()
    {
        string path = Application.persistentDataPath + "/savefile1.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            starsCounter = data.StarsPoints;
        }
    }
}
