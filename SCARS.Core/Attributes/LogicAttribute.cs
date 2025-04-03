using SCARS.ArchitectureRules;

namespace SCARS.Attributes;

/// <summary>
/// Marks a class as Logic, which typically means static-only methods and no state or dependencies.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class LogicAttribute : Attribute, IScarsClassRuleAttribute
{
    public LogicAttribute(params Type[] ruleTypes)
    {
        RuleTypes = ruleTypes?.Length > 0
            ? ruleTypes
            : new[] { typeof(MustBeStaticRule) }; // Default rule for logic classes
    }

    public Type[] RuleTypes { get; }
}