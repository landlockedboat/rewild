using System;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager
{
    private readonly Dictionary<NotificationType, Action> _notificationTable =
        new Dictionary<NotificationType, Action>();

    private Action<NotificationType> _generalActions;

    public void RegisterCallback(NotificationType notification, Action a)
    {
        if (_notificationTable.ContainsKey(notification))
        {
            _notificationTable[notification] += a;
        }
        else
        {
            _notificationTable.Add(notification, a);
        }
    }

    public void RegisterCallback(Action<NotificationType> action)
    {
        _generalActions += action;
    }

    public void UnregisterCallback(NotificationType notification, Action a)
    {
        if (_notificationTable.ContainsKey(notification))
        {
            _notificationTable[notification] -= a;
            if (_notificationTable[notification] == null)
            {
                _notificationTable.Remove(notification);
            }
        }
        else
        {
            Debug.LogWarning("No action for notification type " + notification + " to be unregistered");
        }
    }

    public void UnregisterCallback(Action<NotificationType> action)
    {
        _generalActions -= action;
    }

    public void TriggerCallback(NotificationType notification)
    {
        var generalListenEnabled = _generalActions != null;

        if (generalListenEnabled)
        {
            _generalActions(notification);
        }

        if (_notificationTable.ContainsKey(notification))
        {
            var action = _notificationTable[notification];

            if (action == null || action.GetInvocationList().Length == 0 && !generalListenEnabled)
            {
                WarnNoListeners(notification);
                return;
            }

            _notificationTable[notification]();
        }
        else if (!generalListenEnabled)
        {
            WarnNoListeners(notification);
        }
    }

    private void WarnNoListeners(NotificationType notificationType)
    {
        Debug.LogWarning("There are no listeners for action " + notificationType);
    }
}