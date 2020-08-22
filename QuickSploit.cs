using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeAreDevs_API;

namespace CocoX
{
    public partial class CocoX : Form
    {
        public CocoX()
        {
            InitializeComponent();
        }

        WeAreDevs_API.ExploitAPI API = new WeAreDevs_API.ExploitAPI();

        public static void PopulateListBox(ListBox lsb, string Folder, string FileType)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            FileInfo[] Files = dinfo.GetFiles(FileType);
            foreach (FileInfo file in Files)
            {
                lsb.Items.Add(file.Name);
            }
        }

        WebClient wc = new WebClient();
        private string defPath = Application.StartupPath + "//Monaco//";

        private void AddIntel(string label, string kind, string detail, string insertText)
        {
            string text = "\"" + label + "\"";
            string text2 = "\"" + kind + "\"";
            string text3 = "\"" + detail + "\"";
            string text4 = "\"" + insertText + "\"";
            Monaco.Document.InvokeScript("AddIntellisense", new object[]
            {
                label,
                kind,
                detail,
                insertText
            });
        }

        private void AddGlobalF()
        {
            string[] array = File.ReadAllLines(this.defPath + "//globalf.txt");
            foreach (string text in array)
            {
                bool flag = text.Contains(':');
                if (flag)
                {
                    this.AddIntel(text, "Function", text, text.Substring(1));
                }
                else
                {
                    this.AddIntel(text, "Function", text, text);
                }
            }
        }

        private void AddGlobalV()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalv.txt"))
            {
                this.AddIntel(text, "Variable", text, text);
            }
        }

        private void AddGlobalNS()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalns.txt"))
            {
                this.AddIntel(text, "Class", text, text);
            }
        }

        private void AddMath()
        {
            foreach (string text in File.ReadLines(this.defPath + "//classfunc.txt"))
            {
                this.AddIntel(text, "Method", text, text);
            }
        }

        private void AddBase()
        {
            foreach (string text in File.ReadLines(this.defPath + "//base.txt"))
            {
                this.AddIntel(text, "Keyword", text, text);
            }
        }

        private async void CocoX_Load(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter w = File.AppendText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts")))
                {
                    w.WriteLine("# Anti-Banwave Code");
                    w.WriteLine("127.0.0.1 data.roblox.com");
                    w.WriteLine("127.0.0.1 roblox.sp.backtrace.io");
                }
            }
            catch
            {
                MessageBox.Show("We couldn't activate Anti-Banwave due to an unexpected error!\nBe careful!", "CocoX");
            }

            ScriptBox.Items.Clear();
            PopulateListBox(ScriptBox, "./Scripts", "*.txt");
            PopulateListBox(ScriptBox, "./Scripts", "*.lua");

            WebClient wc = new WebClient
            {
                Proxy = null
            };
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                bool flag2 = registryKey.GetValue(friendlyName) == null;
                if (flag2)
                {
                    registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                }
                registryKey = null;
                friendlyName = null;
            }
            catch (Exception)
            {
            }
            Monaco.Url = new Uri(string.Format("file:///{0}/Monaco/Monaco.html", Directory.GetCurrentDirectory()));
            await Task.Delay(500);
            Monaco.Document.InvokeScript("SetTheme", new string[]
            {
                   "Dark"
            });
            AddBase();
            AddMath();
            AddGlobalNS();
            AddGlobalV();
            AddGlobalF();
            Monaco.Document.InvokeScript("SetText", new object[]
            {
                 "-- CocoX --" +
                 " Thank you for chosing us"
            });
        }

        private void CocoX_Close(object sender, EventArgs e)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\\etc\\hosts");
                File.WriteAllLines(path, (from l in File.ReadLines(path) where l != "127.0.0.1 data.roblox.com" select l).ToList<string>());
                File.WriteAllLines(path, (from l in File.ReadLines(path) where l != "127.0.0.1 roblox.sp.backtrace.io" select l).ToList<string>());
                File.WriteAllLines(path, (from l in File.ReadLines(path) where l != "# Anti-Banwave Code" select l).ToList<string>());
            }
            catch
            {
                MessageBox.Show("We couldn't deactivate Anti-Banwave due to an unexpected error!\nRestart the program as an administrator!", "CocoX");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HtmlDocument document = Monaco.Document;
            string scriptName = "GetText";
            object[] args = new string[0];
            object obj = document.InvokeScript(scriptName, args);
            string script = obj.ToString();

            API.SendLuaScript(script);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            API.LaunchExploit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Monaco.Document.InvokeScript("SetText", new object[]
            {
                ""
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        var MainText = reader.ReadToEnd();
                        Monaco.Document.InvokeScript("SetText", new object[]
                        {
                            MainText
                        });
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HtmlDocument document = Monaco.Document;
            string scriptName = "GetText";
            object[] args = new string[0];
            object obj = document.InvokeScript(scriptName, args);
            string script = obj.ToString();

            try
            {
                var saveFileDialog1 = new SaveFileDialog
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Scripts",
                    Filter = string.Format("{0}Text files (*.txt)|*.txt|Lua files (*.lua)|*.lua", "*.lua"),
                    RestoreDirectory = true,
                    ShowHelp = false,
                    CheckFileExists = false
                };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) { File.WriteAllText(saveFileDialog1.FileName, script); }
            }
            catch
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        var MainText = reader.ReadToEnd();
                        API.SendLuaScript(MainText); // Execute given text from file.
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ScriptBox.Items.Clear();
            PopulateListBox(ScriptBox, "./Scripts", "*.txt");
            PopulateListBox(ScriptBox, "./Scripts", "*.lua");
        }

        private void ScriptBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ScriptBox.SelectedIndex != -1)
            {
                this.Monaco.Document.InvokeScript("SetText", new object[1]
                {
                      (object) System.IO.File.ReadAllText("Scripts\\" + this.ScriptBox.SelectedItem.ToString())
                });
            }
            else
            {
                int num = (int)MessageBox.Show("Please select a script from the list before trying to loading it in.", "CocoX");
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private bool _dragging = false;
        private Point _start_point = new Point(0, 0);

        private void TopBar_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void TopBar_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void TopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ExploitName_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
