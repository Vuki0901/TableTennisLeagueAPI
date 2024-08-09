using Domain.Common;
using Domain.Users;

namespace Domain.Notifications;

public abstract class Notification : Entity
{
    public string Type { get; init; } = null!;
    
    public string? RecipientEmailAddress { get; set; }
    public Player? Sender { get; set; }
}