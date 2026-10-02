namespace Languages.Application.Common;

public enum ResultErrorKind
{
    ValidationFailed,
    AuthenticationFailed,
    AuthorizationFailed,
    ResourceNotFound,
    ConflictOccured,
}