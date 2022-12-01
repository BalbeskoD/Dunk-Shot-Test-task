using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Zenject.Signals;

public class MenuPanel : MonoBehaviour, IPointerDownHandler
{
    private SignalBus _signalBus;


    [Inject]public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        _signalBus.Fire<GameStartSignal>();
    }
}
