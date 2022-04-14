using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;            // object / default data
using System.Web.UI.WebControls;

namespace AITRSurvey
{
    public static class StaffSearchHandler
    {
        static List<string> idList;
        static List<string> itemTypeList;
        static List<WebControl> controlList;
        static List<TextBox> controlTbList;
        static int terminator;
        static DataTable questionDt;
        static DataTableHandler questionValuesDth;

        public static List<string> IdList { get => idList; set => idList = value; }
        public static int Terminator { get => terminator; set => terminator = value; }
        public static DataTableHandler QuestionValuesDth { get => questionValuesDth; set => questionValuesDth = value; }
        public static DataTable QuestionDt { get => questionDt; set => questionDt = value; }
        public static List<string> ItemTypeList { get => itemTypeList; set => itemTypeList = value; }
        public static List<WebControl> ControlList { get => controlList; set => controlList = value; }
        public static List<TextBox> ControlTbList { get => controlTbList; set => controlTbList = value; }
    }
}