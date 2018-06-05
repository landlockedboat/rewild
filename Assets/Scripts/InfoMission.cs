public class InfoMission : Mission
{
    public override void Start()
    {
        // AutoComplete
        CurrentAmmount = ObjectiveAmmount;
        base.Start();
    }
}