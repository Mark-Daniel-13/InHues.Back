namespace InHues.Domain.Helpers
{
    public static class ApiRoutes
    {
        public const string Root = "/api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Custom {
        }
        public static class Identity {
            public const string Login = Base + "/login";
            public const string Register = Base + "/register";
            public const string Refresh = Base + "/refresh";
            public const string Update = Base + "/update";
            public const string InitializeDb = Base + "/initDb/{key}";
            public const string DeleteUser = Base + "/delete/{key}";
            public const string FetchRoles = Base + "/fetchRoles";
            public const string AddRoleToUser = Base + "/addRoleToUser";
            public const string GetUserRole = Base + "/getUserRole";
            public const string GetUsers = Base + "/getUsers";
            public const string GetUser = Base + "/getUser";
            public const string ValidateUsername = Base + "/checkUserName";
            public const string ResetPassword = Base + "/resetPassword";
            public const string ResetPasswordRequest = Base + "/resetPasswordRequest";
        }
        public static class Users {
            private const string ControllerName = "/users";
            private const string ParameterBase = "/{userId}";

            public const string GetAll = Base + ControllerName;                     //  api/v1/posts                || GET
            public const string Get = Base + ControllerName + ParameterBase;        //  api/v1/posts/{postId}       || GET
            public const string Update = Base + ControllerName;                     //  api/v1/posts/{postId}       || PATCH
            public const string Delete = Base + ControllerName + ParameterBase;     //  api/v1/posts/{postId}       || DELETE
            public const string Create = Base + ControllerName;                     //  api/v1/posts                || POST
        }
    }

}
