using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Drawing2D;

namespace UnicodeTypingMaster
{
    public partial class frmTypingArea : Form
    {
        #region "variable"
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        private string currentParagraph;
        public int erCount = 0;
        public int erIndex = 99999;
        public bool erExit = false;
        public List<string> list_level = new List<string>();
        public bool timerStart = false;
        public int timerMM = 0;
        public int timerSS = 0;
        public char[] currentParagraphChar;
        public List<Button> selectButton;

        private int indexMain = 0;
        public bool finish = false;
        public int timerLogoInt = 1;
        public bool shiftKey = false;
        public bool nameDialog = false;

        //4145case
        public int specialIndex;
        public int groupEndIndex;
        public int groupStartIndex;
        public bool specialCharInclude;
        public bool specialCharOneTime = true;
        public bool specialCharOneTimePA = false;
        public int trueKey;
        public char trueChar;
        public string outPut = "";

        //level2
        public int characterCount = 0;
        public int errorCount = 0;
        //helper
        public int methodHelper ;

        public int running = 1;
        #endregion

        public frmTypingArea()
        {
            check();
            InitializeComponent();
            bindObject();
            
        }

        #region'initialize check'
        public void check()
        {
            if (Util.readXmlToDS())
            {
                if (ApplicationGlobal.userName != "" && ApplicationGlobal.userName != "" && ApplicationGlobal.userLesson != "")
                {
                    Util.readFile(ApplicationGlobal.userLevel, ApplicationGlobal.userLesson);
                    this.nameDialog = false;
                }
                else
                {
                    this.nameDialog = true;
                }
            }
        }

        public void bindObject()
        {
            myPanelLogo();
        }
        #endregion
        public void resetVariables()
        {
            this.indexMain = 0;
            this.outPut = "";
            this.timerSS = 0;
            this.timerMM = 0;
            this.lblTimer.Text = timerMM + " : " + timerSS;
            this.tblBtnGroupHeader.Visible = true;
            this.txtBoxSow.Text = "";
            this.textBoxUser.Text = "";
            this.lblErrorCount.Text = "0";
            this.timerStart = false;
            this.finish = false;
        }

        public void showTblReset()
        {
            this.finish = true;
            this.tblNoShift.Visible = false;
            this.lblTimer.Visible = false;
            this.panelShowText.Visible = false;
            this.tblLvl1Score.Visible = true;
        }

        public void prepareTxtBox()
        {
        Up:
            this.tblLvl1Score.Visible = false;
            this.tblShift.Visible = false;
            this.panelShowText.Visible = true;
            this.pannelKeyboard.Visible = true;
            this.tblNoShift.Visible = true;
            this.tblBtnGroupHeader.Visible = true;
            
            if (this.selectButton != null)
            {
                foreach (Button b in this.selectButton)
                {
                    b.BackColor = Color.White;
                }
            }
            showMessage("","", false);

            if(ApplicationGlobal.userLevel=="1")
            {
                this.erCount = 0;
                this.lblErrorCount.Text = "0";
                this.erIndex = 99999;
            }
            this.finish = false; 
            this.erExit = false;
            this.indexMain = 0;
            this.outPut = "";
            this.txtBoxSow.Text = "";
            this.textBoxUser.Text = "";       

            #region 'Paint Form'

            if (ApplicationGlobal.userLevel == "1")
            {
                this.pbMark1.Image = global::UnicodeTypingMaster.Properties.Resources.mark21;
                this.pbMark2.Image = null;
                this.pbMark3.Image = null;
            }
            if (ApplicationGlobal.userLevel == "2")
            {
                this.pbMark1.Image = null;
                this.pbMark2.Image = global::UnicodeTypingMaster.Properties.Resources.mark21;
                this.pbMark3.Image = null;
                /*   this.btnLvl1.BackColor = Color.FromArgb(210, 219, 225);
                   this.btnLvl1.ForeColor = Color.DimGray;
                   this.btnLvl2.BackColor = Color.White;
                   this.btnLvl2.ForeColor = Color.DimGray;
                   this.btnLvl3.BackColor = Color.FromArgb(210, 219, 225);
                   this.btnLvl3.ForeColor = Color.DimGray;*/
            }
            if (ApplicationGlobal.userLevel == "3")
            {
                this.pbMark1.Image = null;
                this.pbMark2.Image = null;
                this.pbMark3.Image = global::UnicodeTypingMaster.Properties.Resources.mark21;
                /* this.btnLvl1.BackColor = Color.FromArgb(210, 219, 225);
                 this.btnLvl1.ForeColor = Color.DimGray;
                 this.btnLvl2.BackColor = Color.FromArgb(210, 219, 225);
                 this.btnLvl2.ForeColor = Color.DimGray;
                 this.btnLvl3.BackColor = Color.White;
                 this.btnLvl3.ForeColor = Color.DimGray;*/
            }

            #endregion


            int less = Convert.ToInt32(ApplicationGlobal.userLesson);
            int lineNum = Convert.ToInt32(ApplicationGlobal.userLineNumber);
            this.textBoxTitle.Text = "Lesson " + less + "- " + (lineNum + 1);
            try
            {
                this.currentParagraphChar = keyEngine.readData(lineNum, ApplicationGlobal.lessons[0]);
            }catch(Exception er) { myPanelMain(); }

            if (this.currentParagraphChar.Length == 0)
            {
                MessageBox.Show("Empty", "Paragraph Blank");
                string s = ((Convert.ToInt32(ApplicationGlobal.userLesson)) - 1).ToString();
                ApplicationGlobal.userLevel = "1";
                ApplicationGlobal.userLesson = s;
                goto Up;

            }
            this.currentParagraph = new string(currentParagraphChar);
            this.txtBoxSow.Text = this.currentParagraph;

            showHintKey();
        }

        public void showHintKey()
        {
            int keyValue = 00000;
            if (this.indexMain < this.currentParagraphChar.Length)
            {
                if (this.currentParagraphChar[this.indexMain] == 32)
                {
                    keyValue = keyEngine.findCharacter((int)this.currentParagraphChar[this.indexMain], "me");
                    this.trueKey = keyValue;
                    goto Skip;
                }
                if (keyEngine.isVowel(this.currentParagraphChar[this.indexMain]) == false)
                {
                    groupIntoPhrase(this.indexMain + 1);
                }
                keyValue = keyValueOutPut();

                this.trueKey = keyValue;
            Skip:
                if (keyValue != 00000)
                {
                    this.selectButton = new List<Button>();
                    if (ApplicationGlobal.shiftKey == true)
                    {
                        highLightBtn(16);
                        highLightBtnShift(keyValue);
                        highLightBtnShift(16);
                    }
                    else
                    {
                        highLightBtn(keyValue);
                    }
                }
            }
            else
            {
                if(ApplicationGlobal.userLevel=="1") {
                    string[] result = keyEngine.wpm(this.textBoxUser.Text.Length, this.timerMM, this.erCount);
                    tblLvl1ScoreView("Your Score",result[3]);
                }
                if (ApplicationGlobal.userLevel == "2")
                {              
                    this.indexMain = 0;
                    this.outPut = "";
                    this.txtBoxSow.Text = "";
                    this.textBoxUser.Text = "";
                    this.finish = false;
                    nextLessons();
                }
            }
        }

        public void groupIntoPhrase(int index)
        {
            this.specialIndex = 99999;
            this.specialCharInclude = false;
            int count = 1;
            this.groupStartIndex = index - 1;
            for (int i = index; i < this.currentParagraphChar.Length; i++)
            {
                if (keyEngine.isVowel(currentParagraphChar[i]))
                {
                    if (this.currentParagraphChar[i] == 4145)
                    {
                        this.specialIndex = i;
                        this.specialCharInclude = true;
                        this.specialCharOneTime = true;
                    }
                    count++;

                }
                else
                {
                    this.groupEndIndex = i - 1;
                    return;
                }
            }
        }

        public int keyValueOutPut()
        {
            if (this.specialCharInclude == true && this.specialCharOneTime == true)
            {
                this.specialCharOneTime = false;
                this.specialCharOneTimePA = true;
                this.trueChar = (char)4145;
                return keyEngine.findCharacter(4145, "me");
            }
            else if (specialCharInclude == true && specialCharOneTime == false)
            {
                this.specialCharOneTimePA = false;
                if (this.indexMain <= this.specialIndex)
                {
                    this.trueChar = this.currentParagraphChar[this.indexMain - 1];
                    return keyEngine.findCharacter((int)this.currentParagraphChar[this.indexMain - 1], "me");
                }
                if (this.indexMain > this.specialIndex)
                {
                    this.trueChar = this.currentParagraphChar[this.indexMain];
                    return keyEngine.findCharacter((int)this.currentParagraphChar[this.indexMain], "me");
                }
            }
            this.trueChar = this.currentParagraphChar[this.indexMain];
            return keyEngine.findCharacter((int)this.currentParagraphChar[this.indexMain], "me");
        }

        private void frmTypingArea_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (this.panelMain.Visible == true)
            {
                if (e.KeyValue == 27)
                {
                    this.Close();
                }
                if (e.KeyCode == Keys.ShiftKey)
                {
                    this.tblNoShift.Visible = false;
                    this.tblShift.Visible = true;
                }
                if (this.finish == false)
                {
                    practiceArea(e);
                }
            }
        }

        public void practiceArea(KeyEventArgs e)
        {
            try
            {
                e.SuppressKeyPress = true;

                this.textBoxUser.Visible = true;
                this.txtBoxSow.Visible = true;
               
                this.lblTimer.Visible = true;
                this.timerStart = true;

                int keyValue = keyEngine.findCharacter(e.KeyValue, "em");
                if (keyValue != 0)
                {
                    if (e.KeyValue == this.trueKey)
                    {
                        if(this.running <=3)
                        {
                            if(this.running==1)
                            {
                                this.pbRunning.Image = global::UnicodeTypingMaster.Properties.Resources.a;
                                this.running++;
                                goto running;
                            }
                            if (this.running == 2)
                            {
                                this.pbRunning.Image = global::UnicodeTypingMaster.Properties.Resources.b;
                                this.running++;
                                goto running;
                            }
                            if (this.running == 3)
                            {
                                this.pbRunning.Image = global::UnicodeTypingMaster.Properties.Resources.c;
                                this.running = 1;
                                goto running;
                            }

                        }
                        
                        running:
                        this.characterCount++;
                     //   this.pbComputer.Image = global::UnicodeTypingMaster.Properties.Resources.pg04;
                        showMessage("","", false);
                        showTextToUser(keyValue);
                        foreach (Button b in this.selectButton)
                        {
                            b.BackColor = Color.White;
                        }
                        this.indexMain++;
                        showHintKey();
                    }
                    else
                    {
                        if (e.KeyValue != 16)
                        {
                        //    this.pbComputer.Image = global::UnicodeTypingMaster.Properties.Resources.pg04_1;
                            this.pbRunning.Image = global::UnicodeTypingMaster.Properties.Resources.e;

                            showMessage(this.trueChar.ToString(),((char)this.trueKey).ToString(), true);

                            if (this.erIndex == 99999 || this.erIndex != this.indexMain)
                            {
                                this.erIndex = this.indexMain;
                                this.erCount++;
                                this.lblErrorCount.Text = this.erCount.ToString();
                            }

                        }
                    }
                }
            }
            catch (Exception argse)
            {
                throw argse;
            }
        }

        public void showMessage(string msg,string ekey, bool open)
        {
            if(open==true)
            {
                this.pbMessage.Visible = true;
                this.txtBoxMessage.Visible = true;
                this.txtBoxMessage.Text = Util.messageWhenError(this.trueChar.ToString(),ekey);
            }
            else
            {
                this.pbMessage.Visible = false;
                this.txtBoxMessage.Visible = false;
            }
            
        }

        public void showTextToUser(int keyValue)
        {
            if (this.specialCharInclude == true)
            {
                if (this.specialCharOneTimePA == true)
                {
                    if (this.indexMain != 0)
                    {
                        char c = this.outPut.Substring(this.outPut.Length - 1).ToCharArray()[0];
                        //    if (keyboardUtil.isVowel(c) == true)
                        //    {
                        this.outPut += ((char)8204).ToString() + ((char)keyValue).ToString();
                        //    }
                    }
                    else
                    {
                        this.outPut += ((char)keyValue).ToString();
                    }

                    this.textBoxUser.Text = this.outPut;
                }
                else
                {
                    string s = "";
                    foreach (char p in this.outPut.ToCharArray())
                    {
                        if (p != (char)8204)
                        {
                            s += p.ToString();
                        }
                    }
                    this.outPut = s;
                    if (this.indexMain <= this.specialIndex)
                    {
                        this.outPut = replaceSpecialChar(this.groupStartIndex, this.groupEndIndex, this.outPut, keyValue);
                        this.outPut += ((char)4145).ToString();

                        this.textBoxUser.Text = this.outPut;
                    }
                    else if (this.indexMain > this.specialIndex)
                    {
                        this.outPut += ((char)keyValue).ToString();
                        this.textBoxUser.Text = this.outPut;
                    }
                }
            }
            else
            {
                this.outPut += ((char)keyValue).ToString();
                this.textBoxUser.Text = this.outPut;
            }

        }

        public string replaceSpecialChar(int startIndex, int endIndex, string sentence, int newKeyValue)
        {
            char[] sc = sentence.ToCharArray();
            string done = "";
            for (int i = startIndex; i < sentence.Length; i++)
            {
                if (sc[i] == (char)4145)
                {
                    sc[i] = (char)newKeyValue;
                    goto Skip;
                }
            }
        Skip:
            foreach (char c in sc)
            {
                done += c.ToString();
            }
            return done;
        }

        public void tblLvl1ScoreView(string wpm,string accuacy)
        {
            this.timerStart = false;
            this.panelShowText.Visible = false;
            this.pannelKeyboard.Visible = false;
            this.tblLvl1Score.Visible = true;
            this.tblBtnGroupHeader.Visible = false;
            this.finish = true;
            this.lblWPM.Text = wpm;
            this.lblAccuacy.Text = accuacy;
            this.erCount = 0;
            foreach (Button b in this.selectButton)
            {
                b.BackColor = Color.White;
            }
        }

        public void finishLessonLevel2(int time)
        {
            string[] wpm = keyEngine.wpm(this.characterCount, time, this.erCount);
            this.characterCount = 0;
            this.erCount = 0;
            resetVariables();
            myPanelScore(wpm);

        }

        public void highLightText(int index, int count, char selection, char doUndo)
        {
            if (doUndo == 'y')
            {
                //hight light word
                if (selection == 'f')
                {
                    this.txtBoxSow.Select(index, count);
                    this.txtBoxSow.SelectionColor = Color.Lime;
                }
                //high light Back
                if (selection == 'b')
                {
                    this.txtBoxSow.Select(index, count);
                    this.txtBoxSow.SelectionBackColor = Color.FromArgb(40, 133, 147);
                }
            }
            if (doUndo == 'n')
            {
                this.txtBoxSow.Select(index, count);
                this.txtBoxSow.SelectionBackColor = Color.FromArgb(31, 85, 112);
            }
            if (doUndo == 'f')
            {
                this.txtBoxSow.Select(index, count);
                //   this.txtBoxSow.SelectionColor = Color.Black;
            }
        } 

        public void highLightBtn(int k)
        {
            
            if (k == 192) { this.bLeft.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bLeft); }
            if (k == 49) { this.b1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b1); }
            if (k == 50) { this.b2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b2); }
            if (k == 51) { this.b3.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b3); }
            if (k == 52) { this.b4.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b4); }
            if (k == 53) { this.b5.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b5); }
            if (k == 54) { this.b6.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b6); }
            if (k == 55) { this.b7.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b7); }
            if (k == 56) { this.b8.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b8); }
            if (k == 57) { this.b9.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b9); }
            if (k == 48) { this.b0.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.b0); }
            if (k == 189) { this.bMinus.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bMinus); }
            if (k == 187) { this.bEqual.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bEqual); }
            if (k == 8) { this.bBackSpace.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bBackSpace); }
            if (k == 81) { this.bQ.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bQ); }
            if (k == 87) { this.bW.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bW); }
            if (k == 69) { this.bE.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bE); }
            if (k == 82) { this.bR.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bR); }
            if (k == 84) { this.bT.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bT); }
            if (k == 89) { this.bY.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bY); }
            if (k == 85) { this.bU.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bU); }
            if (k == 73) { this.bI.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bI); }
            if (k == 79) { this.bO.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bO); }
            if (k == 80) { this.bP.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bP); }
            if (k == 219) { this.bP1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bP1); }
            if (k == 221) { this.bP2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bP2); }
            if (k == 220) { this.bP3.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bP3); }
            if (k == 65) { this.bA.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bA); }
            if (k == 83) { this.bS.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bS); }
            if (k == 68) { this.bD.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bD); }
            if (k == 70) { this.bF.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bF); }
            if (k == 71) { this.bG.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bG); }
            if (k == 72) { this.bH.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bH); }
            if (k == 74) { this.bJ.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bJ); }
            if (k == 75) { this.bK.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bK); }
            if (k == 76) { this.bL.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bL); }
            if (k == 186) { this.bL1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bL1); }
            if (k == 222) { this.bL2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bL2); }
            if (k == 90) { this.bZ.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bZ); }
            if (k == 88) { this.bX.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bX); }
            if (k == 67) { this.bC.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bC); }
            if (k == 86) { this.bV.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bV); }
            if (k == 66) { this.bB.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bB); }
            if (k == 78) { this.bN.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bN); }
            if (k == 77) { this.bM.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bM); }
            if (k == 188) { this.bM1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bM1); }
            if (k == 190) { this.bM2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bM2); }
            if (k == 191) { this.bM3.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bM3); }
            if (k == 91) { this.bWindows.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bWindows); }
            if (k == 32) { this.bSpace.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bSpace); }
            if (k == 16) { this.bLeftShift.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bLeftShift); }
        }

        public void highLightBtnShift(int k)
        {
            if (k == 49) { this.bs1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs1); }
            if (k == 50) { this.bs2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs2); }
            if (k == 51) { this.bs3.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs3); }
            if (k == 52) { this.bs4.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs4); }
            if (k == 53) { this.bs5.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs5); }
            if (k == 54) { this.bs6.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs6); }
            if (k == 55) { this.bs7.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs7); }
            if (k == 56) { this.bs8.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs8); }
            if (k == 57) { this.bs9.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs9); }
            if (k == 48) { this.bs0.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bs0); }
            if (k == 189) { this.bsMinus.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsMinus); }
            if (k == 187) { this.bsEqual.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsEqual); }
            if (k == 8) { this.bsBackSpace.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsBackSpace); }

            if (k == 81) { this.bsQ.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsQ); }
            if (k == 87) { this.bsW.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsW); }
            if (k == 69) { this.bsE.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsE); }
            if (k == 82) { this.bsR.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsR); }
            if (k == 84) { this.bsT.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsT); }
            if (k == 89) { this.bsY.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsY); }
            if (k == 85) { this.bsU.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsU); }
            if (k == 73) { this.bsI.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsI); }
            if (k == 79) { this.bsO.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsO); }
            if (k == 80) { this.bsP.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsP); }
            if (k == 219) { this.bsP1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsP1); }
            if (k == 221) { this.bsP2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsP2); }
            if (k == 220) { this.bsP3.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsP3); }
            if (k == 65) { this.bsA.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsA); }
            if (k == 83) { this.bsS.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsS); }
            if (k == 68) { this.bsD.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsD); }
            if (k == 70) { this.bsF.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsF); }
            if (k == 71) { this.bsG.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsG); }
            if (k == 72) { this.bsH.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsH); }
            if (k == 74) { this.bsJ.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsJ); }
            if (k == 75) { this.bsK.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsK); }
            if (k == 76) { this.bsL.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsL); }
            if (k == 186) { this.bsL1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsL1); }
            if (k == 222) { this.bsL2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsL2); }
            if (k == 90) { this.bsZ.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsZ); }
            if (k == 88) { this.bsX.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsX); }
            if (k == 67) { this.bsC.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsC); }
            if (k == 86) { this.bsV.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsV); }
            if (k == 66) { this.bsB.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsB); }
            if (k == 78) { this.bsN.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsN); }
            if (k == 77) { this.bsM.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsM); }
            if (k == 188) { this.bsM1.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsM1); }
            if (k == 190) { this.bsM2.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsM2); }
            if (k == 191) { this.bsM3.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bsM3); }
            //        if (k == 91) { this.bWindows.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bWindows); }
            //  if (k == 32) { this.bSpace.BackColor = Color.DarkTurquoise; this.selectButton.Add(this.bSpace); }
            if (k == 16)
            {
                this.bsLeftShift.BackColor = Color.DarkTurquoise;
                this.selectButton.Add(this.bsLeftShift);
                //  this.bsRShift.BackColor = Color.DarkTurquoise;
                //  this.selectButton.Add(this.bsRShift);
            }



        }

        public void highLightButton(KeyEventArgs k)
        {
            string i = this.bA.Name;
            if (k.KeyValue == 192) this.bLeft.Select(); //this.bLeft.BackColor = Color.LightSteelBlue; }
            if (k.KeyValue == 49) this.b1.Select();// this.b1.BackColor = Color.LightSteelBlue;  }
            if (k.KeyValue == 50) this.b2.Select(); //this.b2.BackColor = Color.LightSteelBlue;  }
            if (k.KeyValue == 51) this.b3.Select();// this.b3.BackColor = Color.LightSteelBlue; }
            if (k.KeyValue == 52) this.b4.Select(); //BackColor= Color.LightSteelBlue; }
            if (k.KeyValue == 53) this.b5.Select();//BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 54) this.b6.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 55) this.b7.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 56) this.b8.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 57) this.b9.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 48) this.b0.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 189) this.bMinus.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 187) this.bEqual.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 8) this.bBackSpace.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.Q) this.bQ.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.W) this.bW.Select();// BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.E) this.bE.Select(); //BackColor = Color.LightSteelBlue;}
            if (k.KeyCode == Keys.R) this.bR.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.T) this.bT.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.Y) this.bY.Select();//BackColor = Color.LightSteelBlue;}
            if (k.KeyCode == Keys.U) this.bU.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.I) this.bI.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.O) this.bO.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.P) this.bP.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 219) this.bP1.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 221) this.bP2.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 220) this.bP3.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.A) this.bA.Select();// this.bA.BackColor= Color.Transparent;}
            if (k.KeyCode == Keys.S) this.bS.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.D) this.bD.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.F) this.bF.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.G) this.bG.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.H) this.bH.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.J) this.bJ.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.K) this.bK.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.L) this.bL.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 186) this.bL1.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 222) this.bL2.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.Z) this.bZ.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.X) this.bX.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.C) this.bC.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.V) this.bV.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.B) this.bB.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.N) this.bN.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyCode == Keys.M) this.bM.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 188) this.bM1.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 190) this.bM2.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 191) this.bM3.Select(); //BackColor= Color.LightSteelBlue;}
            if (k.KeyValue == 91) this.bWindows.Select(); //BackColor = Color.LightSteelBlue; }

            if (k.KeyCode == Keys.ShiftKey)
            {
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.LShiftKey)))
                {
                    this.bLeftShift.Select();

                }
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.RShiftKey)))
                {
                    this.bRightShift.Select();

                }
            }
            if (k.KeyCode == Keys.ControlKey)
            {
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.LControlKey)))
                {
                    this.bLeftCtrl.Select();
                }
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.RControlKey)))
                {
                    this.bRightCtrl.Select();
                }
            }
            if (k.KeyCode == Keys.Enter) this.bEnter.Select();
            if (k.KeyCode == Keys.Space) this.bSpace.Select();
            if (k.KeyValue == 8) this.bBackSpace.Select();
        }     

        #region'tblRESTART'
        private void btnRestart_Click(object sender, EventArgs e)
        {
            resetVariables();
            prepareTxtBox();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            nextLessons();
        }

        #endregion
        private void frmTypingArea_Load(object sender, EventArgs e)
        {

            this.lblName.Text = ApplicationGlobal.userName;
            this.txtBoxSow.HideSelection = true;
            this.lblTimer.Text = timerMM + " : " + timerSS;
            this.pbComputer.Visible = false;
            prepareMenu();
        }

        private void prepareMenu()
        {
            string[] lvl = { "Basic", "Intermediate", "Advence" };
            foreach (string s in lvl)
            {
                this.comboLevel.Items.Add(s);
            }
            this.comboLevel.SelectedIndex = 0;
            this.comboLesson.SelectedIndex = 0;
            this.comboBoxTime.Items.Add("Unlimited");
        }

        private void txtBoxSow_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void nextLessons()
        {
            try
            {
                if (ApplicationGlobal.userLevel == "1")
                {
                    this.timerMM = 0;
                    this.timerSS = 0;

                }
                this.outPut = "";
                foreach (Button b in this.selectButton)
                {
                    b.BackColor = Color.White;
                }
                int lvl = Convert.ToInt32(ApplicationGlobal.userLevel);
                int lessons = Convert.ToInt32(ApplicationGlobal.userLesson);
                int lineNum = Convert.ToInt32(ApplicationGlobal.userLineNumber);

                if ((lineNum + 1) <= (ApplicationGlobal.lessons[0].Length - 1))
                {
                    ApplicationGlobal.userLineNumber = (lineNum + 1).ToString();
                }
                else
                {
                    if ((lessons + 1) <= 10)
                    {
                        ApplicationGlobal.userLesson = (lessons + 1).ToString();
                        ApplicationGlobal.userLineNumber = "0";
                        Util.readFile(ApplicationGlobal.userLevel, ApplicationGlobal.userLesson);
                    }
                    else
                    {
                        if (lvl <= 3)
                        {
                            //level up
                            ApplicationGlobal.userLesson = "1";
                            ApplicationGlobal.userLineNumber = "0";
                            ApplicationGlobal.userLevel = (lvl + 1).ToString();
                            Util.readFile(ApplicationGlobal.userLevel, ApplicationGlobal.userLesson);
                        }
                        else
                        {
                            
                        }
                    }

                }
                prepareTxtBox();
            }catch(Exception l) {
                MessageBox.Show("Thank You", "Done");
                resetVariables();
                myPanelMenu();
            }
        }

        #region'button Header'
        private void BtnNextHeader_Click(object sender, EventArgs e)
        {
            //  reset();
            nextLessons();
        }

        private void BtnPrevHead_Click(object sender, EventArgs e)
        {
            this.timerMM = 0;
            this.timerSS = 0;
            this.lblErrorCount.Text = this.selectButton.Count.ToString();
            foreach (Button b in this.selectButton)
            {
                b.BackColor = Color.White;
            }
            this.indexMain = 0;

            int lvl = Convert.ToInt32(ApplicationGlobal.userLevel);
            int lessons = Convert.ToInt32(ApplicationGlobal.userLesson);
            int lineNum = Convert.ToInt32(ApplicationGlobal.userLineNumber);

            if ((lineNum) != 0)
            {
                ApplicationGlobal.userLineNumber = (lineNum - 1).ToString();
            }
            else
            {
                if ((lessons - 1) != 0)
                {
                    ApplicationGlobal.userLesson = (lessons - 1).ToString();
                    ApplicationGlobal.userLineNumber = "0";
                }
                else
                {

                }

            }
            prepareTxtBox();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tblLvl1ScoreView("Don't Give Up", "Try Again");

        }

        private void BtnMenu_Click(object sender, EventArgs e)
        {
            resetVariables();
            myPanelMenu();
        }

        private void btnEixt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region'Control Mouse Event in TextBox'

        private void TxtBoxSow_Enter(object sender, EventArgs e)
        {
            this.Controls[0].Focus();
        }

        private void TextBoxUser_Enter(object sender, EventArgs e)
        {
            this.Controls[0].Focus();
        }

        private void TxtBoxMessage_Enter(object sender, EventArgs e)
        {
            this.Controls[0].Focus();
        }
        #endregion

        private void FrmTypingArea_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (this.finish == false)
            {
                this.tblNoShift.Visible = true;
                this.tblShift.Visible = false;
            }
        }

        #region' paint'
        private void PanelMain_Paint(object sender, PaintEventArgs e)
        {
          /*   Control control = (Control)sender;
             Graphics g = e.Graphics;
             g.Clear(Color.White);
             Color endColor = Color.FromArgb(65, 118, 184);
             Color startColor = Color.FromArgb(48, 65, 86);
             using (LinearGradientBrush darkBrush = new LinearGradientBrush(control.ClientRectangle, startColor, endColor, LinearGradientMode.ForwardDiagonal))
             {
                 g.FillRectangle(darkBrush, control.ClientRectangle);
             }
               */
        }
        #endregion
        private void ComboLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region'timer'
        private void TimerLogo_Tick(object sender, EventArgs e)
        {
            this.timerLogoInt += 1;
            if (this.timerLogoInt == 3)
            {
                if (nameDialog == false)
                {
                    this.timerLogo.Enabled = false;
                    myPanelMenu();
                }
                else
                {
                    this.timerLogo.Enabled = false;
                    myPanelLogin();
                }
            }
        }

        private void timer_Tick_1(object sender, EventArgs e)
        {
            if (timerStart == true)
            {
                timerSS++;
                if (timerSS == 60)
                {
                    timerSS = 0;
                    timerMM++;
                }
                this.lblTimer.Text = timerMM + " : " + timerSS;
                if (ApplicationGlobal.finishTime != 0)
                {
                    
                    if (timerMM == ApplicationGlobal.finishTime)
                    {
                        this.timerStart = false;
                        finishLessonLevel2(timerMM);
                    }
                }
                
            }
        }
        #endregion
        private void ComboLevel_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (this.comboLesson.Items.Count != 0)
            {
                this.comboLesson.Items.Clear();
                this.comboBoxTime.Items.Clear();
            }
            if (this.comboLevel.SelectedIndex == 0)
            {
                this.comboBoxTime.Items.Add("Unlimited");
                foreach (string s in ApplicationGlobal.menu[0])
                {
                    this.comboLesson.Items.Add(s);
                }
            }
            if (this.comboLevel.SelectedIndex == 1)
            {
                for(int i=1; i<4; i++) { this.comboBoxTime.Items.Add(i + " minutes"); }
                foreach (string s in ApplicationGlobal.menu[1])
                {
                    this.comboLesson.Items.Add(s);
                }
            }
            if (this.comboLevel.SelectedIndex == 2)
            {
                for (int i = 1; i < 4; i++) { this.comboBoxTime.Items.Add(i + " minutes"); }
                foreach (string s in ApplicationGlobal.menu[2])
                {
                    this.comboLesson.Items.Add(s);
                }
            }
            this.comboLesson.SelectedIndex = 0;
            this.comboBoxTime.SelectedIndex = 0;
        }

        private void BtnMenuNext_Click(object sender, EventArgs e)
        {
            Util.menuConfigReader(this.comboLevel.SelectedIndex, this.comboLesson.SelectedIndex);
            Util.readFile(ApplicationGlobal.userLevel, ApplicationGlobal.userLesson);
            if (this.comboLevel.SelectedIndex == 0)
            {
                ApplicationGlobal.finishTime = 0;
            }
            else
            {
                ApplicationGlobal.finishTime = this.comboBoxTime.SelectedIndex + 1;
            }
            myPanelMain();
            prepareTxtBox();          
        }

        #region"panelControl"
        public void myPanelScore(string[] p)

        {
            this.lblWpmScore.Text = p[0];
            this.lblNetWpmScore.Text = p[1];
            this.lblErrorScore.Text = p[2];
            this.lblTotalWordScore.Text = p[4];
            
            string sac = "";
            int ac;
            try
            {
                sac = p[3];
                this.lblAccuracyScore.Text = p[3];
            }
            catch
            {
                sac = "0";
                this.lblAccuracyScore.Text = "0%";
            }
            if (!sac.Contains("%"))
            {
                ac = 0;
            }
            else
            {
                sac = sac.Remove(sac.IndexOf('%'), 1);
                ac = Convert.ToInt32(sac);
            }
            if (ac < 35)
            {
                this.pbox1.Image = global::UnicodeTypingMaster.Properties.Resources.star;
                this.pbox2.Image = global::UnicodeTypingMaster.Properties.Resources.strb;
                this.pbox3.Image = global::UnicodeTypingMaster.Properties.Resources.strb;
                this.lblStarCount.Text = "You got 1 Star";
            }
            if (ac<=80 && ac > 35)
            {
                this.pbox1.Image = global::UnicodeTypingMaster.Properties.Resources.star;
                this.pbox2.Image = global::UnicodeTypingMaster.Properties.Resources.star;
                this.pbox3.Image = global::UnicodeTypingMaster.Properties.Resources.strb;
                this.lblStarCount.Text = "You got 2 Stars";
            }
            if (ac<101 && ac > 80)
            {
                this.pbox1.Image = global::UnicodeTypingMaster.Properties.Resources.star;
                this.pbox2.Image = global::UnicodeTypingMaster.Properties.Resources.star;
                this.pbox3.Image = global::UnicodeTypingMaster.Properties.Resources.star;
                this.lblStarCount.Text = "You got 3 Stars";
            }
           
            int wpm = Convert.ToInt32(p[0]);
            //.Substring(0, 2));
        //    this.lblMsg.Text = wpm.ToString();
            if(wpm == 0)
            {
                this.tblResult.Controls.Add(this.btnResultPointer, 0, 0);
                this.lblScoreMessage.Text = ApplicationGlobal.slow;
            }
            if (wpm>0 && wpm <= 25)
            {
                this.tblResult.Controls.Add(this.btnResultPointer, 0, 0);
                this.lblScoreMessage.Text = ApplicationGlobal.slow;
            }
            if (wpm > 25 && wpm <= 45)
            {
                this.tblResult.Controls.Add(this.btnResultPointer, 1, 0);
                this.lblScoreMessage.Text = ApplicationGlobal.average;
            }
            if (wpm > 45 && wpm <= 60)
            {
                this.tblResult.Controls.Add(this.btnResultPointer, 2, 0);
                this.lblScoreMessage.Text = ApplicationGlobal.fluent;
            }
            if (wpm > 60 && wpm <= 80)
            {
                this.tblResult.Controls.Add(this.btnResultPointer, 3, 0);
                this.lblScoreMessage.Text = ApplicationGlobal.fast;
            }
            if (wpm > 80 && wpm < 90)
            {
                this.tblResult.Controls.Add(this.btnResultPointer, 4, 0);
                this.lblScoreMessage.Text = ApplicationGlobal.pro;
            }
           
            this.panelScore.Visible = true;
            this.panelScore.BringToFront();
            this.tblButtonMain.Visible = true;

            this.panelMain.Visible = false;
            this.panelLogo.Visible = false;
            this.panelLogin.Visible = false;
            this.panelMenu.Visible = false;
            this.tblBtnGroupHeader.Visible = false;

            this.indexMain = 0;
            this.erCount = 0;
            this.finish = true;
          

        }
        public void myPanelMain()
        {
            this.panelMain.Visible = true;
            this.panelMain.BringToFront();

            this.panelLogo.Visible = false;
            this.panelHelper.Visible = false;
            this.panelMenu.Visible = false;
            this.panelScore.Visible = false;
            this.tblLvl1Score.Visible = false;

            
            this.panelHeader.Visible = true;
            this.tblButtonMain.Visible = true;
            this.tblBtnGroupHeader.Visible = true;

            this.indexMain = 0;
            this.erCount = 0;
            this.finish = false;
        }
        public void myPanelMenu()
        {
            this.panelMenu.Visible = true;
            this.panelMenu.BringToFront();

            this.panelMain.Visible = false;
            this.panelHelper.Visible = false;
            this.panelLogo.Visible = false;
            this.panelScore.Visible = false;
            this.tblBtnGroupHeader.Visible = false;

            this.panelMenu.Visible = true;
            this.panelMenu.BringToFront();
            this.panelHeader.Visible = true;

        }
        public void myPanelLogin()
        {
            this.panelLogin.Visible = true;
            this.panelLogin.BringToFront();
            this.panelLogo.Visible = false;
            this.panelHelper.Visible = false;
            this.panelScore.Visible = false;
            this.panelMenu.Visible = false;
            this.panelHeader.Visible = false;

            
        }
        public void myPanelLogo()
        {
            this.panelLogo.Visible = true;
            this.panelLogo.BringToFront();
            this.panelHeader.Visible = false;
            this.panelScore.Visible = false;
            this.panelHelper.Visible = false;
            this.panelMenu.Visible = false;
            this.panelMain.Visible = false;
            this.panelLogin.Visible = false;

            
        }
        public void myPanelHelper()
        {     
            this.panelHelper.BringToFront();
            this.panelHelper.Visible = true;

            this.tblBtnGroupHeader.Visible = false;
            this.panelScore.Visible = false;
            this.panelMain.Visible = false;
            this.panelMenu.Visible = false;
            this.panelLogin.Visible = false;
            this.panelLogo.Visible = false;

            this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.ref01;
            this.methodHelper = 1;

            
            this.panelHeader.Visible = true;

            
          
       
        }
        #endregion

        private void BtnNextLogin_Click(object sender, EventArgs e)
        {
            try
            {

                string name = "";
                if (this.txtBoxName.Text.Length == 0 || this.txtBoxName.Text == null)
                {
                    this.lblHdwcy.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.panelLogin.Visible = false;
                    if (this.ckMale.CheckState == CheckState.Checked)
                    {
                        name = "Mr " + this.txtBoxName.Text;
                    }
                    else
                    {
                        name = "Miss " + this.txtBoxName.Text;
                    }

                    string[] data = new string[3];
                    string node = "root/user";
                    data[0] = name;
                    data[1] = "1";
                    data[2] = "1";

                    if (Util.updateXml(ApplicationGlobal.systemConfigPath, node, data) == true)
                    {
                        if (Util.readXmlToDS())
                        {
                            if (Util.readFile(ApplicationGlobal.userLevel, ApplicationGlobal.userLesson))
                            {
                                this.panelLogin.Visible = false;
                                this.panelMain.Visible = true;
                                this.panelHeader.Visible = true;
                                this.lblName.Text = ApplicationGlobal.userName;             
                                this.nameDialog = false;
                                myPanelMenu();
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("User Request Not complete", "User Error");
                    }


                }
            }
            catch (Exception es) { }
        }

        private void BtnNextHelper_Click(object sender, EventArgs e)
        {
            if (this.methodHelper == 1)
            {
                if(this.lblIndexHelper.Text=="1")
                {
                    showSheet(1, 2);
                    this.lblIndexHelper.Text = "2";
                    return;
                }
            }
            if(this.methodHelper == 2)
            {
                if (this.lblIndexHelper.Text == "1")
                {
                    showSheet(2, 2);
                    this.lblIndexHelper.Text = "2";
                    return;
                }
                if (this.lblIndexHelper.Text == "2")
                {
                    showSheet(2, 3);
                    this.lblIndexHelper.Text = "3";
                    return;
                }
                if (this.lblIndexHelper.Text == "3")
                {
                    showSheet(2, 4);
                    this.lblIndexHelper.Text = "4";
                    return;
                }
            }
            if (this.methodHelper == 3)
            {
                if (this.lblIndexHelper.Text == "1")
                {
                    showSheet(3, 2);
                    this.lblIndexHelper.Text = "2";
                    return;
                }
                if (this.lblIndexHelper.Text == "2")
                {
                    showSheet(3, 3);
                    this.lblIndexHelper.Text = "3";
                    return;
                }
                if (this.lblIndexHelper.Text == "3")
                {
                    showSheet(3, 4);
                    this.lblIndexHelper.Text = "4";
                    return;
                }
                if (this.lblIndexHelper.Text == "4")
                {
                    showSheet(3, 5);
                    this.lblIndexHelper.Text = "5";
                    return;
                }
                if (this.lblIndexHelper.Text == "5")
                {
                    showSheet(3, 6);
                    this.lblIndexHelper.Text = "6";
                    return;
                }
                if (this.lblIndexHelper.Text == "6")
                {
                    showSheet(3, 7);
                    this.lblIndexHelper.Text = "7";
                    return;
                }
            }
        }

        private void BtnBackHelper_Click(object sender, EventArgs e)
        {
           
        }
        private void showSheet(int category,int subCategory)
        {

            if(category==2)
            {
                if (subCategory == 1) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.ref1; }
                if (subCategory == 2) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.ref2; }
                if (subCategory == 3) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.ref3; }
                if (subCategory == 4) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.ref4; }
            }
            if(category==1)
            {
                if (subCategory == 1) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.ref01; }
                if (subCategory == 2) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.ref02; }         
            }
            if(category==3)
            {
                if (subCategory == 1) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.uni1; }
                if (subCategory == 2) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.uni2; }
                if (subCategory == 3) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.uni3; }
                if (subCategory == 4) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.uni4; }
                if (subCategory == 5) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.uni5; }
                if (subCategory == 6) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.uni6; }
                if (subCategory == 7) { this.pboxHelper.Image = global::UnicodeTypingMaster.Properties.Resources.uni7; }
                
            }
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            myPanelHelper();
        }

        private void PanelShowText_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Color startColor= Color.LightBlue;
            Color endColor = Color.White;
            using (LinearGradientBrush darkBrush = new LinearGradientBrush(control.ClientRectangle, startColor, endColor, LinearGradientMode.Vertical))
            {
                g.FillRectangle(darkBrush, control.ClientRectangle);
            }
        }

        private void PanelHelper_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Color startColor = Color.LightBlue;
            Color endColor = Color.White;
            using (LinearGradientBrush darkBrush = new LinearGradientBrush(control.ClientRectangle, startColor, endColor, LinearGradientMode.Vertical))
            {
                g.FillRectangle(darkBrush, control.ClientRectangle);
            }
        }

        private void LblMethod1_Click(object sender, EventArgs e)
        {
            this.lblMethod1.BackColor = Color.White;
            this.lblMethod2.BackColor = Color.Transparent;
            this.lblAboutUnicode.BackColor = Color.Transparent;
            this.methodHelper = 1;
            showSheet(1,1);
            this.lblIndexHelper.Text = "1";
        }

        private void LblMethod2_Click(object sender, EventArgs e)
        {
            this.lblMethod1.BackColor = Color.Transparent;
            this.lblMethod2.BackColor = Color.White;
            this.lblAboutUnicode.BackColor = Color.Transparent;
            this.methodHelper = 2;
            showSheet(2, 1);
            this.lblIndexHelper.Text = "1";
        }

        private void LblAboutUnicode_Click(object sender, EventArgs e)
        {
            this.lblMethod1.BackColor = Color.Transparent;
            this.lblMethod2.BackColor = Color.Transparent;
            this.lblAboutUnicode.BackColor = Color.White;
            showSheet(3, 1);
            this.methodHelper = 3;
            this.lblIndexHelper.Text = "1";
        }

        private void PanelMenu_Paint(object sender, PaintEventArgs e)
        {
          /*  Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Color startColor = Color.LightBlue;
            Color endColor = Color.FromArgb(201, 246, 255);
            using (LinearGradientBrush darkBrush = new LinearGradientBrush(control.ClientRectangle, startColor, endColor, LinearGradientMode.Vertical))
            {
                g.FillRectangle(darkBrush, control.ClientRectangle);
            }*/
        }

        private void BtnNameCencel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox4_Click(object sender, EventArgs e)
        {
            frmManager manager = new frmManager();
            manager.ShowDialog();
        }

        private void FrmTypingArea_Activated(object sender, EventArgs e)
        {
            this.tblNoShift.Visible = true;
            this.tblShift.Visible = false;
        }
    }
}