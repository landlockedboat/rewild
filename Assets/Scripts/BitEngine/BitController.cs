using System;
using UnityEngine;

public class BitController<T> : BitSingletonController<T> where T : MonoBehaviour
{
}

public class BitSingletonController<T> : BitController where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => SingletonHelper.GetInstance(ref _instance);
}

public class BitController : BitMonoBehaviour, INotifiable
{
    private readonly NotificationManager _notificationManager = new NotificationManager();

    public void RegisterCallback(NotificationType notificationType, Action action)
    {
        _notificationManager.RegisterCallback(notificationType, action);
    }

    public void RegisterCallback(Action<NotificationType> action)
    {
        _notificationManager.RegisterCallback(action);
    }

    public void UnregisterCallback(NotificationType notificationType, Action action)
    {
        _notificationManager.UnregisterCallback(notificationType, action);
    }

    public void UnregisterCallback(Action<NotificationType> action)
    {
        _notificationManager.UnregisterCallback(action);
    }

    public void TriggerCallback(NotificationType notificationType)
    {
        _notificationManager.TriggerCallback(notificationType);
    }
}