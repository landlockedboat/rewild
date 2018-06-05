using UnityEngine;

public class GameOverPanel : TextOverPanel<GameOverPanel>
{
    private void Awake()
    {
        TownController.Instance.RegisterCallback(NotificationType.OnGameOver, OnGameOver);
    }

    private void OnGameOver()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.HasActiveGame.ToString(), 0);
        StartTextOver();
    }
}