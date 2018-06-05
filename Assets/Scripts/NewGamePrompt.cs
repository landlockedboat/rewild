public class NewGamePrompt : UiPanel<NewGamePrompt> {
    public void Accept()
    {
        LevelPicker.Instance.ShowPanel();
        HidePanel();
    }
    
    public void Decline()
    {
        HidePanel();
    }
}
