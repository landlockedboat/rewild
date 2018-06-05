using System;
using UnityEngine;

public class BitGameManager<T> : BitMonoBehaviour, INotifiable where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            var instances = (T[])FindObjectsOfType(typeof(T));

            if (instances.Length > 1)
            {
                throw new Exception("More than one GameManager of type" +
                                           typeof(T) + " in scene.");
            }

            if (instances.Length == 0)
            {
                var gameManagers = Instantiate(Resources.Load<GameObject>("GameManagers"), Vector3.zero, Quaternion.identity);
                return gameManagers.GetComponentInChildren<T>();
            }

            _instance = instances[0];
			
            return _instance;
        }
    }

    // NOTIFICATION MANAGER STUFF
    
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