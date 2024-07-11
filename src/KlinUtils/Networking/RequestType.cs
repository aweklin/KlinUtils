namespace KlinUtils.Networking;

/// <summary>
/// Network request type.
/// </summary>
public enum RequestType
{
    /// <summary>
    /// Indicates this is a GET request.
    /// </summary>
    Get,

    /// <summary>
    /// Indicates this is a POST request.
    /// </summary>
    Post,

    /// <summary>
    /// Indicates this is a PUT request.
    /// </summary>
    Put,

    /// <summary>
    /// Indicates this is a DELETE request.
    /// </summary>
    Delete,
}