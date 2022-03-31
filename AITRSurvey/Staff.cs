using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AITRSurvey
{
    public class Staff
    {
        private string SID;
        private string username;
        private string email;
        private string firstName;
        private string lastName;


        // constructor
        public Staff(string SID, string username, string email, string firstName, string lastName)
        {
            this.SID = SID;
            this.username = username;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
        }




        //getters and setters

        // just another way of getting access to vars
        //encapsilation field  - 4th gen language
        public string SID1 { get => SID; set => SID = value; }
        public string Username { get => username; set => username = value; }
        public string Email { get => email; set => email = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }



        //// 3rd gen lanuage such as c++ 
        //public void setSID(string SID)
        //{
        //    this.SID = SID;
        //}

        //public string getSID()
        //{
        //    return this.SID;
        //}



    }
}