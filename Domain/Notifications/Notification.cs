using Domain.Common;
using Domain.Users;

namespace Domain.Notifications;

public abstract class Notification : Entity
{
    public NotificationType Type { get; set; }
    
    public Player? Recipient { get; set; }
    public Player? Sender { get; set; }
}