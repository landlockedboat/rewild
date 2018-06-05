using System;
using System.Collections.Generic;
using UnityEngine;

public class GameApplication : BitGameManager<GameApplication>
{
    private Language _currentLanguage = Language.Spanish;

    public Language CurrentLanguage
    {
        get { return _currentLanguage; }
        set
        {
            _currentLanguage = value;
            TriggerCallback(NotificationType.OnLanguageChanged);
        }
    }

    private void Awake()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Catalan:
            case SystemLanguage.Spanish:
                CurrentLanguage = Language.Spanish;
                break;
            default:
                CurrentLanguage = Language.English;
                break;
        }
    }

    private int? _languagesCount;

    public int LanguagesCount
    {
        get
        {
            if (!_languagesCount.HasValue)
            {
                _languagesCount = Enum.GetValues(typeof(Language)).Length;
            }

            return _languagesCount.Value;
        }
    }

    [SerializeField] private List<float> _timeScales = new List<float> {1, 1.5f, 2, 2.5f};


    public void ChangeTimeSpeed(int speed)
    {
        if (speed >= 0 && speed < _timeScales.Count)
        {
            Time.timeScale = _timeScales[speed];
        }
    }

    public void QuitApplication()
    {
        print("Quitting application");
        Application.Quit();
    }

    public static bool IsUnityEditor()
    {
        var value = false;
#if UNITY_EDITOR
        value = true;
#endif
        return value;
    }
}