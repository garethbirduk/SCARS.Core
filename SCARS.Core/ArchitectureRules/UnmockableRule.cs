using System.Reflection;

using SCARS.Attributes;

namespace SCARS.ArchitectureRules
{
    public class UnmockableRule
    {
        public static IEnumerable<(Type MockedType, Type TestClass)> FindViolations(Assembly testAssembly)
        {
            var testTypes = testAssembly.GetTypes()
                .Where(t => t.IsClass && t.GetMethods().Any(m => m.GetCustomAttributes().Any(a => a.GetType().Name.StartsWith("Fact") || a.GetType().Name.StartsWith("Theory"))));

            foreach (var type in testTypes)
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var method in methods)
                {
                    var instructions = method.GetMethodBody()?.GetILAsByteArray();
                    if (instructions == null) continue;

                    // Super crude: search for 'Mock`1' constructor call in strings
                    var referencedTypes = method.GetMethodBody()?.LocalVariables
                        .Select(v => v.LocalType)
                        .Where(t => t.Name.StartsWith("Mock"))
                        ?? Enumerable.Empty<Type>();

                    foreach (var mockType in referencedTypes)
                    {
                        var targetInterface = mockType.GenericTypeArguments.FirstOrDefault();
                        if (targetInterface is null) continue;

                        if (targetInterface.GetCustomAttribute<UnmockableAttribute>() is not null)
                        {
                            yield return (targetInterface, type);
                        }
                    }
                }
            }
        }
    }
}