namespace Languages.Application.Models.Common;

public enum ResultErrorKind
{
    ValidationFailed,
    AuthenticationFailed,
    AuthorizationFailed,
    ResourceNotFound,
    ConflictOccured,
    ServerErrorOccured,
}