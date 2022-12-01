using UnityEngine;
using Zenject;
using Zenject.Signals;
using TMPro;
using System.IO;

public class PointCounter : MonoBehaviour
{
    private TextMeshProUGUI pointCounterText;
    private int pointCounter;
    private int bestResult;
    private SignalBus _signalBus;

    public int PointCount => pointCounter;

    

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    private void Awake()
    {
        pointCounterText = GetComponent<TextMeshProUGUI>();
        _signalBus.Subscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Subscribe<GoalSignal>(OnGoal);
        _signalBus.Subscribe<GameRestartSignal>(OnRestart);
        _signalBus.Subscribe<FinishSignal>(OnFinish);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<ClearGoalSignal>(OnClearGoal);
        _signalBus.Unsubscribe<GoalSignal>(OnGoal);
        _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
        _signalBus.Unsubscribe<FinishSignal>(OnFinish);
    }

    private void OnClearGoal(ClearGoalSignal signal)
    {
        if (pointCounter == 0)
        {
            _signalBus.Fire<GameStartSignal>();
        }
        pointCounter += (signal.clearInRow + 1);
        PointCountUpdate();
    }

    private void OnGoal()
    {
        if (pointCounter == 0)
        {
            _signalBus.Fire<GameStartSignal>();
        }
        pointCounter++;
        PointCountUpdate();
    }

    private void OnRestart()
    {
        pointCounter = 0;
        PointCountUpdate();
    }

    private void PointCountUpdate()
    {
        pointCounterText.text = pointCounter.ToString();
    }

    private void OnFinish()
    {
        LoadBestResult();
        if (bestResult < pointCounter)
        {
            bestResult = pointCounter;
            SaveBestResult();
        }
        _signalBus.Fire(new BestResultSignal{_bestResult = bestResult});
    }

    [System.Serializable]
    class SaveData
    {
        public int BestPoints;
    }

    public void SaveBestResult()
    {
        SaveData data = new SaveData();
        data.BestPoints = bestResult;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile2.json", json);
    }

    public void LoadBestResult()
    {
        string path = Application.persistentDataPath + "/savefile2.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestResult = data.BestPoints;
        }
    }
}

