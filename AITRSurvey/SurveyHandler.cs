using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;            // dataTable class
using System.Web.UI.WebControls;

namespace AITRSurvey
{
    public static class SurveyHandler
    {
        static int respondentId;                    //active respondent id
        static int parentQuestionId;                //curent parentQuestion Id
        static List<List<int>> childQuestionsList;  //list holding list of questions id's as int's 
        static string ipAddress;                    //user IpAddress

        static int terminator;                       // our question terminator value
        static DataTableHandler questionDth;         // question dth
        static DataTableHandler questionValuesDth;   // question values dth
        static DataRow activeQuestionRow;            //the active question being displayed



        public static int Terminator { get => terminator; set => terminator = value; }
        public static DataRow ActiveQuestionRow { get => activeQuestionRow; set => activeQuestionRow = value; }
        public static DataTableHandler QuestionValuesDth { get => questionValuesDth; set => questionValuesDth = value; }
        public static DataTableHandler QuestionDth { get => questionDth; set => questionDth = value; }
        public static int ParentQuestionId { get => parentQuestionId; set => parentQuestionId = value; }
        public static List<List<int>> ChildQuestionsList { get => childQuestionsList; set => childQuestionsList = value; }
        public static int RespondentId { get => respondentId; set => respondentId = value; }
        public static string IpAddress { get => ipAddress; set => ipAddress = value; }
    }
}