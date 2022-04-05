using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;            // dataTable class

namespace AITRSurvey
{
    public static class SurveyHandler
    {
        static bool runningChildQuestions;
        static int currentQuestionId;                //the parent we are looking at
        static int terminator;                       // our question terminator value
        static DataTableHandler questionDth;         // question dth
        static DataTableHandler questionValuesDth;   // question values dth
        static DataRow activeQuestionRow;            //the active question being displayed

        public static int CurrentQuestionId { get => currentQuestionId; set => currentQuestionId = value; }
        public static int Terminator { get => terminator; set => terminator = value; }
        public static DataRow ActiveQuestionRow { get => activeQuestionRow; set => activeQuestionRow = value; }
        public static DataTableHandler QuestionValuesDth { get => questionValuesDth; set => questionValuesDth = value; }
        public static DataTableHandler QuestionDth { get => questionDth; set => questionDth = value; }
        public static bool RunningChildQuestions { get => runningChildQuestions; set => runningChildQuestions = value; }
    }
}