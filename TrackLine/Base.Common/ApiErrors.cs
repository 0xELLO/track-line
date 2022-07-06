namespace Base.Common;

public static class ApiErrorDescription
{
    public const string EmailValidationErrorDescription = "Email not valid";
    public const string PasswordValidationErrorDescription = "Password not valid";
    public const string EmailOrPasswordProblemDescription = "Email or password problem";
    public const string EmailAlreadyRegisteredDescription = "Email already registered";
}

public static class ApiErrorSource
{
    public const string EmailSource = "Email";
    public const string PasswordSource = "Password";
    public const string EmailOrPasswordSource = "Email/Password";
}

public static class ApiErrorTitle
{
    public const string ValidationErrorTitle = "Validation error";
    public const string EmailValidationErrorTitle = "Email validation error";
    public const string PasswordValidationErrorTitle = "Password validation error";
    public const string MultipleValidationErrorTitle = "Multiple validation errors";
    public const string AppUserValidationErrorTitle = "AppUser validation error";
}

public static class ApiErrorType
{
    public const string BadRequestType = "Bad request";
    public const string NotFoundType = "Not found";
}