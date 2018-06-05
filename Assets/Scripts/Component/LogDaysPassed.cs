using UnityEngine;

public class LogDaysPassed : MonoBehaviour
{
    private void Start()
    {
        TimeController.Instance.RegisterCallback(NotificationType.OnDayPassed, OnDayPassed);
    }

    private void OnDayPassed()
    {
        Logger.Instance.LogInfo("day_passed_log");
    }
}