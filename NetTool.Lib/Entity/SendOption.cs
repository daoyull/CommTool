namespace NetTool.Lib.Entity;

public class SendOption
{
    public bool CanSend { get; set; } = true;
    
    
    public static SendOption Default => new SendOption();
}