using SCARS.ArchitectureRules;
using SCARS.Attributes;
using System.Reflection;

namespace SCARS;

public class Analyzer
{
    public IEnumerable<(Type Type, IScarsRule Rule)> GetScarsClassRuleAttributeViolations(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var customAttributes = type.GetCustomAttributes()
                .OfType<IScarsClassRuleAttribute>();

            foreach (var attr in customAttributes)
            {
                foreach (var ruleType in attr.RuleTypes)
                {
                    if (Activator.CreateInstance(ruleType) is not IScarsRule rule)
                        continue;

                    if (rule.AppliesTo(type) && rule.IsViolated(type))
                        yield return (type, rule);
                }
            }
        }
    }
}