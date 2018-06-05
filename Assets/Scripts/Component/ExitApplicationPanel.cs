using UnityEngine;

public class ExitApplicationPanel : UiPanel<ExitApplicationPanel>
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPanel();
        }
    }

    public void QuitGame()
    {
        GameApplication.Instance.QuitApplication();
    }
    
    public void GoToMainMenu()
    {
        LoadSaveGame.SaveGameData();
        PlayerPrefs.SetInt(PlayerPrefsKeys.HasActiveGame.ToString(), 1);
        LevelManager.Instance.RestartGame();
    }
}