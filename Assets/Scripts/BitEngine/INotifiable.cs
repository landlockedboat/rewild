using System;

public interface INotifiable
{
    void RegisterCallback(NotificationType notificationType, Action action);
    void RegisterCallback(Action<NotificationType> action);
    
    void UnregisterCallback(NotificationType notificationType, Action action);
    void UnregisterCallback(Action<NotificationType> action);
    
    void TriggerCallback(NotificationType notificationType);
}