namespace EyeDropper.Application.Dtos;

/// <summary>
/// Represents the result of an operation with an optional error message and exception.
/// </summary>
/// <param name="Error">The error message associated with the operation, if any.</param>
/// <param name="Exception">The exception that occurred during the operation, if any.</param>
public record RequestResultDto(string? Error = null, Exception? Exception = null)
{
    #region Properties

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// Returns true if there is no error message; otherwise, false.
    /// </summary>
    public bool IsSuccess => string.IsNullOrEmpty(Error);

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResultDto"/> class representing a successful result.
    /// </summary>
    public RequestResultDto() : this(null, null)
    {
    }
}