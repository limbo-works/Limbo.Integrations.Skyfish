#pragma warning disable CS1591

namespace Limbo.Integrations.Skyfish.Models.Media;

/// <summary>
/// Enum class representing the type of a SKyfish media.
/// </summary>
public enum SkyfishMediaType {

    Unrecognized = -1,

    Unspecified = 0,

    Image,

    Vector,

    Video,

    Generic

}