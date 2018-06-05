using UnityEngine.SceneManagement;

public class LevelManager : BitGameManager<LevelManager>
{
    public void LoadLevel(int number)
    {
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene(number, LoadSceneMode.Additive);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}