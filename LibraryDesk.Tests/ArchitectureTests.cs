using System.Reflection;
using LibraryDesk.Core.Domain;

namespace LibraryDesk.Tests;

public sealed class ArchitectureTests
{
    private const BindingFlags AllMembers = BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance
        | BindingFlags.Static
        | BindingFlags.DeclaredOnly;

    private static readonly string[] _forbiddenInDomain =
    {
        "LibraryDesk.Core.Storage",
        "LibraryDesk.Core.Services",
        "LibraryDesk.Core.Reports",
        "LibraryDesk.Core.Notifications",
        "LibraryDesk.Core.Pricing",
        "System.IO",
        "System.Text.Json",
        "Microsoft.Extensions.Logging",
    };

    [Fact]
    public void Domain_ProtectedFromOuterLayers_NoForbiddenTypesInSignatures()
    {
        Assembly core = typeof(Loan).Assembly;
        IEnumerable<Type> used = core.GetTypes()
            .Where(t => t.Namespace == typeof(Loan).Namespace)
            .SelectMany(t => t.GetFields(AllMembers).Select(f => f.FieldType)
                .Concat(t.GetProperties(AllMembers).Select(p => p.PropertyType))
                .Concat(t.GetMethods(AllMembers)
                    .SelectMany(m => m.GetParameters()
                        .Select(p => p.ParameterType)
                        .Append(m.ReturnType))));

        IEnumerable<string?> offenders = used
            .Where(t => t.Namespace is not null
                && _forbiddenInDomain.Any(n => t.Namespace.StartsWith(n, StringComparison.Ordinal)))
            .Select(t => t.FullName);

        Assert.Empty(offenders);
    }

    [Fact]
    public void Core_DoesNotReference_ConsoleApplication()
    {
        IEnumerable<string?> links = typeof(Loan).Assembly.GetReferencedAssemblies()
            .Select(a => a.Name)
            .Where(n => n is not null && n.StartsWith("LibraryDesk.App", StringComparison.Ordinal));

        Assert.Empty(links);
    }
}
