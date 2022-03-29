using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AITRSurvey
{
    public class User
    {
        String username;
        String password;


        public void setUsername(string username)
        {
          this.username = username;
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

        public string getUsername(string username)
        {
            return this.username;
        }

        public string getPassword(string password)
        {
            return this.password;
        }



    }

}