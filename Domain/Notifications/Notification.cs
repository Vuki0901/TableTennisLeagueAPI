using Domain.Common;
using Domain.Users;

namespace Domain.Notifications;

public abstract class Notification : Entity
{
    public string Type { get; set; } = string.Empty;
    
    public Player? Recipient { get; set; }
    public Player? Sender { get; set; }
}