namespace SCARS.ArchitectureRules;

/// <summary>
/// Ensures the class is declared static (i.e., cannot be instantiated and contains only static members).
/// </summary>
public class MustBeStaticRule : IScarsRule
{
    public string Description => "Class must be static";

    public bool AppliesTo(Type type) => true; // This will be filtered by the attribute

    public bool IsViolated(Type type)
    {
        return !(type.IsAbstract && type.IsSealed); // C# static class = abstract + sealed
    }
}