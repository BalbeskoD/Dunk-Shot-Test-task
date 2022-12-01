using UnityEngine;
using UnityEngine.UI;

public class BackgroundToggler : MonoBehaviour
{
    [SerializeField] private Color blackBack;
    [SerializeField] private Color whiteBack;
    [SerializeField] private Color blackSettingsBack;
    [SerializeField] private Color whiteSettinsBack;
    private bool value;

    private Animator _animator;
    private Camera camera;
    private SettingsPanel settingsPanel;

    private static readonly int Value = Animator.StringToHash("Value");
    private static readonly string mainCameraTag = "Main Camera";

    private void Awake()
    {
        camera = GameObject.Find(mainCameraTag).GetComponent<Camera>();
        _animator = GetComponent<Animator>();
        _animator.SetBool(Value, value);
        settingsPanel = GetComponentInParent<SettingsPanel>();
    }
    private void OnEnable()
    {
        _animator.SetBool(Value, value);
    }

    public void Toggle()
    {
        this.value = !this.value;
        if(value == true)
        {
            camera.backgroundColor = blackBack;
            settingsPanel.gameObject.GetComponent<Image>().color = blackSettingsBack;
        }
        else
        {
            camera.backgroundColor = whiteBack;
            settingsPanel.gameObject.GetComponent<Image>().color = whiteSettinsBack;
        }
        _animator.SetBool(Value, value);
    }
}
