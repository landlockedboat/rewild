using System;
using UnityEngine;

public class LegacyTimeController : BitController<LegacyTimeController>
{
    private TimeModel _timeModel;

    private void Awake()
    {
        _timeModel = TimeModel.Instance;
        _timeModel.CurrentTime = DateTime.Now;
        _timeModel.LastTimeEventLaunched = _timeModel.CurrentTime;
    }

    public void AddTime(TimeSpan timeSpan)
    {
        _timeModel.CurrentTime += timeSpan;

        var timeDiff = _timeModel.CurrentTime.Subtract(_timeModel.LastTimeEventLaunched);

        if (timeDiff.TotalSeconds < 1)
        {
            return;
        }
        
        TriggerCallback(NotificationType.OneSecondElapsed);
        _timeModel.LastTimeEventLaunched = _timeModel.CurrentTime;
    }

    private void Update()
    {
        AddTime(TimeSpan.FromSeconds(Time.deltaTime));
    }
}