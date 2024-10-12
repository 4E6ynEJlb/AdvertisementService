namespace Domain.Models.ApplicationModels
{
    public class ErrorsMessages
    {
        public const string FILE_DOES_NOT_EXIST = "File with this name does not exist";
        public const string CRITERION_NOT_IMPLEMENTED = "Chosen criterion does not implemented";
        public const string REGISTRATION = "Login and/or password are/is already taken";
        public const string INVALID_PAGE = "Chosen page does not exist or its number is invalid";
        public const string FORBIDDEN_ACTION = "You have not permission to do this";
        public const string INCORRECT_MARK = "Mark must be more/equals than 1 and less/equals than 5";
        public const string INCORRECT_CREDENTIALS = "Login and/or password are incorrect";
        public const string INCORRECT_FILE_FORMAT = "Sent file is not an image";
        public const string INCORRECT_ID = "Record with this id not found";
        public const string ID_IN_TOKEN_NOT_FOUND = "User id in token not found";
        public const string INVALID_ID_IN_TOKEN = "Invalid user id in token";
        public const string EMPTY_ARGUMENT = "Request argument is empty";
        public const string MISSING_CONTENT_TYPE = "Image content type is missing";
    }
}
