namespace Base.Common;

// TODO Delete these if not needed
public static class ApiErrorDescription
{
    public const string EmailValidationErrorDescription = "An error occured during email validation. Please verify your email address.";
    public const string UsernameValidationErrorDescription = "An error occured during email validation. Please verify your email address.";
    public const string PasswordValidationErrorDescription = "An error occured during password validation. Please verify your password.";
    public const string MultipleProblemDescription = "An error occured during email or password validation.";
    public const string EmailAlreadyRegisteredDescription = "Email is already registered. Please use another email address.";
}

public enum ApiErrorSource
{
    EmailSource,
    UsernameSource,
    PasswordSource,
    MultipleSource
}

public static class ApiErrorType
{
    public const string BadRequestType = "Bad request";
    public const string NotFoundType = "Not found";
}