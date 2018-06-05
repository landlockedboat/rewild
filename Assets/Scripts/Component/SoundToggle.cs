using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    private Toggle _toggle;

    private void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(AudioManager.Instance.ToggleSound);
    }
}