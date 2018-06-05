using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] private float _timeToLoadLevel = 1f;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.HasActiveGame.ToString()) == 0)
        {
            _button.interactable = false;
        }
        
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            FadeOut.Instance.ShowPanel();
            Invoke(nameof(LoadLevel), _timeToLoadLevel);
        });
    }

    private void LoadLevel()
    {
        LevelManager.Instance.LoadLevel(1);
    }
}