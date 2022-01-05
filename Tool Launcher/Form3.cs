using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool_Launcher;
using static Tool_Launcher.Form1;

namespace ChangeInfo
{
    public partial class Form3 : MetroFramework.Forms.MetroForm
    {
        private ControllerSV objSV;

        public class infoAccount
        {

            public string account { set; get; }


        }
        public Form3(ControllerSV objSV)
        {
            InitializeComponent();
            this.objSV = objSV;
        }

        private void closeForm ()
        {
            Form3 settings = new Form3(objSV);
            this.Close();
            settings.Close();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        public delegate void delPassData(string str);

        private void btnSend_Click(object sender, System.EventArgs e)
        {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            //Thread t11 = new Thread(() =>
            //{
                Form1 frm = new Form1();
                delPassData del = new delPassData(frm.funData);
                int n = 0;
                string line = "";
                string[] ch = objSV.account.Split(new char[] { '|' });
                if (objSV.info == "user")
                {
                    line += textBox1.Text;
                    n++;
                    while (n < ch.Length)
                    {
                        line += "|" + ch[n];
                        n++;
                    }
                }
                else if (objSV.info == "pass")
                {
                    line = line + ch[0] + "|" + textBox1.Text;
                    n = 2;
                    while (n < ch.Length)
                    {
                        line += "|" + ch[n];
                        n++;
                    }
                }
                else if (objSV.info == "name")
                {
                    line = line + ch[0] + "|" + ch[1] + "|" + textBox1.Text;
                    n = 3;
                    while (n < ch.Length)
                    {
                        line += "|" + ch[n];
                        n++;
                    }
                }
                infoAccount newAccount = new infoAccount();
                newAccount.account = line;
                string[] lines = File.ReadAllLines("id.txt");
                File.Delete("id.txt");
                StreamWriter writer;
                using (writer = new StreamWriter("id.txt", true))
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i == objSV.indexAccount - 1)
                        {
                            writer.WriteLine(newAccount.account);
                        }

                        else
                        {
                            writer.WriteLine(lines[i]);
                        }
                    }
                }
                writer.Close();
                closeForm();
                del("done");
            //});
            //t11.Start();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            closeForm();
        }
    }
}
