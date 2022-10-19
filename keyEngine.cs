using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnicodeTypingMaster
{
    public static class keyEngine
    {
        public static int engMyan(int eng)
        {
            if (ApplicationGlobal.shiftKey == false)
            { 
                switch (eng)
                {
                    case 81:
                        return 4102;
                    case 87:
                        return 4112;
                    case 69:
                        return 4116;
                    case 82:
                        return 4121;
                    case 84:
                        return 4129;
                    case 89:
                        return 4117;
                    case 85:
                        return 4096;
                    case 73:
                        return 4100;
                    case 79:
                        return 4126;
                    case 80:
                        return 4101;
                    case 219:
                        return 4127;
                    case 221:
                        return 4137;
                    case 220:
                        return 4175;
                    //-------------------
                    case 65:
                        return 4145;
                    case 83:
                        return 4155;
                    case 68:
                        return 4141;
                    case 70:
                        return 4154;
                    case 71:
                        return 4139;
                    case 72:
                        return 4151;
                    case 74:
                        return 4156;
                    case 75:
                        return 4143;
                    case 76:
                        return 4144;
                    case 186:
                        return 4152;
                    //-----------------
                    case 90:
                        return 4118;
                    case 88:
                        return 4113;
                    case 67:
                        return 4097;
                    case 86:
                        return 4124;
                    case 66:
                        return 4120;
                    case 78:
                        return 4106;
                    case 77:
                        return 4140;
                    case 188:
                        return 4170;
                    case 190:
                        return 4171;
                    //enter
                    case 32:
                        return 32;
                    case 16:
                        return 16;
                    case 13:
                        return 13;

                    case 192: return 4176;
                    case 49: return 4161;
                    case 50: return 4162;
                    case 51: return 4163;
                    case 52: return 4164;
                    case 53: return 4165;
                    case 54: return 4166;
                    case 55: return 4167;
                    case 56: return 4168;
                    case 57: return 4169;
                    case 48: return 4160;
                }
                 return 00000;
            }
            else
            {  
                //------shift
                switch (eng)
                {
                    case 192: return 4110;
                    case 49: return 4109;
                    case 50: return 4178;
                    case 51: return 4107;
                    case 52: return 4179;
                    case 53: return 4180;
                    case 54: return 4181;
                    case 55: return 4123;
                
                    case 81:
                        return 4104;
                    case 87:
                        return 4125;
                    case 69:
                        return 4131;
                    
                    case 84:
                        return 4132;
                    case 89:
                        return 4172;
                    case 85:
                        return 4133;
                    case 73:
                        return 4173;
                    case 79:
                        return 4159;
                    case 80:
                        return 4111;
                    case 219:
                        return 4135;
                    case 221:
                        return 4138;
                    case 220:
                        return 4177;
                    //-------------------
                    case 65:
                        return 4119;
                    case 83:
                        return 4158;
                    case 68:
                        return 4142;
                    case 70:
                        return 4153;
                    case 71:
                        return 4157;
                    case 72:
                        return 4150;
                    case 74:
                        return 4146;
                    case 75:
                        return 4114;
                    case 76:
                        return 4115;
                    case 186:
                        return 4098;
                    //-----------------
                    case 90:
                        return 4103;
                    case 88:
                        return 4108;
                    case 67:
                        return 4099;
                    case 86:
                        return 4128;
                    case 66:
                        return 4122;
                    case 78:
                        return 4105;
                    case 77:
                        return 4134;
                    //enter
                    case 32:
                        return 32;
                    case 16:
                        return 16;
                    case 13:
                        return 13;
                }
                return 00000;
            }

            
        }

        public static string myanEng(int myan)
        {
            switch(myan)
            {
                case 4176: return "192";
                case 4161: return "49";
                case 4162: return "50";
                case 4163: return "51";
                case 4164: return "52";
                case 4165: return "53";
                case 4166: return "54";
                case 4167: return "55";
                case 4168: return "56";
                case 4169: return "57";
                case 4160: return "48";

                case 4102:
                    return "81";
                case 4112:
                    return "87";
                case 4116:
                    return "69";
                case 4121:
                    return "82";
                case 4129:
                    return "84";
                case 4117:
                    return "89";
                case 4096:
                    return "85";
                case 4100:
                    return "73";
                case 4126:
                    return "79";
                case 4101:
                    return "80";
                case 4127:
                    return "219";
                case 4137:
                    return "221";
                case 4175:
                    return "220";
                    //----------------------------
                case 4145:
                    return "65";
                case 4155:
                    return "83";
                case 4141:
                    return "68";
                case 4154:
                    return "70";
                case 4139:
                    return "71";
                case 4151:
                    return "72";
                case 4156:
                    return "74";
                case 4143:
                    return "75";
                case 4144:
                    return "76";
                case 4152:
                    return "186";
                    //-------------------------
                case 4118:
                    return "90";
                case 4113:
                    return "88";
                case 4097:
                    return "67";
                case 4124:
                    return "86";
                case 4120:
                    return "66";
                case 4106:
                    return "78";
                case 4140:
                    return "77";
                case 4170:
                    return "188";
                case 4171:
                    return "190";
                    //----------
                case 32:
                    return "32";
                case 16:
                    return "16";
                case 13:
                    return "13";
             

                //shift  
                case 4110: return "s192";
                case 4109: return "s49";
                case 4178: return "s50";
                case 4107: return "s51";
                case 4179: return "s52";
                case 4180: return "s53";
                case 4181: return "s54";
                case 4123: return "s55";
                

                case 4104:
                    return "s81";
                case 4125:
                    return "s87";
                case 4131:
                    return "s69";
                case 4132:
                    return "s84";
                case 4172:
                    return "s89";
                case 4133:
                    return "s85";
                case 4173:
                    return "s73";
                case 4159:
                    return "s79";
                case 4111:
                    return "s80";
                case 4135:
                    return "s219";
                case 4138:
                    return "s221";
                case 4177:
                    return "s220";
                //-------------------
                case 4119:
                    return "s65";
                case 4158:
                    return "s83";
                case 4142:
                    return "s68";
                case 4153:
                    return "s70";
                case 4157:
                    return "s71";
                case 4150:
                    return "s72";
                case 4146:
                    return "s74";
                case 4114:
                    return "s75";
                case 4115:
                    return "s76";
                case 4098:
                    return "s186";
                //-------------------------
                case 4103:
                    return "s90";
                case 4108:
                    return "s88";
                case 4099:
                    return "s67";
                case 4128:
                    return "s86";
                case 4122:
                    return "s66";
                case 4105:
                    return "s78";
                case 4134:
                    return "s77";
            

            }
            return "00000";
        }

        public static string[] wpm(int totalWord, float min,int error)
        {
            Console.WriteLine(totalWord);
            float temp_Gross = (totalWord/5) / min;
            float gross = Util.Truncate(temp_Gross, 1);
            float temp_Netgross = ((totalWord - error)/5) / min;
            float netGross = Util.Truncate(temp_Netgross, 1);

            float tmp = (float)(totalWord - error) / totalWord;
            float temp_accuracy;
            if (tmp == 0)
            {
                temp_accuracy = 0;
            }
            else
            {
                temp_accuracy = tmp * 100;
            }
            float accuracy = Util.Truncate(temp_accuracy, 1);

            
            string sgross = gross.ToString();
            string netWPM = ((int)netGross).ToString();

            string[] result = new string[5];
            if (netGross < 0) { netWPM = "N/A"; }
            result[0] = sgross;
            result[1] = netWPM;
            result[2] = error.ToString();
            result[3] = Convert.ToInt32(accuracy).ToString()+"%";
            result[4] = totalWord.ToString();
               // accuracy.ToString()+"%";

            return result;
        }

        public static int findCharacter(int keyCount, string choose)
        {
            try
            {
                if (choose == "me")
                {
                    string key = myanEng(keyCount);
                    if (key.StartsWith("s"))
                    {
                        ApplicationGlobal.shiftKey = true;
                        return Convert.ToInt32(key.Substring(1, key.Length - 1));
                    }
                    else
                    {
                        ApplicationGlobal.shiftKey = false;
                        return Convert.ToInt32(key);
                    }
                }
                else
                {
                    return engMyan(keyCount);
                }
            }
            catch (Exception e)
            {
                return 00000;

            }

        }

        public static bool isVowel(char c)
        {
            int vowel = (int)c;
            if (vowel == 4145 ||
                vowel == 4155 ||
                vowel == 4141 ||
                vowel == 4154 ||
                vowel == 4139 ||
                vowel == 4151 ||
                vowel == 4156 ||
                vowel == 4143 ||
                vowel == 4144 ||
                vowel == 4152 ||
                vowel == 4140 ||
                vowel == 4158 ||
                vowel == 4142 ||
                vowel == 4153 ||
                vowel == 4157 ||
                vowel == 4150 ||
                vowel == 4146)
            {
                return true;
            }
            else { return false; }

        }

        public static bool groupVowel(int group, int ch)
        {
            if (group == 3)
            {
                if (ch == 4152 ||
                    ch == 4143 ||
                    ch == 4140 ||
                    ch == 4151 ||
                    ch == 4146)
                {
                    return true;
                }
                else { return false; }

            }
            if (group == 4)
            {
                if (ch == 4152 ||
                    ch == 4154 ||
                    ch == 4151 ||
                    ch == 4140)
                {
                    return true;
                }
                else { return false; }
            }
            return false;


        }

        public static char[] readData(int lineNumber, string[] lessons)
        {
            try
            {
                string filtered = "";
                char[] data = lessons[lineNumber].ToCharArray();
                for (int i = 0; i < data.Length; i++)
                {
                    if ((int)data[i] != 8204)
                    {
                        if ((int)data[i] != 8203)
                        {
                            if (data[i] != 32 || keyEngine.isVowel(data[i]) == false)
                            {
                                // this.allVowels = false;                        
                            }
                            filtered += data[i].ToString();
                        }
                    }
                }
                return filtered.ToCharArray();
            }
            catch (Exception ex) { throw ex; }

        }

    }
}
