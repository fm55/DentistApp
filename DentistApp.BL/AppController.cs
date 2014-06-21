using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentistApp.BL
{
    public class AppController
    {
        //UserDAL UserDAL { get; set; }
        public bool LogIn(string userName, string password)
        {
            //return UserDAL.LogIn(userName, password);
            return true;
        }

        public bool LogOff(string userName, string password)
        {
            //return UserDAL.LogOff(userName);
            return true;
        }
    }
}
