using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.HasActiveGame.ToString()) == 1)
            {
                NewGamePrompt.Instance.ShowPanel();
            }
            else
            {
                LevelPicker.Instance.ShowPanel();
            }
        });
    }
}

public enum PlayerPrefsKeys
{
    HasActiveGame
}