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
        static bool runningChildQuestions;
        static int parentQuestionId;
        static List<List<int>> childQuestionsList; 

        static int terminator;                       // our question terminator value
        static DataTableHandler questionDth;         // question dth
        static DataTableHandler questionValuesDth;   // question values dth
        static DataRow activeQuestionRow;            //the active question being displayed



        public static int Terminator { get => terminator; set => terminator = value; }
        public static DataRow ActiveQuestionRow { get => activeQuestionRow; set => activeQuestionRow = value; }
        public static DataTableHandler QuestionValuesDth { get => questionValuesDth; set => questionValuesDth = value; }
        public static DataTableHandler QuestionDth { get => questionDth; set => questionDth = value; }
        public static bool RunningChildQuestions { get => runningChildQuestions; set => runningChildQuestions = value; }
        public static int ParentQuestionId { get => parentQuestionId; set => parentQuestionId = value; }
        public static List<List<int>> ChildQuestionsList { get => childQuestionsList; set => childQuestionsList = value; }
    }
}