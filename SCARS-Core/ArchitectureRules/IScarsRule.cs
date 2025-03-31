namespace SCARS.ArchitectureRules;

/// <summary>
/// Defines a rule that can be applied to a class to validate architectural principles (e.g., SCARS).
/// </summary>
public interface IScarsRule
{
    /// <summary>
    /// A human-readable description of what the rule checks.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Determines whether this rule applies to the given type.
    /// </summary>
    bool AppliesTo(Type type);

    /// <summary>
    /// Determines whether the given type violates this rule.
    /// </summary>
    bool IsViolated(Type type);
}
