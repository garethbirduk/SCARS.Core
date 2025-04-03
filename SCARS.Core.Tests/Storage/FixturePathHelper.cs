using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SCARS.Tests.Storage;

public static class FixturePathHelper
{
    private static string GetRealTestClassName()
    {
        var stack = new StackTrace();

        foreach (var frame in stack.GetFrames()!)
        {
            var method = frame.GetMethod();
            var declaringType = method?.DeclaringType;

            if (declaringType == null)
                continue;

            var typeName = declaringType.FullName ?? "";

            // ✅ Skip compiler-generated types
            if (typeName.Contains("System.Runtime.CompilerServices"))
                continue;

            if (typeName.Contains("<") || typeName.Contains(">"))
                continue;

            if (declaringType == typeof(FixturePathHelper))
                continue;

            return declaringType.Name;
        }

        throw new InvalidOperationException("Could not determine calling test class.");
    }

    public static string GetFixtureFilePath(string filename, string prefixPath = "",
        [CallerMemberName] string methodName = "")
    {
        if (string.IsNullOrWhiteSpace(prefixPath))
        {
            prefixPath = "Fixtures";
        }
        var testClassName = GetRealTestClassName();
        return Path.Combine(prefixPath, testClassName, methodName, filename);
    }
}