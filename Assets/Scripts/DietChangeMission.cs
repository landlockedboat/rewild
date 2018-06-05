public class DietChangeMission : UpdateLockedBuildingsMission
{
    public Diet NewDiet;
    
    public override void Start()
    {
        LevelConfiguration.Instance.CurrentDiet = NewDiet;
        base.Start();
    }
}