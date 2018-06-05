using UnityEngine;

public class TimeController : BitController<TimeController>
{
    private float _secondsInADay;
    private float _currentSecondsInADay;
    [HideInInspector] public int CurrentDay = 1;

    private void Start()
    {
        _secondsInADay = LevelConfiguration.Instance.SecondsInADay;
    }

    private void Update()
    {
        _currentSecondsInADay += Time.deltaTime;
        if (_currentSecondsInADay < _secondsInADay)
        {
            return;
        }
        
        _currentSecondsInADay = 0;
        ++CurrentDay;
        TriggerCallback(NotificationType.OnDayPassed);
    }
}