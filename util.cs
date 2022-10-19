using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;

namespace UnicodeTypingMaster
{
    static class Util
    {
        
        public static bool updateXml(string dsPath,string node,string[] data)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(dsPath);

                XmlNodeList nodeList = xmlDoc.SelectNodes(node);
                for (int i = 0; i < data.Length; i++)
                {
                    int l = i;
                    nodeList[0].ChildNodes[ i ].InnerText = data[i];
                }
                xmlDoc.Save(dsPath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool readXmlToDS()
        {
            try
            {
                DataSet ds_systemConfig = new DataSet();
                ds_systemConfig.ReadXml(ApplicationGlobal.systemConfigPath);
                ApplicationGlobal.ds_systemConfig = ds_systemConfig;

                ApplicationGlobal.userName = ApplicationGlobal.ds_systemConfig.Tables["user"].Rows[0]["name"].ToString();
                ApplicationGlobal.userLevel = ApplicationGlobal.ds_systemConfig.Tables["user"].Rows[0]["level"].ToString();
                ApplicationGlobal.userLesson = ApplicationGlobal.ds_systemConfig.Tables["user"].Rows[0]["slevel"].ToString();

                ApplicationGlobal.menu = readMenu(ApplicationGlobal.menuPath);
                return true;
            }
            catch (Exception e) { return false; }
        }      
        public static string messageWhenError(string key,string ekey)
        {

            string spfx = "";
            string ssfx = "";
            char[] prefix = { (char)4121,
                                (char)4158,
                                (char)4140,
                                (char)4152,
                                (char)4116,
                                (char)4145,
                                (char)4117,
                                (char)4139,
                                (char)4126,
                                (char)4106,
                                (char)4154 };
            char[] subfix = { (char)4096,
                (char)4141,
                (char)4143,
                (char)4116,
                (char)4141,
                (char)4117,
                (char)4154,
                (char)4117,
                (char)4139};

            foreach(char c in prefix)
            {
                spfx += c.ToString();
            }
            foreach (char c in subfix)
            {
                ssfx += c.ToString();
            }

            return spfx + " ' " + ekey + " ' "+ ssfx;
        }
        public static bool readFile(string lvl,string lesson)
        {
            try
            {
                ApplicationGlobal.lessons = new List<string[]>();
                string path = @"Data/lvl" + lvl + "/" + lesson + ".txt";
                if (File.Exists(path))
                {
                    ApplicationGlobal.lessons.Add(File.ReadAllLines(@"Data/lvl" + lvl + "/" + lesson + ".txt", Encoding.UTF8));
                    return true;
                }
                else
                {
                    return false;
                }

            }catch(Exception f)
            {
                return false;
            }
        }

        public static List<string[]> readMenu(string path)
        {
            List<string[]> menu = new List<string[]>();

            for(int i=1; i<=3; i++)
            {
                if (File.Exists(path+"/lvl"+i+".txt"))
                {
                    menu.Add(File.ReadAllLines(path + "/lvl" + i + ".txt", Encoding.UTF8));
                }
            }
            return menu;
        }

        public static void menuConfigReader(int lvlIndex, int lessIndex)
        {
            int lvl = (lvlIndex+1);
            int les = (lessIndex + 1);
            ApplicationGlobal.userLevel = ""+lvl.ToString();
            ApplicationGlobal.userLesson = ""+les.ToString();
            ApplicationGlobal.userLineNumber = "0";
        }
        public static float Truncate(this float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }
    }
}
