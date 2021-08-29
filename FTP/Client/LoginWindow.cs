using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;

namespace Client
{
    public partial class LoginWindow : Form
    {
        /**
         * This form includes user login set IP address of FTP server.
         * Once the control connection is done, it will turn to another form to do ftp operations
         * 
         * What I'm thinking about is to create a .dat file to store the IDs and IPs user used to input, but anyway, it's optional.
         * Another suggestion is to initialize the ftpSeverIP for default thus we don't need to create .dat file.
         * 
         *
         * 
         **/
        public string ftpServerIP;
        public string ftpUserID;
        public string ftpPassword;
        private MainWindow child;

        Point mouseOff;//用于获取鼠标位置
        bool leftFlag;//移动标识
        public LoginWindow()
        {
            InitializeComponent();
            this.IPBox.Text = "127.0.0.1";
            // ResourceManager rm = new ResourceManager("My_resources", typeof(Resources).Assembly);
            this.hideUser(true);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void hideUser(bool isHide)
        {
            if (isHide)
            {
                this.label2.Hide();
                this.label3.Hide();
                this.line1.Hide();
                this.line2.Hide();
                this.IDBox.Hide();
                this.IDBox.Clear();
                this.passwordBox.Clear();
                this.passwordBox.Hide();
                this.backBtn.Hide();
                //this.button1.Location = new Point(42, 360);
                this.label5.Show();
            }
            else
            {
                this.label2.Show();
                this.label3.Show();
                this.line1.Show();
                this.line2.Show();
                this.label5.Hide();
                this.IDBox.Show();
                this.label5.Hide();
                this.backBtn.Show();
                this.passwordBox.Show();
                //this.button1.Location = new Point(42, 541);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /**
             * Try to connect to the server
             **/
            // 我在想要不要这里就测试一下是否能连接服务器
            ftpServerIP = IPBox.Text.Trim();
            ftpUserID = IDBox.Text.Trim();
            ftpPassword = passwordBox.Text;
            bool success = false;
            if (ftpUserID == "test")
            {
                // 进入测试模式，不需要输入用户名和密码就可以对界面进行调试
                success = true;
            }
            else
            {
                // 必须要验证成功，才能够进入到主界面
                FtpWebRequest reqFTP;
                try
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/"));
                    reqFTP.UseBinary = true;
                    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    reqFTP.UsePassive = false;
                    WebResponse response = reqFTP.GetResponse();
                    response.Close();
                    success = true;
                }
                catch
                {
                    MessageBox.Show("无法连接Ftp服务器\n要仅测试界面，请在用户名输入test.");
                    success = false;
                    // 如果连接失败，是无法进入到主界面的
                }
            }
            if (success)
            {
                if (child == null)
                {
                    child = new MainWindow(this);
                }
                child.Show();
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.hideUser(false);
        }

        private void exitBtn_MouseEnter(object sender, EventArgs e)
        {
            this.exitBtn.BackgroundImage = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("close_clicked");
        }

        private void exitBtn_MouseLeave(object sender, EventArgs e)
        {
            this.exitBtn.BackgroundImage = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("close");
        }

        private void LoginWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y <= 40)
            {
                mouseOff = new Point(e.X, e.Y);//获取当前鼠标位置
                leftFlag = true;//用于标记窗体是否能移动(此时鼠标按下如果说用户拖动鼠标则窗体移动)
            }
        }

        private void LoginWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Location = new Point(Control.MousePosition.X - mouseOff.X, Control.MousePosition.Y - mouseOff.Y);
            }
        }

        private void LoginWindow_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false; //释放鼠标标识为false 表示窗体不可移动
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.hideUser(true);
        }
    }
}
