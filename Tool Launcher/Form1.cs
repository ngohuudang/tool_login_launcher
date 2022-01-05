
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KAutoHelper;
using FoodAndPetFarms;
using System.Reflection;
using System.Runtime.InteropServices;
using ChangeInfo;
using static ChangeInfo.Form3;

namespace Tool_Launcher
{
    
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        string id = "";
        string password = "";
        string name = "";
        string dateTime = "";
        string test = "";
        public class ControllerSV
        {
            public int indexAccount { set; get; }

            public string account { set; get; }

            public string info { set; get; }

        }
        class IniFile   // revision 11
        {
            string Path;
            string EXE = Assembly.GetExecutingAssembly().GetName().Name;

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

            public IniFile(string IniPath = null)
            {
                Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
            }

            public string Read(string Key, string Section = null)
            {
                var RetVal = new StringBuilder(255);
                GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
                return RetVal.ToString();
            }

            public void Write(string Key, string Value, string Section = null)
            {
                WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
            }

            public void DeleteKey(string Key, string Section = null)
            {
                Write(Key, null, Section ?? EXE);
            }

            public void DeleteSection(string Section = null)
            {
                Write(null, null, Section ?? EXE);
            }

            public bool KeyExists(string Key, string Section = null)
            {
                return Read(Key, Section).Length > 0;
            }
        }
        public Form1()
        {
            InitializeComponent();
            load_account();
        }
        
        //System.Threading.ThreadStateException
        //Threading.Thread.CurrentThread.ApartmentState = Threading.ApartmentState.STA;
        private void DoOnUIThread(MethodInvoker d)
        {
            if (this.InvokeRequired) { this.Invoke(d); } else { d(); }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        public void funData(string str)
        {
            test=str;
            if (test != "")
            {
                test = "";
                Application.Restart();
            }    
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListView lsv = sender as ListView;
            //if(lsv.CheckedItems.Count>0)
            //{
            //    foreach(ListViewItem item in lsv.CheckedItems)
            //    {
            //        //MessageBox.Show(item.Text);
            //    }
                
            //}
        }
        public void send_tai_khoan(object sender, EventArgs e, IntPtr hWnd, string tai_khoan)
        {

            int x = 720;
            int y = 210;
            AutoControl.BringToFront(hWnd);
            var pointToClick = AutoControl.GetGlobalPoint(hWnd, x, y);
            EMouseKey mouseKey = EMouseKey.LEFT;
            AutoControl.MouseClick(pointToClick, mouseKey);
            DoOnUIThread(delegate ()
            {
                AutoControl.SendMultiKeysFocus(new KeyCode[] { KeyCode.CONTROL, KeyCode.KEY_A });
                AutoControl.SendStringFocus(tai_khoan);
            });
        }
        public void send_mat_khau(object sender, EventArgs e, IntPtr hWnd, string mat_khau)
        {
            //int x = 720;
            //int y = 260;
            //AutoControl.BringToFront(hWnd);
            //var pointToClick = AutoControl.GetGlobalPoint(hWnd, x, y);
            //EMouseKey mouseKey = EMouseKey.LEFT;
            //AutoControl.GetGlobalPoint(hWnd, x, y);
            //mouseKey = EMouseKey.LEFT;
            //AutoControl.MouseClick(pointToClick, mouseKey);
            AutoControl.SendKeyFocus(KeyCode.TAB);
            DoOnUIThread(delegate () {
                AutoControl.SendStringFocus(mat_khau);
            });
            
        }
        public void login(string id,string password, string name,object sender, EventArgs e)
        {
            for(int i=0;i<4;i++)
            {
                IntPtr hWnd = IntPtr.Zero;
                hWnd = AutoControl.FindWindowHandle(null, "Login");
                AutoControl.BringToFront(hWnd);
                EMouseKey mouseKey = EMouseKey.LEFT;
                //MessageBox.Show("lick toa do 720,210");
                var pointToClick = AutoControl.GetGlobalPoint(hWnd, 720, 210);
                //MessageBox.Show("click tai khoan");
                send_tai_khoan(sender, e, hWnd, id);
                Thread.Sleep(1000);
                //MessageBox.Show("click mk");
                send_mat_khau(sender, e, hWnd, password);
                Thread.Sleep(1000);
                //MessageBox.Show("send enter");
                AutoControl.SendKeyFocus(KeyCode.ENTER);
                Thread.Sleep(3000);
                //MessageBox.Show("chon sever");
                AutoControl.MouseClick(pointToClick, mouseKey);
                Thread.Sleep(2000);
                hWnd = IntPtr.Zero;
                hWnd = AutoControl.FindWindowHandle(null, "Gunny Launcher");
                AutoControl.SendText(hWnd,name=="" ?id:name );
                if (hWnd != IntPtr.Zero)
                {
                    //MessageBox.Show(hWnd.ToString());
                    break;
                }
            }
        }
        public void Remove_id(ref string[] lines, string[] temp)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                string[] ch = lines[i].Split(new char[] { '|' });
                if (temp[i] != ch[0])
                {
                    for (int j = i; j < lines.Length - 1; j++)
                    {
                        lines[j] = lines[j + 1];
                    }
                    lines[lines.Length - 1] = "";
                    return;
                }
            }
        }
        private void deadWithLineNull(string[] lines)
        {
            int n = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    continue;
                }
                n++;
            }
            if(n!=lines.Length)
            {
                string[] newLines = new string[n];
                n = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length == 0)
                    {
                        continue;
                    }
                    newLines[n++] = lines[i];
                }
                File.Delete("id.txt");
                StreamWriter writer;
                using (writer = new StreamWriter("id.txt", true))
                {
                    for (int i = 0; i < n; i++)
                    {
                        writer.WriteLine(newLines[i]);
                    }   
                }
                writer.Close();
            }
        }
        public void load_account ()
        {
            listView1.Items.Clear();
            string[] lines = File.ReadAllLines("id.txt");
            deadWithLineNull(lines);
            lines = File.ReadAllLines("id.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] ch = lines[i].Split(new char[] { '|' });
                id = ch[0];
                password = ch[1];
                if (ch.Length == 3)
                {
                    name = ch[2];
                }
                else
                {
                    name = "";
                }
                var MyIni = new IniFile("Settings.ini");
                dateTime = MyIni.Read(id);
                ListViewItem item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(id);
                if (checkBox1.Checked)
                    item.SubItems.Add(password);
                else
                    item.SubItems.Add("***********");
                item.SubItems.Add(name);
                item.SubItems.Add(dateTime);
                listView1.Items.Add(item);
            }
        }
        private void copyItem(int col)
        {
            //var builder = new StringBuilder();
            //foreach (ListViewItem item1 in listView1.SelectedItems)
            //    builder.AppendLine(item1.SubItems[col].Text);
            //Clipboard.SetText(builder.ToString());

            int index = 0;
            listView1.Invoke(new MethodInvoker(() =>
            {
                index = Int32.Parse(listView1.SelectedItems[0].Text);

            }));
            string[] lines = File.ReadAllLines("id.txt");
            string[] ch = lines[index - 1].Split(new char[] { '|' });
            if(col==3)
            {
                if (ch.Length == 3)
                {

                    Clipboard.SetText(ch[2]);
                }

            }
            else
                Clipboard.SetText(ch[col - 1]);
        }
        private void deleteAccount()
        {
            string[] lines = File.ReadAllLines("id.txt");
            //delete line selected
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            File.Delete("id.txt");
            StreamWriter writer;
            using (writer = new StreamWriter("id.txt", true))
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    int index = Int32.Parse(listView1.Items[i].Text);
                    writer.WriteLine(lines[index - 1]);
                }
                writer.Close();
                //string[] temp = File.ReadAllLines("id.txt");
                //Remove_id(ref lines, temp);
            }
            load_account();
        }
        private void loginAccount(object sender, EventArgs e)
        {
            int index = 0;
            DateTime now = DateTime.Now;
            Process.Start("client2.exe");
            Thread.Sleep(9000);
            Thread t11 = new Thread(() =>
            {
                //if (listView1.CheckedItems.) ;
                //for (int i = 0; i < ; i++)
                //System.Collections.IList list = listView1.CheckedItems;
                //for (int i = 0; i < 1 ; i++)
                {

                    listView1.Invoke(new MethodInvoker(() =>
                    {
                        index = Int32.Parse(listView1.SelectedItems[0].Text);

                    }));
                    string[] lines = File.ReadAllLines("id.txt");
                    string[] ch = lines[index - 1].Split(new char[] { '|' });
                    id = ch[0];
                    password = ch[1];
                    if (ch.Length == 3)
                    {
                        name = ch[2];
                    }
                    else
                    {
                        name = "";
                    }
                    listView1.Invoke(new MethodInvoker(() =>
                    {
                        listView1.Items[index - 1].BackColor = Color.LightGreen;
                        //listView1.Items[index].SubItems.Add(index2.ToString());
                        listView1.Refresh();
                    }));
                    login(id, password, name, sender, e);
                    var MyIni = new IniFile("Settings.ini");
                    MyIni.Write(id, now.ToString());
                }

            });
            t11.Start();
            
        }
        private void change(string info)
        {
            ControllerSV objSV = new ControllerSV();
            objSV.indexAccount = 0;
            objSV.info = info;
            objSV.account = "";
            string[] lines;
            listView1.Invoke(new MethodInvoker(() =>
            {
                objSV.indexAccount = Int32.Parse(listView1.SelectedItems[0].Text);
                lines = File.ReadAllLines("id.txt");
                objSV.account = lines[objSV.indexAccount - 1];
            }));
            Form3 form3 = new Form3(objSV);
            form3.Show();
            form3.BringToFront();


        }
 
        private void metroButton3_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw7898 = new StreamWriter("id.txt", true))
            {
                sw7898.WriteLine(textBox1.Text);
                sw7898.Close();
            }
            load_account();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            load_account();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            form2.BringToFront();

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyItem(1);     
        }

        private void passToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyItem(2);
        }

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyItem(3);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteAccount();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loginAccount(sender, e);
        }

        private void userToolStripMenuItem1_Click(object sender, EventArgs e)
        { 
            change("user");     
        }

        private void passToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            change("pass");
        }

        private void nameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            change("name");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var xpath=Path.GetDirectoryName(Application.ExecutablePath);
            Process.Start(xpath);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            load_account();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            load_account();
            if (textBox2.Text != "")
            {
                string[] lines = File.ReadAllLines("id.txt");

                foreach (ListViewItem item in listView1.Items)
                {
                    int index= Int32.Parse(item.Text)-1;
                    string[] str = lines[index].ToLower().Split(new char[] { '|' });

                    if (str[0].Contains(textBox2.Text.ToLower()))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        listView1.Items.Remove(item);
                    }

                }
                if (listView1.SelectedItems.Count == 1)
                {
                    listView1.Focus();
                }
            }
            else
            {
                Refresh();
            }
        }
    }
}
