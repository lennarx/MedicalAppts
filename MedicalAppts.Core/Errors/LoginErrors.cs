namespace MedicalAppts.Core.Errors
{
    public static class LoginErrors
    {
        public static readonly Error UserNotFound = new Error(400, "No user was found for provided criteria");
        public static readonly Error EmailOrPasswordIncorrect = new Error(400, "User name or password incorrect");
        public static readonly Error UserBlocked = new Error(400, "Too many login attempts, the user is blocked");
    }
}
