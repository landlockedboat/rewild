using System;
using UnityEngine;

public class LevelPicker : UiPanel<LevelPicker> {
    public void LoadLevel1()
    {
        PickLevel(1);
    }
    
    public void LoadLevel2()
    {
        PickLevel(2);
    }
    
    public void LoadLevel3()
    {
        throw new NotImplementedException();
    }
    
    public void LoadLevel4()
    {
        throw new NotImplementedException();
    }

    void PickLevel(int number)
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.HasActiveGame.ToString(), 0);
        LevelManager.Instance.LoadLevel(number);
    }
}
