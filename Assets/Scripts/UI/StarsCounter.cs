using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using TMPro;
using System.IO;

public class StarsCounter : MonoBehaviour
{
    private TextMeshProUGUI starsCounterText;
    private int starsCounter;
    private SignalBus _signalBus;

    public int StarsCount => starsCounter;

    

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
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<StarChangeSignal>(OnStarChange);
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
