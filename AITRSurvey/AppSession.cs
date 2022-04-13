using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;            // object / default data

namespace AITRSurvey
{
    public static class AppSession
    {
        //Staff Staff   
        //List<int> idList

        const string STAFF = "staff";

        public static Staff getStaff()
        {
            return (Staff)HttpContext.Current.Session[STAFF];
        }

        public static void setStaff(Staff staff)
        {
            HttpContext.Current.Session[STAFF] = staff;
        }


        public static void clearStaff()
        {
            HttpContext.Current.Session[STAFF] = null;
        }




    }
}