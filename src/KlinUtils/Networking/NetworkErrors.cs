using KlinUtils.DiscriminatedUnion;

namespace KlinUtils.Networking;

/// <summary>
/// Contains series of possible network errors.
/// </summary>
public readonly ref struct NetworkErrors
{
    public static Error UrlRequired => new("UrlRequired", "Url is required");
    public static Error UrlNotFound => new("UrlNotFound", "Url not found");
    public static Error InvalidUrl => new("InvalidUrl", "Invalid url");
    public static Error PayloadNotAllowed => new("PayloadNotAllowed", "Payload is not allowed for GET and DELETE requests");
}