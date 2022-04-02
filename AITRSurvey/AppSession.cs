using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AITRSurvey
{
    public static class AppSession
    {

        

        public static Staff getStaff()
        {
            return (Staff)HttpContext.Current.Session["Staff"];
        }

        public static void setStaff(Staff staff)
        {
            HttpContext.Current.Session["Staff"] = staff;
        }


        public static void clearStaff()
        {
            HttpContext.Current.Session["Staff"] = null;
        }





        public static void setRespondentId(int respondentID)
        {
            HttpContext.Current.Session["RespondentId"] = respondentID;
        }


        public static int getRespondentId()
        {
            return (int)HttpContext.Current.Session["RespondentId"];
        }

        public static void clearRespondentId()
        {
            HttpContext.Current.Session["RespondentId"] = null;
        } 






    }
}