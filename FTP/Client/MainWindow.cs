using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace Client
{
    public partial class MainWindow : Form
    {
        private LoginWindow parent;
        Point mouseOff;//用于获取鼠标位置
        bool leftFlag;//移动标识
        public MainWindow(LoginWindow parent)
        {
            this.parent = parent;
            this.parent.Hide();
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();

            if (this.parent.ftpUserID == "test")
            {
                this.fileListBox.Items.Add("测试用例1");
                this.fileListBox.Items.Add("测试用例2");
                this.fileListBox.Items.Add("测试用例3");
            }
            else
            {
                this.fileListBox.Items.Clear();
            }
        }
        private void Upload(string filename)//上传方法
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" + this.parent.ftpServerIP + "/" + fileInf.Name;
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.parent.ftpServerIP + "/" + fileInf.Name));
            reqFTP.Credentials = new NetworkCredential(this.parent.ftpUserID, this.parent.ftpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.UsePassive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            //指定主动方式
            reqFTP.UsePassive = false;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            //打开一个文件流来读入上传的文件
            FileStream fs = fileInf.OpenRead();

            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                MessageBox.Show("上传成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "上传出错");
            }
        }

        public void DeleteFTP(string fileName)//删除功能
        {
            try
            {
                string uri = "ftp://" + this.parent.ftpServerIP + "/" + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.parent.ftpServerIP + "/" + fileName));

                reqFTP.Credentials = new NetworkCredential(this.parent.ftpUserID, this.parent.ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UsePassive = false;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
                MessageBox.Show("删除成功!");

            }
            catch
            {
                MessageBox.Show("删除失败，刷新或稍后再试!");
            }
        }
        public string[] GetFileList()//获取文件列表方法
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.parent.ftpServerIP + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this.parent.ftpUserID, this.parent.ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.UsePassive = false;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();

                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }

        }

        private string[] GetFilesDatailList(string filename)//获取文件详细信息列表
        {
            // if failed ,return null info.
            string[] ret = null;
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.parent.ftpServerIP + "/"));
                ftp.Credentials = new NetworkCredential(this.parent.ftpUserID, this.parent.ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                ftp.UsePassive = false;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] info = line.Split('\t');
                    if (info[0] == filename)
                    {
                        ret = info;
                        break;
                    }
                    else
                    {
                        line = reader.ReadLine();
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ret;
            }
        }

        private void Download(string filePath, string fileName)//下载方法
        {
            FtpWebRequest reqFTP;
            Uri u = new Uri("ftp://" + parent.ftpServerIP + "/" + fileName);
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(u);

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential(this.parent.ftpUserID, this.parent.ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch
            {

            }
        }

        private void Rename(string currentFilename, string newFilename)//重命名方法
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.parent.ftpServerIP + "/" + currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential(this.parent.ftpUserID, this.parent.ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
                MessageBox.Show("重命名成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                MessageBox.Show("请选择你需要删除的文件!");
            }
            else
            {
                string fileName = fileListBox.SelectedItem.ToString();
                DialogResult dr = MessageBox.Show($"确认要删除文件{fileName}吗?", "确认删除",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    DeleteFTP(fileName);
                    getFileBtn_Click(null, null);

                }
            }
        }

        private void getFileBtn_Click(object sender, EventArgs e)
        {
            string[] filenames = this.GetFileList();
            fileListBox.Items.Clear();
            try
            {
                foreach (string filename in filenames)
                {
                    fileListBox.Items.Add(filename);
                }
            }
            catch
            {

            }
        }

        private void uploadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog opFilDIg = new OpenFileDialog();
            if (opFilDIg.ShowDialog() == DialogResult.OK)
            {
                Upload(opFilDIg.FileName);
                getFileBtn_Click(null, null);

            }
        }

        private void detailButton_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                MessageBox.Show("请选择文件!");
            }
            else
            {
                string filename = fileListBox.SelectedItem.ToString();
                string[] details = GetFilesDatailList(filename);
                if (details == null)
                {
                    MessageBox.Show("无法获取文件的详细信息！请重试");
                }
                else
                {
                    MessageBox.Show(
                        $"Raw response fropm server to {parent.ftpUserID}:\n{details[0]}\n{details[1]}\n{details[2]}\n{details[3]}");
                }
            }
        }

        private void downloadBtn_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                MessageBox.Show("请选择需要下载的文件!");
                return;
            }
            FolderBrowserDialog fldDlg = new FolderBrowserDialog();
            string fileName = fileListBox.SelectedItem.ToString();
            if (fileName != string.Empty)
            {
                if (fldDlg.ShowDialog() == DialogResult.OK)
                {
                    Download(fldDlg.SelectedPath, fileName);
                }
            }
            else
            {
                MessageBox.Show("请选择下载的文件！");
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.parent.Show();
            this.Hide();
        }

        private void renameBtn_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                MessageBox.Show("必须选中你想要重命名的文件！");
            }
            else
            {
                string currentFileName = fileListBox.SelectedItem.ToString();
                string newFileName = filenameBox.Text.ToString();
                DialogResult dr = MessageBox.Show($"确认要重命名文件{currentFileName}->{newFileName}吗?", "确认重命名",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    if (newFileName.Trim() != "")
                    {
                        Rename(currentFileName, newFileName);
                        getFileBtn_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("重命名不能为空!");
                    }
                }
            }
        }

        private void fileListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color foreColor;
            Font font;
            e.DrawBackground();
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {//如果当前行为选中行。
             //绘制选中时要显示的蓝色边框。
                Color c = SystemColors.ControlDark;
                foreColor = Color.Black; //Color.FromArgb(6, 82, 121);
                font = new Font("黑体", 11, FontStyle.Bold);
                e.Graphics.FillRectangle(new SolidBrush(c), e.Bounds);//绘制背景
            }
            else
            {
                font = e.Font;
                foreColor = e.ForeColor;
            }

            //  e.DrawFocusRectangle();
            StringFormat strFmt = new System.Drawing.StringFormat();
            strFmt.Alignment = StringAlignment.Center; //文本垂直居中
            strFmt.LineAlignment = StringAlignment.Center; //文本水平居中

            e.Graphics.DrawString(fileListBox.Items[e.Index].ToString(), font, new SolidBrush(foreColor), e.Bounds, strFmt);
        }

        private void MainWindow_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOff = new Point(e.X, e.Y);//获取当前鼠标位置
            leftFlag = true;//用于标记窗体是否能移动(此时鼠标按下如果说用户拖动鼠标则窗体移动)
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Location = new Point(Control.MousePosition.X - mouseOff.X, Control.MousePosition.Y - mouseOff.Y);
            }
        }

        private void MainWindow_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false; //释放鼠标标识为false 表示窗体不可移动
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.parent.Close();
        }

        private void fileListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40;
        }

        private void MainWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.parent.Hide();
            }
            if (this.parent.ftpUserID == "test")
            {
                this.InfoLabel.Text = "Test Mode.";
            }
            else
            {
                this.InfoLabel.Text = $"Welcome! {this.parent.ftpUserID}. ";
            }
        }

        private void closeBtn_MouseEnter(object sender, EventArgs e)
        {
            this.closeBtn.BackgroundImage = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("close_clicked");
        }

        private void closeBtn_MouseLeave(object sender, EventArgs e)
        {
            this.closeBtn.BackgroundImage = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("close");
        }

        private void fileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem != null)
            {
                string filename = fileListBox.SelectedItem.ToString();
                filenameBox.Text = filename;
                string[] details = GetFilesDatailList(filename);
                titleLabel.Text = filename;
                if (details == null)
                {
                    DetailLabel.Text = "无法获取文件的详细信息！请重试";
                }
                else
                {
                    DetailLabel.Text = $"创建日期: {details[1]}\n文件大小: {details[2]}KB\n文件类型: {details[3]}";
                }
            }
        }
    }
}
