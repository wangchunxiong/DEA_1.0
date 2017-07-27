using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DEA_APP
{
    /// <summary>
    /// ConnectWind.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectWind : Window
    {
        //FTP对象
        FtpClient client = new FtpClient();
        //设置适配器IP地址
        string ftpServerIP1;
        bool online = false; //是否在线

        string ftpRemotePath1;
        string ftpUserID1;
        string ftpPassword1;
        string filename1;
        string ftpURI1;
        string appPath1;

        public ConnectWind()
        { 
            InitializeComponent(); 
            //ftp用户
            ftpUserID1 = "dea";
            //ftp密码
            ftpPassword1 = "dea123@@@";
        }

        //联机
        private void connect_Click(object sender, RoutedEventArgs e)
        {
            _DeaIp.IsEnabled = false;
            String dea_ip1 = _DeaIp.Text;
            ftpServerIP1 = _DeaIp.Text;

            if (!dea_ip1.Equals("") | !dea_ip1.Equals(null))
            {
                try
                {
                    // 上传文件
                    client.Host = ftpServerIP1;
                    client.Credentials = new NetworkCredential(ftpUserID1, ftpPassword1);
                    client.Port = 21;

                    if (checkLocalIP())
                    {
                        //连接适配器
                        client.Connect();
                        if (client.IsConnected)
                        {
                            online = true;
                            MessageBox.Show("联机成功！", "提示");
                            _Connect_but.IsEnabled = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("联机失败，请检查网络设置！", "提示");
                            _Connect_but.IsEnabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("本地IP设置异常！", "提示");
                        _Connect_but.IsEnabled = true;
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("联机失败，请检查网络设置！", "异常");
                    _Connect_but.IsEnabled = true;
                    return;
                }

            }
            else
            {
                MessageBox.Show("请输入适配器IP地址！", "提示");
                _Connect_but.IsEnabled = true;
            }
        }

        //检查IP设置
        private bool checkLocalIP()
        {
            bool ipok = false;
            try
            {
                string hostName = Dns.GetHostName();//本机名  
                System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6 
                foreach (IPAddress ip in addressList)
                {
                    if (ip.ToString().Contains(ftpServerIP1.Substring(1, 10)))
                    {
                        ipok = true;
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            return ipok;
        }
    }
}
