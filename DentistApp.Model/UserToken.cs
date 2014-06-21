using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentistApp.Model
{
    public class UserToken
    {
        public static User CurrentUser { get; set; }
        public static bool LogUser(User user)
        {
            CurrentUser = user;
            return true;
        }

        public static bool LogUserOff()
        {
            CurrentUser = null;
            return true;
        }

    }
}
