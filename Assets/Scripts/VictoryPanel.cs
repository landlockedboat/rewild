using System;
using UnityEngine;

public class VictoryPanel : TextOverPanel<VictoryPanel>
{
    private string _formUrlSpanish = "https://goo.gl/forms/cHjq8cMYNmiYiX093";
    private string _formUrlEnglish = "https://goo.gl/forms/yowbM3nT6UKl0BaA3";


    private void Start()
    {
        LevelConfiguration.Instance.RegisterCallback(NotificationType.OnVictoryAchieved, OnVictoryAchieved);
    }

    private void OnVictoryAchieved()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.HasActiveGame.ToString(), 0);
        StartTextOver();
    }

    protected override void FinishTextOver()
    {
        string formUrl;
        switch (GameApplication.Instance.CurrentLanguage)
        {
            case Language.English:
                formUrl = _formUrlEnglish;
                break;
            case Language.Spanish:
                formUrl = _formUrlSpanish;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Application.OpenURL(formUrl);
        base.FinishTextOver();
    }
}