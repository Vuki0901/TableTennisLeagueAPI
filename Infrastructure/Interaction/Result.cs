namespace Infrastructure.Interaction;

public class Result
{
    protected Result() {}
    
    public Result(Dictionary<string, string[]> errors) => Errors = errors;
    
    public Dictionary<string, string[]>? Errors { get; }
    public bool HasErrors => Errors is not null;
}