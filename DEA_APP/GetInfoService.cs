using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using PublicClass;
using System.Windows;
using System.IO;
using System.Windows.Controls;

namespace DEA_APP
{
    public class GetInfoService
    {   //获得任务号列表
        List<String> tasklist = new List<String>();
         
        //调试时使用
        string protocol_file_path = @"../../config_file/protocol";
        //发布时使用
        //string protocol_file_path = Directory.GetCurrentDirectory()+"\\config_file\\protocol";

        public List<String> GetTaskNumList()
        {
            tasklist.Clear();
            for (int i = 1; i < 50; i++)
            {
                tasklist.Add("任务" + i);
            } 
            return tasklist; 
            
        }

        //获得任务端口列表
        public List<String> GetComNumList()
        {
            tasklist.Clear();
            for (int i = 1; i < 20; i++)
            {
                tasklist.Add("COM" + i);
            }
            return tasklist;

        } 

        //获得任务读写下拉
        public List<String> GetReadWriteList()
        {
            tasklist.Clear();
            tasklist.Add("读");
            tasklist.Add("写");
            return tasklist; 
        }

        //获得任务站号列表
        public List<String> GetDeviceNumList()
        {
            tasklist.Clear();
            for (int i = 1; i < 50; i++)
            {
                tasklist.Add(i + "号站");
            }
            return tasklist; 
        }

        //获得端口设置站号列表
        public List<String> GetSpdList()
        {
            tasklist.Clear();
            tasklist.Add("2400");
            tasklist.Add("9600");
            tasklist.Add("38400");
            tasklist.Add("4800");
            tasklist.Add("19200");
            return tasklist;
        }

        //获得端口设置位长列表
        public List<String> GetBitList()
        {
            tasklist.Clear();
            tasklist.Add("7");
            tasklist.Add("8");
            return tasklist;
        }
        //获得端口设置同步位列表
        public List<String> GetSyncBitList()
        {
            tasklist.Clear();
            tasklist.Add("N[0]");
            tasklist.Add("O[1]");
            tasklist.Add("E[2]");
            return tasklist;
        }
        //获得端口设置停止位列表
        public List<String> GetStopBitList()
        {
            tasklist.Clear();
            tasklist.Add("1");
            tasklist.Add("2");
            return tasklist;
        }

        //加载端口中协议列表
        public List<String> GetProtocolList()
        {
            string ReadLine;
            string[] array;
            List<String> tasklist = new List<String>();
            //string Path = Directory.GetCurrentDirectory() + "/config_file/protocol";

            if (File.Exists(protocol_file_path))
            {
                StreamReader reader = new StreamReader(protocol_file_path,
                                  System.Text.Encoding.GetEncoding("GB2312"));
                while (reader.Peek() >= 0)
                {
                    try
                    {
                        ReadLine = reader.ReadLine();
                        if (ReadLine != "")
                        {
                            array = ReadLine.Split('\n');
                            if (array.Length == 0)
                            {
                                //MessageBox.Show("协议档案格式错误！", "提示");
                                return tasklist;
                            }
                            tasklist.Add(array[0]);
                        }
                        //return tasklist;

                        //_DataGrid_DeviceSet_ComBox_Protocol.ItemsSource = protocolData2;

                        //_DataGrid_DeviceSet_ComBox_Protocol.SelectedValuePath//SelectedValueBinding//SelectedItemBinding
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        return tasklist;
                    }

                }
                return tasklist;
            }
            else
            {
                //MessageBox.Show("协议档案不存在！", "提示");
                return tasklist;
            }

        }

    }

    
}
