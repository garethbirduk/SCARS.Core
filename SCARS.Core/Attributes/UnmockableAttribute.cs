namespace SCARS.Attributes;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class UnmockableAttribute : Attribute
{
    public UnmockableAttribute(string reason = "This service should be replaced by a real or fake implementation.", string description = "")
    {
        Reason = reason;
        Description = description;
    }

    public string Description { get; }
    public string Reason { get; set; }
}