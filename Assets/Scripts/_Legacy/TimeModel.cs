using System;

public class TimeModel : Singleton<TimeModel>
{
    public DateTime CurrentTime;
    public DateTime LastTimeEventLaunched;
    //[HideInInspector]
    //public bool IsTimeRunning = true;
}