using System.Runtime.CompilerServices;

// Grants the test assemblies access to `internal` members of this assembly (e.g.
// ModificatorObserver.NotifyValueChange), so tests can exercise them directly instead of only
// indirectly through public call paths.
[assembly: InternalsVisibleTo("InsaneOne.Modifiers.Tests.EditMode")]
[assembly: InternalsVisibleTo("InsaneOne.Modifiers.Tests.PlayMode")]
