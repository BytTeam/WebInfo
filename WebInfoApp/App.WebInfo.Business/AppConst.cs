using System;
using System.Collections.Generic;
using System.Text;

namespace App.WebInfo.Business
{
    public static class AppConst
    {
        public static string UserSessionName="Session_Login_User";

        public static class UserRole
       {
           public static string General = "General";
           public static string Private = "Private";
           public static string Help = "Help";
           public static string Education = "Education";
           public static string Create = "Create";
           public static string Delete = "Delete";
           public static string Admin = "Admin";
       }
    }
}
