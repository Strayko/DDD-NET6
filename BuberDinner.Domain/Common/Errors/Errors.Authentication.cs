using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidEmail =>
            Error.Validation(code: "Auth.InvalidCred", description: "Invalid Email.");
        
        public static Error InvalidPassword =>
            Error.Validation(code: "Auth.InvalidCred", description: "Invalid Password.");
    }
}