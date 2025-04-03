using System.Reflection;

namespace SCARS.ArchitectureRules;

/// <summary>
/// Checks whether a class marked with [Glue] violates glue-only responsibilities.
/// </summary>
public class NoLogicMethodsRule : IScarsRule
{
    public string Description => "Glue classes should not contain logic (methods with >20 bytes IL).";

    public bool AppliesTo(Type type) => true; // This will be filtered by the attribute

    public bool IsViolated(Type type)
    {
        return type
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.DeclaringType == type)
            .Any(m => m.GetMethodBody()?.GetILAsByteArray()?.Length > 20);
    }
}