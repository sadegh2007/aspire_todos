namespace AspireTodo.Notifications.Shared;

public class NotificationBaseData<TData> where TData : class
{
    // public NotificationType Type { get; set; }
    public string Type => typeof(TData).Name;
    public TData? Data { get; set; }
}