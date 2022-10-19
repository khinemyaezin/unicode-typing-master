using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace UnicodeTypingMaster
{
    class ApplicationGlobal
    {
        public static string systemConfigPath = @"SystemConfig.xml";
        public static string menuPath = @"Menu";

        public static string userName;
        public static string userLevel;
        public static string userLesson;
        public static string userLineNumber = "0";
        public static DataSet ds_systemConfig;
        public static List<string[]> lessons;
        public static List<string[]> menu;
        public static bool shiftKey = false;

        public static int finishTime = 1;

        public static string slow = "You are not fast enough";
        public static string average = "You are normal";
        public static string fluent = "You are little faster";
        public static string fast = "You are super fast";
        public static string pro = "Oh you are pro";


    }
}
