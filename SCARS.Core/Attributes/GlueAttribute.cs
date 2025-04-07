using SCARS.ArchitectureRules;

namespace SCARS.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class GlueAttribute : Attribute, IScarsClassRuleAttribute
{
    public GlueAttribute(params Type[] ruleTypes)
    {
        RuleTypes = ruleTypes?.Length > 0 ? ruleTypes : new[] { typeof(NoLogicMethodsRule) };
    }

    public Type[] RuleTypes { get; }
}