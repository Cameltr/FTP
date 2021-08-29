using System;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server
{
    class UserInfo//用户类
    {
        internal String user;//用户名
        internal String password;//密码
        internal String workDir;//主目录

    }

    class FtpState//FTP的各个状态标志
    {
        internal const int FS_WAIT_LOGIN = 0;
        internal const int FS_WAIT_PASS = 1;
        internal const int FS_LOGIN = 2;
        internal const int FTYPE_ASCII = 0;
        internal const int FTYPE_IMAGE = 1;
        internal const int FMODE_STREAM = 0;
        internal const int FMODE_COMPRESSED = 1;
        internal const int FSTRU_FILE = 0;
        internal const int FSTRU_PAGE = 1;
    }

    class FtpHandler//处理用户在客户端请求的命令，然后通过服务器处理返回操作的响应码
    {
        // 初始化参数
        TcpClient csocket;
        TcpClient dsocket;
        int id;
        String cmd = "";
        String param = "";
        String user;
        String remoteHost = "";
        int remotePort = 0;
        String dir = FtpServer.initDir;
        String rootdir = "c:/";
        String currentFileName = "";
        String newFileName = "";
        int state = 0;
        String reply;
        StreamWriter out_Renamed;
        int type = 0;
        String requestfile = "";
        bool isrest = false;

        /// <summary>
        /// 解析输入命令字符串：解析输入的字符串得到命令名，返回相应的响应码
        /// </summary>
        /// <param name="s">输入的字符串</param>
        /// <returns>响应码</returns>
        int parseInput(String s)
        {
            Console.WriteLine(s);
            int p = 0;
            int i = -1;
            p = s.IndexOf(" ");
            if (p == -1)
            {
                cmd = s;
            }
            else
            {
                cmd = s.Substring(0, (p) - (0));
            }

            if (p >= s.Length || p == -1)
            {
                param = "";
            }
            else
            {
                param = s.Substring(p + 1, (s.Length) - (p + 1));
            }
            cmd = cmd.ToUpper();


            if (cmd.Equals("USER"))
                i = 1;
            if (cmd.Equals("PASS"))
                i = 2;
            if (cmd.Equals("ACCT"))
                i = 3;
            if (cmd.Equals("CDUP"))
                i = 4;
            if (cmd.Equals("SMNT"))
                i = 5;
            if (cmd.Equals("CWD"))
                i = 6;
            if (cmd.Equals("QUIT"))
                i = 7;
            if (cmd.Equals("REIN"))
                i = 8;
            if (cmd.Equals("PORT"))
                i = 9;
            if (cmd.Equals("PASV"))
                i = 10;
            if (cmd.Equals("TYPE"))
                i = 11;
            if (cmd.Equals("STRU"))
                i = 12;
            if (cmd.Equals("MODE"))
                i = 13;
            if (cmd.Equals("RETR"))
                i = 14;
            if (cmd.Equals("STOR"))
                i = 15;
            if (cmd.Equals("STOU"))
                i = 16;
            if (cmd.Equals("APPE"))
                i = 17;
            if (cmd.Equals("ALLO"))
                i = 18;
            if (cmd.Equals("REST"))
                i = 19;
            if (cmd.Equals("RNFR"))
                i = 20;
            if (cmd.Equals("RNTO"))
                i = 21;
            if (cmd.Equals("ABOR"))
                i = 22;
            if (cmd.Equals("DELE"))
                i = 23;
            if (cmd.Equals("RMD"))
                i = 24;
            if (cmd.Equals("XMKD"))
                i = 25;
            if (cmd.Equals("MKD"))
                i = 25;
            if (cmd.Equals("PWD"))
                i = 26;
            if (cmd.Equals("LIST"))
                i = 27;
            if (cmd.Equals("NLST"))
                i = 28;
            if (cmd.Equals("SITE"))
                i = 29;
            if (cmd.Equals("SYST"))
                i = 30;
            if (cmd.Equals("HELP"))
                i = 31;
            if (cmd.Equals("NOOP"))
                i = 32;
            if (cmd.Equals("XPWD"))
                i = 33;
            if (cmd.Equals("OPTS"))
                i = 34;
            if (cmd.Equals("SIZE"))
                i = 35;
            return i;
        } // parseInput() end

        /// <summary>
        /// 传入路径，验证是否合法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal int validatePath(String s)
        {
            FileInfo f = new FileInfo(s);
            bool tmpBool;
            if (File.Exists(f.FullName))
                tmpBool = true;
            else
                tmpBool = Directory.Exists(f.FullName);
            if (tmpBool && !Directory.Exists(f.FullName))
            {
                String s1 = s.ToLower();
                String s2 = rootdir.ToLower();
                if (s1.StartsWith(s2))
                    return 1;
                else
                    return 0;
            }
            f = new FileInfo(addTail(dir) + s);
            bool tmpBool2;
            if (File.Exists(f.FullName))
                tmpBool2 = true;
            else
                tmpBool2 = Directory.Exists(f.FullName);
            if (tmpBool2 && !Directory.Exists(f.FullName))
            {
                String s1 = (addTail(dir) + s).ToLower();
                String s2 = rootdir.ToLower();
                if (s1.StartsWith(s2))
                    return 2;
                else
                    return 0;
            }
            return 0;

        }

        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal bool checkPass(String s)
        {
            for (int i = 0; i < FtpServer.usersInfo.Count; i++)
            {
                if (((UserInfo)FtpServer.usersInfo[i]).user.Equals(user) &&
                ((UserInfo)FtpServer.usersInfo[i]).password.Equals(s))
                {
                    rootdir = ((UserInfo)FtpServer.usersInfo[i]).workDir;
                    dir = ((UserInfo)FtpServer.usersInfo[i]).workDir;
                    return true;
                }
            }
            return false;
        } // checkPass() end

        internal string getUSER()
        {
            return user;
        }

        internal bool commandUSER()//USER命令的业务逻辑函数
        {// 当用户输入USER命令后显示相应消息，置状态位
            if (cmd.Equals("USER"))
            {
                reply = "331 User name okay, need password";
                user = param;
                state = FtpState.FS_WAIT_PASS;
                return false;
            }
            else
            {
                reply = "501 Syntax error in parameters or arguments";
                return true;
            }
        } // commandUSER() end


        internal bool commandPASS()//登录
        {// 当用户输入PASS命令后，判断密码是否合法并执行相应的登录动作
            if (cmd.Equals("PASS"))
            {
                if (checkPass(param))
                {
                    reply = "230 User logged in, proceed";
                    state = FtpState.FS_LOGIN;
                    Console.Out.WriteLine("Message: user " + user + " Form " + remoteHost +
                        "Login");
                    Console.Out.Write("->");
                    return false;
                }
                else
                {
                    reply = "530 Not logged in";
                    return true;
                }
            }
            else
            {
                reply = "501 Syntax error in parameters or arguments";
                return true;
            }
        } // commandPASS() end

        internal void errCMD()//显示出错信息
        {
            reply = "500 Syntax error, command unrecognized";
        }


        internal bool commandCDUP()//寻找当前目录
        {
            dir = FtpServer.initDir;
            FileInfo f = new FileInfo(dir);
            if (f.DirectoryName != null && (!dir.Equals(rootdir)))
            {
                dir = f.DirectoryName;
                reply = "200 Command okay";
            }
            else
            {
                reply = "550 Current directory has no parent";
            }

            return false;
        } // commandCDUP() end

        internal bool commandCWD()//判别是否存在所对应的工作目录
        {
            FileInfo f = new FileInfo(param);
            String s = "";
            String s1 = "";
            if (dir.EndsWith("/"))
                s = dir;
            else
                s = dir + "/";
            FileInfo f1 = new FileInfo(s + param);

            bool tmpBool;
            if (File.Exists(f.FullName))
                tmpBool = true;
            else
                tmpBool = Directory.Exists(f.FullName);
            if (Directory.Exists(f.FullName) && tmpBool)
            {
                if (param.Equals("..") || param.Equals("..\\"))
                {
                    if (String.Compare(dir, rootdir, true) == 0)
                    {
                        reply = "550 The directory dose not exists";
                    }
                    else
                    {
                        s1 = new FileInfo(dir).DirectoryName;
                        if (s1 != null)
                        {
                            dir = s1;
                            reply = "250 Requested file action okay, directory change to " + dir;
                        }
                        else
                        {
                            reply = "550 The directory does not exists";
                        }
                    }
                }
                else if (param.Equals(".") || param.Equals(".\\"))
                {

                }
                else
                {
                    dir = param;
                    reply = "250 Requested file action okay, directory change to " + dir;
                }
            }
            else
            {
                bool tmpBool2;
                if (File.Exists(f1.FullName))
                    tmpBool2 = true;
                else
                    tmpBool2 = Directory.Exists(f1.FullName);
                if (Directory.Exists(f1.FullName) && tmpBool2)
                {
                    dir = s + param;
                    reply = "250 Requested file action okay, directory change to " + dir;
                }
                else
                    reply = "501 Syntax error in parameters or arguments";
            }

            return false;
        } // commandCWD() end

        internal bool commandQUIT() // 退出
        {
            reply = "221 Service closing control connection";
            return true;
        } // commandQUIT() end

        internal bool commandPORT() // 解析字符串，查看是否有格式问题
        {
            int p1 = 0;
            int p2 = 0;
            int[] a = new int[6];
            int i = 0;
            try
            {
                while ((p2 = param.IndexOf(",", p1)) != -1)
                {
                    a[i] = Int32.Parse(param.Substring(p1, (p2) - (p1)));
                    p2 = p2 + 1;
                    p1 = p2;
                    i++;
                }
                a[i] = Int32.Parse(param.Substring(p1, (param.Length) - (p1)));
            }
            catch (FormatException e)
            {
            }
            finally
            {
                remoteHost = a[0] + "." + a[1] + "." + a[2] + "." + a[3];
                remotePort = a[4] * 256 + a[5];
                reply = "200 Command okay";

            }
            return false;

        } // commandPORT() end

        internal bool commandLIST() // 命令LIST，启动二进制传输模式
        {
            try
            {
                Console.WriteLine(remoteHost);
                dsocket = new TcpClient(remoteHost, remotePort);
                StreamWriter temp_writer;
                temp_writer = new StreamWriter(dsocket.GetStream(), Encoding.Default);
                StreamWriter dout = temp_writer;
                if (param.Equals("") || param.Equals("LIST"))
                {
                    out_Renamed.WriteLine("150 Opening ASCII mode data connection for/bin/ls");
                    FileInfo f = new FileInfo(dir);
                    DirectoryInfo di = new DirectoryInfo(f.FullName);
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        //format: 
                        //filename  lastwritetime length extension 
                        dout.WriteLine(fi.Name + "\t" + fi.LastWriteTime.ToString() +
                            "\t" + fi.Length.ToString() + "\t" + fi.Extension.ToString() + "\t");
                    }
                }
                dout.Close();
                dsocket.Close();
                reply = "226 Transfer complete !";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                reply = "451 Requested action aborted: local error in processing";
                return false;
            }
            finally { }
            return false;
        } // commandLIST() end

        internal bool commandNLST() // 命令NLST，启动二进制传输模式
        {
            try
            {
                Console.WriteLine(remoteHost);
                dsocket = new TcpClient(remoteHost, remotePort);
                StreamWriter temp_writer;
                temp_writer = new StreamWriter(dsocket.GetStream(), Encoding.Default);
                StreamWriter dout = temp_writer;
                if (param.Equals("") || param.Equals("LIST"))
                {
                    out_Renamed.WriteLine("150 Opening ASCII mode data connection for/bin/ls");
                    FileInfo f = new FileInfo(dir);
                    // String[] dirStructure = Directory.GetFiles(f.FullName);
                    // String fileType;
                    DirectoryInfo di = new DirectoryInfo(f.FullName);
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        dout.WriteLine(fi.Name);
                    }
                }
                dout.Close();
                dsocket.Close();
                reply = "226 Transfer complete !";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                reply = "451 Requested action aborted: local error in processing";
                return false;
            }
            finally { }
            return false;
        } // commandNLST() end


        internal bool commandTYPE() //命令TYPE：获取命令类型
        {
            if (param.Equals("a"))
            {
                type = FtpState.FTYPE_ASCII;
                reply = "200 Command okay Change to ASCII mode";
            }
            else if (param.Equals("I"))
            {
                type = FtpState.FTYPE_IMAGE;
                reply = "200 Command okay Change to BINARY mode";
            }
            else
            {
                reply = "504 Command not implemented for that parameter";
            }

            return false;
        } // commandTYPE() end

        internal bool commandRETR() //启动二进制传输模式，初始化工作环境
        {
            requestfile = param;
            FileInfo f = new FileInfo(requestfile);
            bool tmpBool;
            if (File.Exists(f.FullName))
                tmpBool = true;
            else
                tmpBool = Directory.Exists(f.FullName);
            if (!tmpBool)
            {
                f = new FileInfo(addTail(dir) + param);
                bool tmpBool2;
                if (File.Exists(f.FullName))
                    tmpBool2 = true;
                else
                    tmpBool2 = Directory.Exists(f.FullName);
                if (!tmpBool2)
                {
                    reply = "550 File not found";
                    return false;
                }
                requestfile = addTail(dir) + param;
            }
            if (isrest)
            {

            }
            else
            {
                if (type == FtpState.FTYPE_IMAGE)
                {
                    try
                    {
                        out_Renamed.WriteLine("150 Opening Binary mode data connection for " +
                            requestfile);
                        dsocket = new TcpClient(remoteHost, remotePort);
                        BufferedStream fin = new BufferedStream(new FileStream(requestfile, FileMode.Open, FileAccess.Read));
                        NetworkStream dout = dsocket.GetStream();
                        byte[] buf = new byte[1024];
                        int l = 0;
                        while ((l = fin.Read(buf, 0, 1024)) != 0)
                        {
                            dout.Write(buf, 0, l);
                        }
                        fin.Close();
                        dout.Close();
                        dsocket.Close();
                        reply = "226 Transfer complete !";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        reply = "451 Requested action aborted: local error in processing";
                        return false;
                    }
                    finally { }
                }
                if (type == FtpState.FTYPE_ASCII)
                {
                    try
                    {
                        out_Renamed.WriteLine("150 Opening ASCII mode data connection for " +
                            requestfile);
                        dsocket = new TcpClient(remoteHost, remotePort);
                        StreamReader fin = new StreamReader(new StreamReader(requestfile, Encoding.Default).BaseStream,
                            new StreamReader(requestfile, Encoding.Default).CurrentEncoding);
                        StreamWriter temp_writer;
                        temp_writer = new StreamWriter(dsocket.GetStream(), Encoding.Default);
                        temp_writer.AutoFlush = true;
                        StreamWriter dout = temp_writer;
                        String s;
                        while ((s = fin.ReadLine()) != null)
                        {
                            dout.WriteLine(s);
                        }
                        fin.Close();
                        dout.Close();
                        dsocket.Close();
                        reply = "226 Transfer complete !";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        reply = "451 Requested action aborted: local error in processing";
                        return false;
                    }
                    finally { }
                }
            }
            return false;
        } // commandRETR() end


        internal bool commandSTOR()//获取当前目录
        {
            if (param.Equals(""))
            {
                reply = "501 Syntax error in parameters or arguments";
                return false;
            }
            requestfile = addTail(dir) + param;
            if (type == FtpState.FTYPE_IMAGE)
            {
                try
                {
                    out_Renamed.WriteLine("150 Opening Binary mode data connection for " +
                        requestfile);
                    dsocket = new TcpClient(remoteHost, remotePort);
                    BufferedStream fout = new BufferedStream(new FileStream(requestfile, FileMode.Create));
                    BufferedStream din = new BufferedStream(dsocket.GetStream());
                    byte[] buf = new byte[1024];
                    int l = 0;
                    while ((l = din.Read(buf, 0, 1024)) != 0)
                    {
                        fout.Write(buf, 0, l);
                    }
                    din.Close();
                    fout.Close();
                    dsocket.Close();
                    reply = "226 Transfer complete !";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    reply = "451 Requested action aborted: local error in processing";
                    return false;
                }
                finally { }
            }
            if (type == FtpState.FTYPE_ASCII)
            {
                try
                {
                    out_Renamed.WriteLine("150 Opening ASCII mode data connection for " +
                        requestfile);
                    dsocket = new TcpClient(remoteHost, remotePort);
                    StreamWriter fout = new StreamWriter(new FileStream(requestfile,
                        FileMode.Create), Encoding.Default);
                    StreamReader din = new StreamReader(new StreamReader(dsocket.GetStream(),
                        Encoding.Default).BaseStream,
                        new StreamReader(dsocket.GetStream(), Encoding.Default).CurrentEncoding);
                    String line;
                    while ((line = din.ReadLine()) != null)
                    {
                        fout.WriteLine(line);
                    }
                    din.Close();
                    fout.Close();
                    dsocket.Close();
                    reply = "226 Transfer complete !";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    reply = "451 Requested action aborted: local error in processing";
                    return false;
                }
                finally { }
            }
            return false;
        } //commandSTOR() end

        internal bool commandPWD() // 显示当前目录
        {
            reply = "257 " + dir + " is current directory.";
            return false;
        } //commandPWD() end

        internal bool commandNOOP() // 确认
        {
            reply = "200 OK.";
            return false;
        } //commandNOOP end

        internal bool commandABOR() // 关闭套接字
        {
            try
            {
                dsocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                reply = "451 Requested action aborted: local error in processing";
                return false;
            }
            finally { }
            reply = "421 Service not available, closing control connection";
            return false;
        } //commandABOR() end

        internal bool commandDELE() // 删除文件
        {
            int i = validatePath(param);
            if (i == 0)
            {
                reply = "550 Request action not taken";
                return false;
            }
            if (i == 1)
            {
                FileInfo f = new FileInfo(param);
                bool tmpBool;
                if (File.Exists(f.FullName))
                {
                    File.Delete(f.FullName);
                    tmpBool = true;
                }
                else if (Directory.Exists(f.FullName))
                {
                    Directory.Delete(f.FullName);
                    tmpBool = true;
                }
                else
                {
                    tmpBool = false;
                }
                bool generatedAux = tmpBool;
            }
            if (i == 2)
            {
                FileInfo f = new FileInfo(addTail(dir) + param);
                bool tmpBool2;
                if (File.Exists(f.FullName))
                {
                    File.Delete(f.FullName);
                    tmpBool2 = true;
                }
                else if (Directory.Exists(f.FullName))
                {
                    Directory.Delete(f.FullName);
                    tmpBool2 = true;
                }
                else
                {
                    tmpBool2 = false;
                }
                bool generatedAux2 = tmpBool2;
            }

            reply = "250 Request file action ok, complete.";
            return false;
        } //commandDELE() end

        internal bool commandRNFR()
        {
            int i = validatePath(param);
            if (i == 0)
            {
                reply = "550 Request action not taken";
                return false;
            }
            if (i == 1)
            {
                FileInfo f = new FileInfo(param);
                bool tmpBool;
                if (File.Exists(f.FullName))
                {
                    currentFileName = f.FullName;
                    tmpBool = true;
                }
                //else if (Directory.Exists(f.FullName))
                //{
                //    Directory.Delete(f.FullName);
                //    tmpBool = true;
                //}
                else
                {
                    tmpBool = false;
                }
                bool generatedAux = tmpBool;
            }
            if (i == 2)
            {
                FileInfo f = new FileInfo(addTail(dir) + param);
                bool tmpBool2;
                if (File.Exists(f.FullName))
                {
                    currentFileName = f.FullName;
                    tmpBool2 = true;
                }
                //else if (Directory.Exists(f.FullName))
                //{
                //    Directory.Delete(f.FullName);
                //    tmpBool2 = true;
                //}
                else
                {
                    tmpBool2 = false;
                }
                bool generatedAux2 = tmpBool2;
            }

            reply = "250 Request file action ok, complete.";
            return false;
        } //commandRNFR() end

        internal bool commandRNTO()
        {
            if (param.Equals(""))
            {
                reply = "553 Request action not taken: filename is invalid";
                return false;
            }
            newFileName = addTail(dir) + param;
            try
            {
                File.Move(currentFileName, newFileName);
                reply = "250 Request file action ok, complete.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                reply = "553 Request action not taken: filename is invalid";
                return false;
            }
            finally { };
            return false;
        } //commandRNTO() end

        internal bool commandMKD() // 创建目录
        {
            String s1 = param.ToLower();
            String s2 = rootdir.ToLower();
            if (s1.StartsWith(s2))
            {
                FileInfo f = new FileInfo(param);
                bool tmpBool;
                if (File.Exists(f.FullName))
                    tmpBool = true;
                else
                    tmpBool = Directory.Exists(f.FullName);
                if (tmpBool)
                {
                    reply = "550 Request action not taken";
                    return false;
                }
                else
                {
                    Directory.CreateDirectory(f.FullName);
                    reply = "250 Request file action ok, complete.";
                }
            }
            else
            {
                FileInfo f = new FileInfo(addTail(dir) + param);
                bool tmpBool2;
                if (File.Exists(f.FullName))
                    tmpBool2 = true;
                else
                    tmpBool2 = Directory.Exists(f.FullName);
                if (tmpBool2)
                {
                    reply = "550 Request action not taken";
                    return false;
                }
                else
                {
                    Directory.CreateDirectory(f.FullName);
                    reply = "250 Request file action ok, complete.";
                }
            }

            return false;
        } //commandMKD() end

        internal bool commandSIZE() // 获取文件长度
        {
            requestfile = param;
            try
            {
                Console.WriteLine(remoteHost);
                dsocket = new TcpClient(remoteHost, remotePort);
                StreamWriter temp_writer;
                temp_writer = new StreamWriter(dsocket.GetStream(), Encoding.Default);
                StreamWriter dout = temp_writer;
                out_Renamed.WriteLine("150 Opening ASCII mode data connection for " +
                    requestfile);
                FileInfo f = new FileInfo(requestfile);
                dout.WriteLine(f.Length.ToString());
                dout.Close();
                dsocket.Close();
                reply = "226 Transfer complete !";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                reply = "451 Requested action aborted: local error in processing";
                return false;
            }
            finally { }
            return false;
        }


        internal String addTail(String s) // 增加尾缀"/"
        {
            if (!s.EndsWith("/"))
                s = s + "/";
            return s;
        }

        public FtpHandler(TcpClient s, int i) // 构造函数
        {
            csocket = s;
            id = i;
        }

        public void HandleThread() // 命令翻译
        {
            String str = "";
            int parseResult;

            try
            {
                StreamReader in_Renamed = new StreamReader(csocket.GetStream(), Encoding.Default);
                StreamWriter temp_writer;
                temp_writer = new StreamWriter(csocket.GetStream(), Encoding.Default);
                temp_writer.AutoFlush = true;
                out_Renamed = temp_writer;
                state = FtpState.FS_WAIT_LOGIN;
                bool finished = false;
                while (!finished)
                {
                    str = in_Renamed.ReadLine();
                    if (str == null)
                        finished = true;
                    else
                    {
                        parseResult = parseInput(str);
                        Console.Out.WriteLine("Command:" + cmd + " Parameter:" + param);
                        Console.Out.Write("->");
                        switch (state)
                        {
                            case FtpState.FS_WAIT_LOGIN:
                                finished = commandUSER();
                                break;

                            case FtpState.FS_WAIT_PASS:
                                finished = commandPASS();
                                break;

                            case FtpState.FS_LOGIN:
                                {
                                    switch (parseResult)
                                    {
                                        case -1:
                                            errCMD();
                                            break;

                                        case 4:
                                            finished = commandCDUP();
                                            break;

                                        case 6:
                                            finished = commandCWD();
                                            break;

                                        case 7:
                                            finished = commandQUIT();
                                            break;

                                        case 9:
                                            finished = commandPORT();
                                            break;

                                        case 27:
                                            finished = commandLIST();
                                            break;
                                        case 28:
                                            finished = commandNLST();
                                            break;

                                        case 11:
                                            finished = commandTYPE();
                                            break;

                                        case 14:
                                            finished = commandRETR();
                                            break;

                                        case 15:
                                            finished = commandSTOR();
                                            break;

                                        case 20:
                                            finished = commandRNFR();
                                            break;

                                        case 21:
                                            finished = commandRNTO();
                                            break;

                                        case 26:
                                        case 33:
                                            finished = commandPWD();
                                            break;

                                        case 32:
                                            finished = commandNOOP();
                                            break;

                                        case 22:
                                            finished = commandABOR();
                                            break;

                                        case 23:
                                            finished = commandDELE();
                                            break;

                                        case 25:
                                            finished = commandMKD();
                                            break;

                                        case 35:
                                            finished = commandSIZE();
                                            break;
                                    } // switch(parseResult) end
                                } // case FtpState.FS_LOGIN: end
                                break;
                        } // switch(state) end
                    } // else
                    out_Renamed.WriteLine(reply);
                } // while
                csocket.Close();
            }
            //try
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally { }
        }
    }

    class FtpConsole//承担在控制台上的所有业务逻辑
    {
        internal StreamReader cin;
        internal String conCmd;
        internal String conParam;

        internal int consoleQUIT()//退出
        {
            Environment.Exit(0);
            return 0;
        }

        internal bool consoleLISTUSER()//列出用户列表
        {
            Console.Out.WriteLine("username \t\t workdirectory");
            for (int i = 0; i < FtpServer.usersInfo.Count; i++)
            {
                Console.Out.Write(((UserInfo)FtpServer.usersInfo[i]).user + "\t\t\t" + ((UserInfo)FtpServer.usersInfo[i]).workDir);
            }
            Console.Out.WriteLine("");
            return false;
        }

        internal bool consoleLIST()//判断是否是系统用户
        {
            int i = 0;
            for (i = 0; i < FtpServer.users.Count; i++)
            {
                Console.Out.WriteLine(((FtpHandler)FtpServer.users[i]).getUSER());
            }
            return false;
        }

        internal bool validateUserName(String s)//根据传入的用户名称验证是否与文件中的名称匹配
        {
            for (int i = 0; i < FtpServer.usersInfo.Count; i++)
            {
                if (((UserInfo)FtpServer.usersInfo[i]).user.Equals(s))
                    return false;
            }
            return true;
        }

        internal bool consoleADDUSER()//增加用户
        {
            Console.Out.Write("please enter username:");
            try
            {
                cin = new StreamReader(new StreamReader(Console.OpenStandardInput(), Encoding.Default).BaseStream, new StreamReader(Console.OpenStandardInput(), Encoding.Default).CurrentEncoding);
                UserInfo tempUserInfo = new UserInfo();
                String line = cin.ReadLine();
                if ((object)line != (object)"")
                {
                    //用户已经存在
                    if (!validateUserName(line))
                    {
                        Console.Out.WriteLine("user" + line + "already exsits!");
                        return false;
                    }
                }
                else
                {
                    //出错用户名不能为空
                    Console.Out.WriteLine("username cannnot be null!");
                    return false;
                }
                tempUserInfo.user = line;
                Console.Out.Write("enter password:");
                line = cin.ReadLine();
                if ((Object)line != (Object)"")
                    tempUserInfo.password = line;
                else
                {
                    Console.Out.WriteLine("password cannot be null:");
                    return false;
                }
                Console.Out.Write("enter the initial directory:");
                line = cin.ReadLine();
                if ((object)line != (Object)"")
                {
                    FileInfo f = new FileInfo(line);
                    bool tmpBool;
                    if (File.Exists(f.FullName))
                        tmpBool = true;
                    else
                        tmpBool = Directory.Exists(f.FullName);
                    if (!tmpBool)
                    {
                        Directory.CreateDirectory(f.FullName);
                    }
                    tempUserInfo.workDir = line;
                }
                else
                {
                    Console.Out.WriteLine("the directory cannot be null!");
                    return false;
                }
                FtpServer.usersInfo.Add(tempUserInfo);
                saveUserInfo();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            finally
            {

            }
            return false;//上述操作都没有出现错误
        }

        internal void saveUserInfo()//保存用户信息函数
        {
            String s = "";
            try
            {
                StreamWriter fout = new StreamWriter(new FileStream("user.cfg", FileMode.OpenOrCreate), Encoding.Default);
                for (int i = 0; i < FtpServer.usersInfo.Count; i++)
                {
                    s = ((UserInfo)FtpServer.usersInfo[i]).user + "|" + ((UserInfo)FtpServer.usersInfo[i]).password + "|" + ((UserInfo)FtpServer.usersInfo[i]).workDir + "|";
                    fout.Write(s);

                    fout.WriteLine();
                }
                fout.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {

            }
        }

        internal bool consoleDELUSER()//删除用户
        {
            String s = "";
            if (conParam.Equals(""))
            {
                Console.Out.WriteLine("usage:deluser username");
                return false;
            }
            for (int i = 0; i < FtpServer.usersInfo.Count; i++)
            {
                s = ((UserInfo)FtpServer.usersInfo[i]).user;
                if (s.Equals(conParam))
                {
                    Console.Out.WriteLine("User" + conParam + "deleted");
                    FtpServer.usersInfo.RemoveAt(i);
                    saveUserInfo();
                    return false;
                }
            }
            Console.Out.WriteLine("User" + conParam + "not exists");
            return false;
        }

        internal bool consoleHELP()//显示帮助
        {
            if (conParam.Equals(""))
            {
                Console.Out.WriteLine("adduser:add new user");
                Console.Out.WriteLine("deluser<username>:delete a user");
                Console.Out.WriteLine("quit :quit");
                Console.Out.WriteLine("list :list all user connect to server");
                Console.Out.WriteLine("listener:list all account of this server");
                Console.Out.WriteLine("help:show this help");
            }
            else if (conParam.Equals("adduser"))
                Console.Out.WriteLine("adduser:add new user");
            else if (conParam.Equals("deluser"))
                Console.Out.WriteLine("deluser<username>:delete a user");
            else if (conParam.Equals("qiut"))
                Console.Out.WriteLine("quit :quit");
            else if (conParam.Equals("list"))
                Console.Out.WriteLine("list :list all user connect to server");
            else if (conParam.Equals("listener"))
                Console.Out.WriteLine("listener:list all account of this server");
            else if (conParam.Equals("help"))
                Console.Out.WriteLine("help:show this help");
            else
                return false;
            return false;
        }

        internal bool consoleERR()//出错函数
        {
            Console.Out.WriteLine("bad command!");
            return false;
        }

        public FtpConsole()//构造函数
        {
            Console.Out.WriteLine("ftp server started!");
            cin = new StreamReader(new StreamReader(Console.OpenStandardInput(), Encoding.Default).BaseStream, new StreamReader(Console.OpenStandardInput(), Encoding.Default).CurrentEncoding);
        }

        public void ConsoleThread()//控制台线程函数
        {
            bool ok = false;
            String input = "";
            while (!ok)
            {
                Console.Out.Write("->");
                try
                {
                    //读入一行内容
                    input = cin.ReadLine();
                }
                catch (Exception e)
                {
                    //读入失败，打印错误信息
                    Console.WriteLine(e.ToString());
                }
                finally
                {

                }
                //判断输入命令内容
                switch (parseInput(input))
                {
                    case 1:
                        consoleQUIT();
                        break;
                    case 8:
                        ok = consoleLISTUSER();
                        break;
                    case 0:
                        ok = consoleLIST();
                        break;
                    case 2:
                        ok = consoleADDUSER();
                        break;
                    case 3:
                        ok = consoleDELUSER();
                        break;
                    case 7:
                        ok = consoleHELP();
                        break;
                    case -1:
                        ok = consoleERR();
                        break;
                }
            }
        }

        internal int parseInput(String s)//解析输入的函数
        {
            //变量初始化
            //大写命令名
            String upperCmd;
            int p = 0;
            conCmd = "";
            conParam = "";
            p = s.IndexOf(" ");
            if (p == -1)
                conCmd = s;
            else
                conCmd = s.Substring(0, (p) - (0));
            if (p >= s.Length || p == -1)
                conParam = "";
            else
                conParam = s.Substring(p + 1, (s.Length) - (p + 1));
            upperCmd = conCmd.ToUpper();

            if (upperCmd.Equals("LIST"))
                return 0;
            else if (upperCmd.Equals("QUIT") || upperCmd.Equals("EXIT"))
                return 1;
            else if (upperCmd.Equals("ADDUSER"))
                return 2;
            else if (upperCmd.Equals("DELUSER"))
                return 3;
            else if (upperCmd.Equals("EDITUSER"))
                return 4;
            else if (upperCmd.Equals("ADDDIR"))
                return 5;
            else if (upperCmd.Equals("REMOVEDIR"))
                return 6;
            else if (upperCmd.Equals("HELP") || upperCmd.Equals("?"))
                return 7;
            else if (upperCmd.Equals("LISTENER"))
                return 8;
            return -1;
        }
    }

    public class FtpServer
    {
        public static String initDir;
        public static ArrayList users = new ArrayList();
        public static ArrayList usersInfo = new ArrayList();

        public FtpServer()
        {
            FtpConsole fc = new FtpConsole();
            Thread t = new Thread(new ThreadStart(fc.ConsoleThread));
            t.Start();
            loadUsersInfo();
            int counter = 1;
            int i = 0;
            try
            {
                TcpListener tcpListener;
                tcpListener = new TcpListener(IPAddress.Any, 21);
                tcpListener.Start();
                TcpListener s = tcpListener;
                //Console.WriteLine("I'm listening.");
                while (true)
                {
                    TcpClient incoming = s.AcceptTcpClient();
                    StreamReader in_Renamed = new StreamReader(incoming.GetStream(), Encoding.Default);
                    StreamWriter temp_writer = new StreamWriter(incoming.GetStream(), Encoding.Default);
                    temp_writer.AutoFlush = true;
                    StreamWriter out_Renamed = temp_writer;
                    out_Renamed.WriteLine("220 Service ready for new user" + counter);

                    //创建服务线程
                    FtpHandler h = new FtpHandler(incoming, i);
                    Thread t1 = new Thread(new ThreadStart(h.HandleThread));
                    t1.Start();
                    users.Add(h);
                    counter++;
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {

            }
        }

        public void loadUsersInfo()//加载用户信息
        {
            String s = new Uri(Path.GetFullPath("user.cfg")).ToString();//该文件保存用户名密码等信息
            s = s.Substring(6, s.Length - 6);
            int p1 = 0;
            int p2 = 0;
            bool tmpBool;
            if (File.Exists(s))
            {
                tmpBool = true;
            }
            else
            {
                tmpBool = Directory.Exists(s);
            }
            if (tmpBool)
            {
                try
                {
                    StreamReader fin = new StreamReader(new StreamReader(new FileStream(s, FileMode.Open, FileAccess.Read), Encoding.Default).BaseStream, new StreamReader(new FileStream(s, FileMode.Open, FileAccess.Read), Encoding.Default).CurrentEncoding);
                    String line;
                    String field;
                    int i = 0;
                    while ((line = fin.ReadLine()) != null)
                    {
                        UserInfo tempUserInfo = new UserInfo();
                        p1 = 0;
                        p2 = 0;
                        i = 0;
                        if (line.StartsWith("#"))
                            continue;
                        while ((p2 = line.IndexOf("|", p1)) != -1)
                        {
                            field = line.Substring(p1, (p2) - (p1));
                            p2 = p2 + 1;
                            p1 = p2;
                            switch (i)
                            {
                                case 0:
                                    tempUserInfo.user = field;
                                    break;
                                case 1:
                                    tempUserInfo.password = field;
                                    break;
                                case 2:
                                    tempUserInfo.workDir = field;
                                    break;
                            }
                            i++;
                        }
                        usersInfo.Add(tempUserInfo);
                    }
                    fin.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {

                }
            }
        }

        public static void Main(string[] args)
        {
            initDir = "c:/";
            FtpServer ftpServer = new FtpServer();
            Console.WriteLine("#");

        }
    }
}

public class ImageClass
{
    public Image ResourceImage;
    private int ImageWidth;
    private int ImageHeight;

    public string ErrMessage;

    /// <summary>   
    /// 类的构造函数   
    /// </summary>   
    /// <param name="ImageFileName">图片文件的全路径名称</param>   
    public ImageClass(string ImageFileName)
    {
        ResourceImage = Image.FromFile(ImageFileName);
        ErrMessage = "";
    }

    public bool ThumbnailCallback()
    {
        return false;
    }

    /// <summary>   
    /// 生成缩略图重载方法1，返回缩略图的Image对象   
    /// </summary>   
    /// <param name="Width">缩略图的宽度</param>   
    /// <param name="Height">缩略图的高度</param>   
    /// <returns>缩略图的Image对象</returns>   
    public Image GetReducedImage(double Percent)
    {
        try
        {
            Image ReducedImage;

            Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            ImageWidth = Convert.ToInt32(ResourceImage.Width * Percent);
            ImageHeight = Convert.ToInt32(ResourceImage.Width * Percent);

            ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

            return ReducedImage;
        }
        catch (Exception e)
        {
            ErrMessage = e.Message;
            return null;
        }
    }
}
