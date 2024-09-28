using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Configurations.Generic
{
    public static class ErrorMessages
    {
       public static class Generic
        {
            public const string ObjectNotFound = "Object not found";
            public const string InvalidRequest = "Invalid request";
            public const string UnexpectedError = "An unexpected error occurred";
            public const string InvalidPayload = "Invalid payload";
            public const string SomethingWentWrong = "Something went wrong";
            public const string BadRequest = "BadRequest";
        }

        public static class User
        {
            public const string UserNotFound = "User not found";
        }
    }
}
