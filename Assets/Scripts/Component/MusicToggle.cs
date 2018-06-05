using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    private Toggle _toggle;

    private void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(AudioManager.Instance.ToggleMusic);
    }
}