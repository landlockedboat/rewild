public class DaysPassedMission : Mission
{
    public override void Start()
    {
        CurrentAmmount = TimeController.Instance.CurrentDay;
        TimeController.Instance.RegisterCallback(NotificationType.OnDayPassed, OnDayPassed);
        base.Start();
    }

    private void OnDayPassed()
    {
        ++CurrentAmmount;
    }
}