using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace PolygraphProj
{
    public partial class Form1 : Form
    {
        private string server = "62.217.176.74";
        private string username = "admin";
        private string password = "ROvO!wJ6";
        private string fileName = "";

        struct FtpSetting 
        {
            public string Server { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FileName { get; set; }
            public string FullName { get; set; }
        }

        FtpSetting _inputParameter;
        public struct User 
        {
            public string idUser; 
            public string idAnswer;
            public int classLable;
            public int classLable1;


            public User(string _idAnswer, string _idUser, int _classLable, int _classLable1) 
            {
                idAnswer = _idAnswer;
                idUser = _idUser;
                classLable = _classLable;
                classLable1 = _classLable1;
                
            }
        }

        List<User> users = new List<User>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
        }

        private void CheckTabl() 
        {
            dataGridView1.AllowUserToAddRows = false;
            users.Add(new User("6ffe4933-3202","Саша123452", 10, 18));
            users.Add(new User("451b-ae6b-fee85cafc829", "Маша2134", 7, 37));
            users.Add(new User("743f097a57fb", "Гоша23213", 13, 42));
            users.Add(new User("432ff432df", "Алена424352", 12, 36));
            users.Add(new User("5twfg23441234", "Гоша24673", 10, 42));
            users.Add(new User("tye64rw32454-", "Маша243563", 3, 57));
            users.Add(new User("324tfgdf321", "Гоша2212343", 6, 36));
            users.Add(new User("346253rwgwsg324254", "Саша111324321", 4, 28));
            users.Add(new User("fgsfser24gdf342", "Алена423422", 15, 31));
            users.Add(new User("6ffe49", "Маша245542", 9, 26));
            users.Add(new User("403dfd1a9945", "Саша11223", 3, 18));
            users.Add(new User("6ffe4933", "Маша21234", 9, 3));
            users.Add(new User("3202", "Гоша243523", 1, 19));
            users.Add(new User("613241324sdaf2234", "Маша22345", 8, 86));
            users.Add(new User("6sadfa2321", "Саша1674", 12, 59));

            DataTable table = new DataTable();
            table.Columns.Add("id Вопроса", typeof(string));
            table.Columns.Add("id Участника", typeof(string));
            table.Columns.Add("уровень стресса", typeof(int));
            table.Columns.Add("% грубых ошибок", typeof(int));

            for (int i = 0; i < users.Count; i++)
            {
                table.Rows.Add(users[i].idAnswer, users[i].idUser, users[i].classLable, users[i].classLable1);
            }
            dataGridView1.DataSource = table;
        }




        private void Import_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, Filter = "All files|*.*" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    _inputParameter.Username = username;
                    _inputParameter.Password = password;
                    _inputParameter.Server = server;
                    _inputParameter.FileName = fi.Name;
                    _inputParameter.FullName = fi.FullName;
                }
            }
                MessageBox.Show("импорт файла ", "Подождите идет обработка файла", MessageBoxButtons.OKCancel);
            
            CheckTabl();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, Filter = "All files|*.*" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    _inputParameter.Username = username;
                    _inputParameter.Password = password;
                    _inputParameter.Server = server;
                    _inputParameter.FileName = fi.Name;
                    _inputParameter.FullName = fi.FullName;
                }
            }
            MessageBox.Show("Экспорт файла ", "Подождите идет обработка файла", MessageBoxButtons.OKCancel);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string fileName = ((FtpSetting)e.Argument).FileName;
            string fullName = ((FtpSetting)e.Argument).FullName;
            string userName = ((FtpSetting)e.Argument).Username;
            string password = ((FtpSetting)e.Argument).Password;
            string server = ((FtpSetting)e.Argument).Server;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", server, fileName)));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(userName, password);
            Stream ftpStream = request.GetRequestStream();
            FileStream fs = File.OpenRead(fullName);
            byte[] buffer = new byte[1024];
            double total = (double)fs.Length;
            int byteRead = 0;
            double read = 0;
            do
            {
                if (!backgroundWorker1.CancellationPending)
                {
                    byteRead = fs.Read(buffer, 0, 1024);
                    ftpStream.Write(buffer, 0, byteRead);
                    read += (double)byteRead;
                    double precentage = read / total * 100;
                    backgroundWorker1.ReportProgress((int)precentage);
                }

            } while (byteRead !=0);
            fs.Close();
            ftpStream.Close();
        }
    }
}
