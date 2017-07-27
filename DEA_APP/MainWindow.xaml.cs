using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PublicClass;
using System.Runtime.Serialization;
using System.Collections;
using Microsoft.Win32;
using System.Net;
using FluentFTP;
using MinimalisticTelnet;
using System.Globalization;
using System.Management;
using System.Web.Script.Serialization;

namespace DEA_APP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {   private string appVersion = "10000";
        bool online = false; //是否在线
        //端口设置DataTable  
        DataTable dt = new DataTable();
        //任务设置DataTable  
        DataTable TaskTable = new DataTable();
        //设置适配器IP地址
        string ftpServerIP;
        //FTP对象
        FtpClient client = new FtpClient();
        string ftpRemotePath ;
        string ftpUserID ;
        string ftpPassword ;
        string filename ;
        string ftpURI ;
        string appPath ;
        //string protocol_file_path;
        string isMainDevice;
        string isMainTxt;

        public MainWindow()
        {
            InitializeComponent(); 
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //_Grid_DeviceConnect.Visibility = Visibility.Hidden;
            _Grid_DeviceSet.Visibility = Visibility.Hidden;
            _Grid_TaskSet.Visibility = Visibility.Hidden;
            ftpServerIP = "192.168.1.233";
            //FTP工作目录
            ftpRemotePath = "/";
            //ftp用户
            ftpUserID = "dea";
            //ftp密码
            ftpPassword = "dea123@@@";
            //上传后的配置文件名称
            filename = "config.json";
            //ftp地址
            ftpURI = "ftp://" + ftpServerIP + ftpRemotePath;
            //获取当前应用程序工作目录
            appPath = Directory.GetCurrentDirectory() + "\\";
            //调试时使用
            //protocol_file_path = @"../../config_file/protocol";
            //发布时使用
            //protocol_file_path = Directory.GetCurrentDirectory()+"\\config_file\\protocol";
        }

        TaskType taskData = new TaskType();
        
        //初始化端口设置中的数据
        private void _DataGrid_DeviceSet_Loaded(object sender, RoutedEventArgs e)
        { 
            //初始化DviceSetDataGrid 
            DataRow dr;
            for (int i = 0; i < 2; i++)
            { 
                dr = dt.NewRow();  
                dt.Rows.Add(dr);
            }
            _DataGrid_DeviceSet.ItemsSource = dt.DefaultView; 
        }
        //手动增加端口行
        private void _Add_Devece_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            //MessageBox.Show(dt.Rows.Count.ToString());
            //SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, dt.Rows.Count-1, "_DataGrid_DeviceSet_ComBox_DeviceSet", "COM"+ (dt.Rows.Count - 1).ToString());
        }
         
        //获取端口选中列表数据集
        private DeviceType DeviceTypeInfo()
        {    
            //获取列对象控件值  
            PublicClass.DeviceType devicetype = new DeviceType();
            int rowNumCont = _DataGrid_DeviceSet.Items.Count;
            if (rowNumCont > 0)
            { 
                //获取行
                int deviceRowNum = _DataGrid_DeviceSet.SelectedIndex;
                // 获取DataGrid 里面DataGridComboBoxColumn列控件值
                //DataGridRow task_dgrow = (DataGridRow)_DataGrid_DeviceSet.ItemContainerGenerator.ContainerFromIndex(rowNum);
                devicetype.DeviceSet_ComBox_DeviceSet = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, deviceRowNum, "_DataGrid_DeviceSet_ComBox_DeviceSet");
                devicetype.DeviceSet_ComBox_Protocol = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 2, deviceRowNum, "_DataGrid_DeviceSet_ComBox_Protocol");
                devicetype.DeviceSet_ComBox_Spd = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 3, deviceRowNum, "_DataGrid_DeviceSet_ComBox_Spd");
                devicetype.DeviceSet_ComBox_Bit = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 4, deviceRowNum, "_DataGrid_DeviceSet_ComBox_Bit");
                devicetype.DeviceSet_ComBox_Sync_Bit = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 5, deviceRowNum, "_DataGrid_DeviceSet_ComBox_Sync_Bit");
                devicetype.DeviceSet_ComBox_Stop_Bit = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 6, deviceRowNum, "_DataGrid_DeviceSet_ComBox_Stop_Bit");
                devicetype.DeviceSet_RadioBut_Is_Main = Convert.ToBoolean(GetDataTemplateData("RadioButton", _DataGrid_DeviceSet, 7, deviceRowNum, "_DataGrid_DeviceSet_RadioBut_Is_Main"));
                devicetype.DeviceSet_TextBox_Site = GetDataTemplateData("TextBox", _DataGrid_DeviceSet, 8, deviceRowNum, "_DropDevice_siteTxtBox");
                devicetype.DropDevice_TextBox_stop = GetDataTemplateData("TextBox", _DataGrid_DeviceSet, 9, deviceRowNum, "_DropDevice_stopTxtBox");
            }
            return devicetype;
        }
        //获取端口中所有的数据集
        private List<DeviceType> DeviceTypeAllData()
        {
            List<DeviceType> listDeviceData = new List<DeviceType>();
            int rowNumCont = _DataGrid_DeviceSet.Items.Count;
            if (rowNumCont > 0)
            { 
                for (int i = 0; i < rowNumCont; i++)
                {
                    //获取DataGrid 里面DataGridComboBoxColumn列控件值
                    //获取行
                    PublicClass.DeviceType devicetype = new DeviceType();
                    //获取列对象控件值   
                    devicetype.DeviceSet_ComBox_DeviceSet = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, i, "_DataGrid_DeviceSet_ComBox_DeviceSet");
                    devicetype.DeviceSet_ComBox_Protocol = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 2, i, "_DataGrid_DeviceSet_ComBox_Protocol");
                    devicetype.DeviceSet_ComBox_Spd = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 3, i, "_DataGrid_DeviceSet_ComBox_Spd");
                    devicetype.DeviceSet_ComBox_Bit = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 4, i, "_DataGrid_DeviceSet_ComBox_Bit");
                    devicetype.DeviceSet_ComBox_Sync_Bit = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 5, i, "_DataGrid_DeviceSet_ComBox_Sync_Bit");
                    devicetype.DeviceSet_ComBox_Stop_Bit = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 6, i, "_DataGrid_DeviceSet_ComBox_Stop_Bit");
                    devicetype.DeviceSet_RadioBut_Is_Main = Convert.ToBoolean(GetDataTemplateData("RadioButton", _DataGrid_DeviceSet, 7, i, "_DataGrid_DeviceSet_RadioBut_Is_Main"));

                    if (GetDataTemplateData("TextBox", _DataGrid_DeviceSet, 8, i, "_DropDevice_siteTxtBox").Length > 0)
                    { devicetype.DeviceSet_TextBox_Site = devicetype.DeviceSet_TextBox_Site = GetDataTemplateData("TextBox", _DataGrid_DeviceSet, 8, i, "_DropDevice_siteTxtBox"); }
                    else { devicetype.DeviceSet_TextBox_Site = "null"; }

                    if (GetDataTemplateData("TextBox", _DataGrid_DeviceSet, 9, i, "_DropDevice_stopTxtBox").Length > 0)
                    { devicetype.DropDevice_TextBox_stop = devicetype.DropDevice_TextBox_stop = GetDataTemplateData("TextBox", _DataGrid_DeviceSet, 9, i, "_DropDevice_stopTxtBox"); }
                    else { devicetype.DropDevice_TextBox_stop = "null"; }

                    listDeviceData.Add(devicetype);
                } 
            }
            return listDeviceData;
        }
        
        //删除端口 
        private void _DropDevice_but_Click(object sender, RoutedEventArgs e)
        { 
            DeviceType devicetype = DeviceTypeInfo();
            String comBoxText = devicetype.DeviceSet_ComBox_DeviceSet;

            if (comBoxText.Equals("")|| comBoxText.Equals(null))
            {
                comBoxText = (_DataGrid_DeviceSet.SelectedIndex+1).ToString();
            }
            //删除端口
            if (dt.Rows.Count > 0 && _DataGrid_DeviceSet.SelectedIndex > 1)
            {
                if (MessageBox.Show("是否删除" + comBoxText + "端口？", "删除提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    if (_DataGrid_DeviceSet.SelectedIndex != -1 && _DataGrid_DeviceSet.SelectedIndex != 0 && _DataGrid_DeviceSet.SelectedIndex != dt.Rows.Count)
                    {
                        dt.Rows.RemoveAt(_DataGrid_DeviceSet.SelectedIndex);
                    }
                    else
                    {
                        dt.Rows.RemoveAt(dt.Rows.Count - 1);
                    }
                }
            }
            else
            {
                MessageBox.Show("默认端口不能删除！", "提示");
            }
        }
         
        private void menu_Task_Click(object sender, RoutedEventArgs e)
        {
               //_Grid_DeviceConnect.Visibility = Visibility.Visible;
               _Grid_DeviceSet.Visibility = Visibility.Visible;
               _Grid_TaskSet.Visibility = Visibility.Visible;
        }
        
        private void menu_System_Log_Click(object sender, RoutedEventArgs e)
        {
            //_Grid_DeviceConnect.Visibility = Visibility.Hidden;
            //_Grid_DeviceSet.Visibility = Visibility.Hidden;
            //_Grid_TaskSet.Visibility = Visibility.Hidden;
            //MessageBox.Show(Convert.ToString(0));
        }

        //纵向滚动条鼠标事件
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent; eventArg.Source = sender;
            scrollViewer.RaiseEvent(eventArg);
        }

        //获取DATAGRID中模板列控件对象
        //FindName(datagrid名称，列，行，控件名)
        public object FindName(System.Windows.Controls.DataGrid myDataGrid, int columnIndex, int rowIndex, string controlName)
        {
            object itemobj = new object();
            try
            {
                //FrameworkElement item1 = _DataGrid_DeviceSet.Columns[1].GetCellContent(_DataGrid_DeviceSet.Items[0]);
                //FrameworkElement item2 = _DataGrid_TaskSet.Columns[1].GetCellContent(_DataGrid_TaskSet.Items[0]);

                FrameworkElement item = myDataGrid.Columns[columnIndex].GetCellContent(myDataGrid.Items[rowIndex]);
                DataGridTemplateColumn temp = (myDataGrid.Columns[columnIndex] as DataGridTemplateColumn);
                itemobj = temp.CellTemplate.FindName(controlName, item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return itemobj; 
        }
        //端口记录元件文本框输入转为大写
        private void _DropDevice_siteTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
                TextBox txtbox = (TextBox)sender;
                txtbox.Text = txtbox.Text.ToUpper();
                txtbox.SelectionStart = txtbox.Text.Length;
        }
        //输入转为大写
        private void _DropDevice_stopTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
                TextBox txtbox = (TextBox)sender;
                txtbox.Text = txtbox.Text.ToUpper();
                txtbox.SelectionStart = txtbox.Text.Length;
        }

        //对象背景色方法
        void _SetBackgroundColor(object sender, String b_color)
        {

            DataGridRow _object = (DataGridRow)sender;
            Color backgroundColor = ((Color)ColorConverter.ConvertFromString(b_color));
            _object.Background = new SolidColorBrush(backgroundColor);

        }

       
        //手动增加任务行
        private void _Add_Task_But_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = TaskTable.NewRow();
            TaskTable.Rows.Add(dr);

            _DataGrid_TaskSet.ItemsSource = TaskTable.DefaultView;
        }
        //窗体加载事件
        private void Dea_Maim_Window_Loaded(object sender, RoutedEventArgs e)
        {
             
           // MessageBox.Show(SystemParameters.PrimaryScreenHeight.ToString());
                
        } 
        //获取任务选中列表数据集
        private TaskType taskTypeInfo()
            {   //获取列对象控件值  
                PublicClass.TaskType tasktype = new TaskType();
                int rowNumCont = _DataGrid_TaskSet.Items.Count;
                if (rowNumCont > 0)
                {
                    //获取DataGrid 里面DataGridComboBoxColumn列控件值
                    //获取行
                    int taskRowNum = _DataGrid_TaskSet.SelectedIndex;
                      
                    tasktype.TaskSet_ComBox_TaskNum =   GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, taskRowNum, "_DataGrid_TaskSet_ComBox_TaskNum");
                    tasktype.TaskSet_ComBox_DeviceSet =  GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, taskRowNum, "_DataGrid_TaskSet_ComBox_DeviceSet");
                    tasktype.TaskSet_Label_Is_Main =  GetDataTemplateData("Label", _DataGrid_TaskSet, 3, taskRowNum, "_DataGrid_TaskSet_Label_Is_Main");
                    tasktype.TaskSet_ComBox_ReadWrite =   GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, taskRowNum, "_DataGrid_TaskSet_ComBox_ReadWrite");
                    tasktype.TaskSet_ComBox_DeviceNum =  GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 5, taskRowNum, "_DataGrid_TaskSet_ComBox_DeviceNum");
                    tasktype.TaskSet_textBox_read_address =  GetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, taskRowNum, "_DataGrid_TaskSet_textBox_readWriteAdd");
                    tasktype.TaskSet_textBox_read_len = GetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, taskRowNum, "_DataGrid_TaskSet_textBox_readLen");
                    tasktype.TaskSet_textBox_data_len = GetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, taskRowNum, "_DataGrid_TaskSet_textBox_dataLen");
                    tasktype.TaskSet_textBox_write_address =  GetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, taskRowNum, "_DataGrid_TaskSet_textBox_writeAdd");
                    tasktype.TaskSet_textBox_data_index =  GetDataTemplateData("TextBox", _DataGrid_TaskSet, 10, taskRowNum, "_DataGrid_TaskSet_textBox_dataIndex");
                 }
                return tasktype;
            }

        //获取任务表所有的数据集
        private List<TaskType> taskTypeAllData()
        {
            List<TaskType> listDeviceData = new List<TaskType>();
            int rowNumCont = _DataGrid_TaskSet.Items.Count;
            if (rowNumCont > 0)
            {
                for (int i = 0; i < _DataGrid_TaskSet.Items.Count; i++)
                {
                    //获取DataGrid 里面DataGridComboBoxColumn列控件值
                    //获取行
                    DataGridRow task_dgrow = (DataGridRow)_DataGrid_TaskSet.ItemContainerGenerator.ContainerFromIndex(i);
                    PublicClass.TaskType tasktype = new TaskType();
                    //获取列对象控件值   
                    tasktype.TaskSet_ComBox_TaskNum = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, i, "_DataGrid_TaskSet_ComBox_TaskNum");
                    tasktype.TaskSet_ComBox_DeviceSet = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, i, "_DataGrid_TaskSet_ComBox_DeviceSet");
                    tasktype.TaskSet_Label_Is_Main = GetDataTemplateData("Label", _DataGrid_TaskSet, 3, i, "_DataGrid_TaskSet_Label_Is_Main");
                    tasktype.TaskSet_ComBox_ReadWrite = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, i, "_DataGrid_TaskSet_ComBox_ReadWrite");
                    tasktype.TaskSet_ComBox_DeviceNum = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 5, i, "_DataGrid_TaskSet_ComBox_DeviceNum");
                     
                    if (GetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, i, "_DataGrid_TaskSet_textBox_readWriteAdd").Length > 0)
                    { tasktype.TaskSet_textBox_read_address = GetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, i, "_DataGrid_TaskSet_textBox_readWriteAdd"); }
                    else { tasktype.TaskSet_textBox_read_address = "null"; }

                    if (GetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, i, "_DataGrid_TaskSet_textBox_readLen").Length > 0)
                    { tasktype.TaskSet_textBox_read_len = GetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, i, "_DataGrid_TaskSet_textBox_readLen"); }
                    else { tasktype.TaskSet_textBox_read_len = "null"; }

                    if (GetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, i, "_DataGrid_TaskSet_textBox_dataLen").Length > 0)
                    { tasktype.TaskSet_textBox_data_len = GetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, i, "_DataGrid_TaskSet_textBox_dataLen"); }
                    else { tasktype.TaskSet_textBox_data_len = "null"; }

                    if (GetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, i, "_DataGrid_TaskSet_textBox_writeAdd").Length > 0)
                    { tasktype.TaskSet_textBox_write_address = GetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, i, "_DataGrid_TaskSet_textBox_writeAdd"); }
                    else { tasktype.TaskSet_textBox_write_address = "null"; }

                    if (GetDataTemplateData("TextBox", _DataGrid_TaskSet, 10, i, "_DataGrid_TaskSet_textBox_dataIndex").Length > 0)
                    { tasktype.TaskSet_textBox_data_index = GetDataTemplateData("TextBox", _DataGrid_TaskSet, 10, i, "_DataGrid_TaskSet_textBox_dataIndex"); }
                    else { tasktype.TaskSet_textBox_data_index = "null"; }

                    listDeviceData.Add(tasktype);
                }
            }
            return listDeviceData;
        }

        //手动删除任务
        private void _DropTask_but_Click(object sender, RoutedEventArgs e)
        {
            TaskType tasktype = taskTypeInfo(); 
            //删除任务 
            if (TaskTable.Rows.Count > 0 )
            {
                if (MessageBox.Show("是否删除如下任务：" + "\n任务号：" + tasktype.TaskSet_ComBox_TaskNum +
                                                               "\n端口：" + tasktype.TaskSet_ComBox_DeviceSet +
                                                               "\n站号：" + tasktype.TaskSet_ComBox_DeviceNum +
                                                              "\n读/写：" + tasktype.TaskSet_ComBox_ReadWrite, "删除提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    if (_DataGrid_TaskSet.SelectedIndex != -1)
                    {
                        TaskTable.Rows.RemoveAt(_DataGrid_TaskSet.SelectedIndex);
                    }
                    else
                    {
                        return;
                    }
                } 
            }  
        }
        //任务DATAGRID加载事件
        private void _DataGrid_TaskSet_Loaded(object sender, RoutedEventArgs e)
        {
            ////端口下拉
            //List<String> comSetData = new List<string>();
            //for (int i = 1; i < 21; i++)
            //{
            //    comSetData.Add("COM" + i);
            //}
            //_DataGrid_TaskSet_ComBox_DeviceSet.ItemsSource = comSetData;
            ////任务号下拉
            //List<String> taskNum = new List<string>();
            //for (int i = 1; i < 21; i++)
            //{
            //    taskNum.Add("任务"+i);
            //}
            //_DataGrid_TaskSet_ComBox_TaskNum.ItemsSource = taskNum;

            ////读写下拉
            //List<String> readWrite = new List<string>();
            //readWrite.Add("读");
            //readWrite.Add("写");
            //_DataGrid_TaskSet_ComBox_ReadWrite.ItemsSource = readWrite;
            ////站号下拉
            //List<String> deviceNum = new List<string>();
            //for (int i = 1; i < 21; i++)
            //{
            //    deviceNum.Add(  i + "号站");
            //}

            //_DataGrid_TaskSet_ComBox_DeviceNum.ItemsSource = deviceNum;
            



        }
        //测试按钮2
        private void test_But1_Click(object sender, RoutedEventArgs e)
        {
            //测试List查询功能。
            //  UseFinds();
            //测试查找DataGrid模板列控件
            //RadioButton readbut = (RadioButton)FindName(_DataGrid_DeviceSet,7,0, "_DropDevice_radioBut");

            //////获得端口集合所有数据
            //List<DeviceType> listDeviceData = new List<DeviceType>();
            //listDeviceData = DeviceTypeAllData();
            //for (int i = 0; i < listDeviceData.Count; i++)
            //{
            //    PublicClass.DeviceType devicetype = new DeviceType();
            //    devicetype = listDeviceData[i];
            //    // MessageBox.Show(devicetype.DeviceSet_ComBox_DeviceSet);
            //}
            ////获得LIST给定条件查询返回的结果集
            //IEnumerable<DeviceType> resultList = new List<DeviceType>();
            //resultList = DeviceListFind(listDeviceData, "COM1");
            //foreach (DeviceType dev in resultList)
            //{
            //    //MessageBox.Show(dev.DeviceSet_ComBox_Protocol);
            //}

            ////获得任务集合所有数据
            //List<TaskType> listTaskData = new List<TaskType>();
            //listTaskData = taskTypeAllData();
            ////for (int i = 0; i < listTaskData.Count; i++)
            ////{
            ////    PublicClass.TaskType devicetype = new TaskType();
            ////    devicetype = listTaskData[i];
            ////    // MessageBox.Show(devicetype.DeviceSet_ComBox_DeviceSet);
            ////}
            ////获得LIST给定条件查询返回的结果集
            //IEnumerable<TaskType> _taskrRsultList = new List<TaskType>();
            //_taskrRsultList = TaskListFind(listTaskData, "任务1","读");
            //foreach (TaskType dev in _taskrRsultList)
            //{
            //    MessageBox.Show(dev.TaskSet_ComBox_DeviceSet);
            //}





            ////创建一个保存文件式的对话框  
            //SaveFileDialog saveFile = new SaveFileDialog();
            ////设置这个对话框的起始保存路径  
            //// saveFile.InitialDirectory = Application;
            ////设置保存的文件的类型，注意过滤器的语法  
            //saveFile.Filter = "TXT File(*.json)|*.json";


            //try
            //{

            //        var result = saveFile.ShowDialog();
            //        if (result == true)
            //        {
            //            FileStream savefs = new FileStream(saveFile.FileName, FileMode.Create);
            //            StreamWriter savesw = new StreamWriter(savefs);
            //            savesw.Write(jsonStr);
            //            savesw.Close();
            //            MessageBox.Show("保存成功！", "提示");
            //        }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "错误");
            //}
            DataRow t_dr = TaskTable.NewRow();
            TaskTable.Rows.Add(t_dr);
            _DataGrid_TaskSet.ItemsSource = TaskTable.DefaultView;
             
            //SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, 0, "_DataGrid_TaskSet_ComBox_TaskNum", "任务1");
            //SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, 0, "_DataGrid_TaskSet_ComBox_DeviceSet", "COM1");
            //SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, 0, "_DataGrid_TaskSet_ComBox_DeviceSet", "COM1");
            //((ComboBox)FindName(_DataGrid_TaskSet, 2, 0, "_DataGrid_TaskSet_ComBox_DeviceSet")).Text = "COM2";
        }

        //调试按钮
        private void test_But_Click(object sender, RoutedEventArgs e)
        {

            FrameworkElement item1 = _DataGrid_DeviceSet.Columns[1].GetCellContent(_DataGrid_DeviceSet.Items[0]);
            FrameworkElement item2 = _DataGrid_TaskSet.Columns[1].GetCellContent(_DataGrid_TaskSet.Items[0]);

            SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, 0, "_DataGrid_TaskSet_ComBox_TaskNum", "任务1");

        }

        //=====================================
        //DeviceType类COM端口名称List查询方法
        static IEnumerable<DeviceType> DeviceListFind(List<DeviceType> list, string _findStr)
        {
            var resultList = from DeviceType device in list
                             where device.DeviceSet_ComBox_DeviceSet.Equals(_findStr) 
                             select device;
            return (IEnumerable<DeviceType>)resultList;
        }
        //TaskType类任务名称List查询方法
        static IEnumerable<TaskType> TaskListFind(List<TaskType> list, string _taskNumFindStr,string _tskReadWriteStr)
        {
            var resultList = from TaskType device in list
                             where device.TaskSet_ComBox_TaskNum.Equals(_taskNumFindStr) && device.TaskSet_ComBox_ReadWrite.Equals(_tskReadWriteStr)
                             select device;
            return (IEnumerable<TaskType>)resultList;
        }
  
        //读写换件值变更设置
        private void _DataGrid_TaskSet_ComBox_ReadWrite_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
          
        }

        private void _DataGrid_TaskSet_textBox_readWriteAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            txtbox.Text = txtbox.Text.ToUpper();
            txtbox.SelectionStart = txtbox.Text.Length;
        }

        private void _DataGrid_TaskSet_textBox_readLen_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.ToUpper();
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void _DataGrid_TaskSet_textBox_writeAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            txtbox.Text = txtbox.Text.ToUpper();
            txtbox.SelectionStart = txtbox.Text.Length;
        }
        //读元件控件设置
        private void _DataGrid_TaskSet_textBox_readWriteAdd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //获取DataGrid 里面DataGridComboBoxColumn列控件值 
            //端口选中行
            int deviceRowNum = _DataGrid_TaskSet.SelectedIndex;
            string isReadWrite = "";

            if (CheckTaskRowInterity(deviceRowNum))
            {
                isReadWrite = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, deviceRowNum, "_DataGrid_TaskSet_ComBox_ReadWrite");
                if (isReadWrite == "读")
                {
                    e.Handled = false;
                }
                else
                {
                    MessageBox.Show("读站才能设置读元件!", "提示");
                    
                    e.Handled = true;
                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, deviceRowNum, "_DataGrid_TaskSet_textBox_readWriteAdd", null);
                }
            }
            else
            {
                MessageBox.Show("（*）属性必填！", "提示"); 
                e.Handled = true;
                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, deviceRowNum, "_DataGrid_TaskSet_textBox_readWriteAdd", null);
            }

        }
        //读取长度控件设置
        private void _DataGrid_TaskSet_textBox_readLen_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int deviceRowNum = _DataGrid_TaskSet.SelectedIndex;
            string isReadWrite = ""; 
            //获取DataGrid 里面DataGridComboBoxColumn列控件值 
            //端口选中行

            if (CheckTaskRowInterity(deviceRowNum))
            {
                isReadWrite = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, deviceRowNum, "_DataGrid_TaskSet_ComBox_ReadWrite");
                if (isReadWrite == "读")
                {
                    e.Handled = input_num(sender, e);
                }
                else
                {
                    MessageBox.Show("读站才能设置读元件!", "提示");
                    e.Handled = true;
                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, deviceRowNum, "_DataGrid_TaskSet_textBox_readLen", null);
                }
            }
            else
            {
                MessageBox.Show("（*）属性必填！", "提示");
                e.Handled = true;
                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, deviceRowNum, "_DataGrid_TaskSet_textBox_readLen", null);
            }
        }
        //数据长度控件设置
        private void _DataGrid_TaskSet_textBox_dataLen_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            txtbox.Text = txtbox.Text.ToUpper();
            txtbox.SelectionStart = txtbox.Text.Length;
        }
        //数据长度控件设置
        private void _DataGrid_TaskSet_textBox_dataLen_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int deviceRowNum = _DataGrid_TaskSet.SelectedIndex;
            string isReadWrite = "";
            //获取DataGrid 里面DataGridComboBoxColumn列控件值 
            //端口选中行

            if (CheckTaskRowInterity(deviceRowNum))
            {
                isReadWrite = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, deviceRowNum, "_DataGrid_TaskSet_ComBox_ReadWrite");
                if (isReadWrite == "读")
                {
                    e.Handled = input_num(sender, e);
                }
                else
                {
                    MessageBox.Show("读站才能设置读元件!", "提示");
                    e.Handled = true;
                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, deviceRowNum, "_DataGrid_TaskSet_textBox_dataLen", null);
                    
                }
            }
            else
            {
                MessageBox.Show("（*）属性必填！", "提示");
                e.Handled = true;
                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, deviceRowNum, "_DataGrid_TaskSet_textBox_dataLen", null);
               
            }
        }
        //写元件控件设置
        private void _DataGrid_TaskSet_textBox_writeAdd_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //获取DataGrid 里面DataGridComboBoxColumn列控件值 
            //端口选中行
            int deviceRowNum = _DataGrid_TaskSet.SelectedIndex;
            string isReadWrite = "";

            if (CheckTaskRowInterity(deviceRowNum))
            {
                isReadWrite = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, deviceRowNum, "_DataGrid_TaskSet_ComBox_ReadWrite");
                if (isReadWrite == "写")
                {
                    e.Handled = false;
                }
                else
                {
                    MessageBox.Show("写站才能设置写元件!", "提示");
                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, deviceRowNum, "_DataGrid_TaskSet_textBox_writeAdd", null);
                    e.Handled = true;
                }
            }
            else
            {
                MessageBox.Show("（*）属性必填！", "提示");
                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, deviceRowNum, "_DataGrid_TaskSet_textBox_writeAdd", null);
                e.Handled = true;
            }
        }
        
        //生成json文件
        private string classtojson()
        {
            ////类转json测试
            List<DeviceType> listDeviceData = new List<DeviceType>();
            listDeviceData = DeviceTypeAllData();
            //获得任务集合所有数据
            List<TaskType> listTaskData = new List<TaskType>();
            listTaskData = taskTypeAllData();
            //定义任务数据集筛选对象
            IEnumerable<TaskType> resultTaskList = new List<TaskType>();

            //定义泛型任务类型
            //List<JsonType> jsonlist = new List<JsonType>();
            JsonType jsonlist = new JsonType();

            //生成端口JSON数据
            int devicecont = listDeviceData.Count;
            JsonType jsontype = new JsonType();

            jsontype.serial = new List<Serial>();
            jsontype.task = new List<JTask>();
            jsontype.site = new List<Site>();

            DeviceType devicetype = new DeviceType();

            for (int i = 0; i < devicecont; i++)
            {
                Serial serial = new Serial();
                serial.attribute = new int[4];
                //端口数据
                //MessageBox.Show(listDeviceData[i].DeviceSet_ComBox_Sync_Bit.Substring(2, 1));
                serial.port = Convert.ToInt32(listDeviceData[i].DeviceSet_ComBox_DeviceSet.Substring(3, 1));
                serial.protocol = listDeviceData[i].DeviceSet_ComBox_Protocol; 
                serial.attribute[0] = Convert.ToInt32(listDeviceData[i].DeviceSet_ComBox_Spd);
                serial.attribute[1] = Convert.ToInt32(listDeviceData[i].DeviceSet_ComBox_Bit);
                serial.attribute[2] = Convert.ToInt32(listDeviceData[i].DeviceSet_ComBox_Sync_Bit.Substring(2, 1));
                serial.attribute[3] = Convert.ToInt32(listDeviceData[i].DeviceSet_ComBox_Stop_Bit);
                serial.main = listDeviceData[i].DeviceSet_RadioBut_Is_Main;
                serial.fault = listDeviceData[i].DeviceSet_TextBox_Site;
                serial.stop = listDeviceData[i].DropDevice_TextBox_stop;
                jsontype.serial.Add(serial);
            }

            //生成任务json数据
            List<JTask> jtask = new List<JTask>();
            List<Read> read = new List<Read>();

            var taskG = from ps in listTaskData
                        group ps by ps.TaskSet_ComBox_TaskNum
                     into g
                        select new { name = g.Key, count = g.Count() };
            //任务遍历
            for (int i = 0; i < taskG.Count(); i++)
            {
                JTask t_task = new JTask();
                t_task.read = new List<Read>();
                t_task.write = new List<Write>();

                //获得任务LIST给定条件(任务号)查询返回的“读”结果集
                resultTaskList = TaskListFind(listTaskData, taskG.ToList()[i].name, "读");
                string str = null;
                string d_tasknum = null;
                string d_device = null;

                foreach (TaskType dev in resultTaskList)
                {
                    Read t_read = new Read();
                    d_tasknum = dev.TaskSet_ComBox_TaskNum;
                    d_device = dev.TaskSet_ComBox_DeviceSet;
                    str = dev.TaskSet_ComBox_DeviceNum;

                    t_read.task_number = Convert.ToInt32(d_tasknum.Substring(2, d_tasknum.Length - 2));
                    t_read.port = Convert.ToInt32(d_device.Substring(3, d_device.Length - 3));
                    if (dev.TaskSet_Label_Is_Main != "0" || dev.TaskSet_Label_Is_Main != null)
                    {
                        t_read.is_main = "\u662f";
                    }
                    else
                    {
                        t_read.is_main = dev.TaskSet_Label_Is_Main;
                    }
                    t_read.site_number = Convert.ToInt32(str.Substring(0, str.Length - 2));
                    t_read.address = dev.TaskSet_textBox_read_address;
                    t_read.read_length = Convert.ToInt32(dev.TaskSet_textBox_read_len);
                    t_read.data_length = Convert.ToInt32(dev.TaskSet_textBox_data_len);
                    t_read.data_index = strToIntArr(dev.TaskSet_textBox_data_index);
                    t_task.read.Add(t_read);
                }
                //获得任务LIST给定条件(任务号)查询返回的“写”结果集
                resultTaskList = TaskListFind(listTaskData, taskG.ToList()[i].name, "写");

                foreach (TaskType dev in resultTaskList)
                {
                    Write t_write = new Write();
                    d_tasknum = dev.TaskSet_ComBox_TaskNum;
                    d_device = dev.TaskSet_ComBox_DeviceSet;
                    str = dev.TaskSet_ComBox_DeviceNum;



                    t_write.task_number = Convert.ToInt32(d_tasknum.Substring(2, d_tasknum.Length - 2));
                    t_write.port = Convert.ToInt32(d_device.Substring(3, d_device.Length - 3));
                    if (dev.TaskSet_Label_Is_Main!="0" || dev.TaskSet_Label_Is_Main != null)
                    {
                        t_write.is_main = "\u662f";
                    }
                    else
                    {
                        t_write.is_main = dev.TaskSet_Label_Is_Main;
                    } 
                    t_write.site_number = Convert.ToInt32(str.Substring(0, str.Length - 2));
                    t_write.address = dev.TaskSet_textBox_read_address;
                    t_write.read_length = Convert.ToInt32(dev.TaskSet_textBox_read_len);
                    t_write.data_length = Convert.ToInt32(dev.TaskSet_textBox_data_len);
                    t_write.data_index = strToIntArr(dev.TaskSet_textBox_data_index);
                    t_task.write.Add(t_write);
                }
                jsontype.task.Add(t_task);
            }
            //生成site对象数据
            for (int i = 0; i < listDeviceData.Count; i++)
            {
                Site t_site = new Site();
                devicetype = listDeviceData[i];
                if (!devicetype.DeviceSet_RadioBut_Is_Main)
                {
                    t_site.port = Convert.ToInt32(devicetype.DeviceSet_ComBox_DeviceSet.Substring(3, 1));
                    t_site.fault = devicetype.DeviceSet_TextBox_Site;
                    t_site.stop = devicetype.DropDevice_TextBox_stop;
                    jsontype.site.Add(t_site);
                } 
            }
            if (!taskDelay.Text.Equals(""))
            {
                jsontype.delay = Convert.ToInt32(taskDelay.Text); 
            }
            else
            {
                jsontype.delay = 5;
            }

            if (!taskDelay.Text.Equals(""))
            {
                jsontype.version=appVersion;
            }
            else
            {
                jsontype.version = "10000";
            }

            //所有json类型加入集合
            //jsonlist.Add(jsontype);
            jsonlist = jsontype;

            System.Text.StringBuilder jsonBuilder = new System.Text.StringBuilder();
            String jsonStr;
            JavaScriptSerializer js = new JavaScriptSerializer();
            jsonStr = js.Serialize(jsonlist);
            return jsonStr;
            //格式化json字串


        }
        


        private void _ToFile_Task_But_Click(object sender, RoutedEventArgs e)
        {
            //创建一个保存文件式的对话框  
            SaveFileDialog saveFile = new SaveFileDialog();
            //设置这个对话框的起始保存路径  
           // saveFile.InitialDirectory = Application;
            //设置保存的文件的类型，注意过滤器的语法  
            

            try
            {
                if (checkTaskInterity())
                {
                    MessageBox.Show("必填项信息不完整！", "提示");
                }
                else
                {
                    saveFile.Filter = "TXT File(*.json)|*.json";
                    string jsonstr = classtojson();
                    var result = saveFile.ShowDialog();
                    if (result == true)
                    {
                        FileStream savefs = new FileStream(saveFile.FileName, FileMode.Create);
                        StreamWriter savesw = new StreamWriter(savefs);
                        savesw.Write(jsonstr);
                        savesw.Close();
                        MessageBox.Show("保存成功！", "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
            
           
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Message"></param>
        public static void WriteNewFile(string FileName, string Message)
        {
            //bool toFile = false;
            if (File.Exists(FileName))
            {
                //MessageBox.Show(FileName+"存在！");
                File.Delete(FileName);
            }
            //MessageBox.Show(FileName + "不存在！");
            using (FileStream fileStream = File.Create(FileName))
            {
                byte[] bytes = new UTF8Encoding(true).GetBytes(Message);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        ////获得任务条件查询，返回按任务名分组结果
        public IEnumerable taskGroupData()
        {
            ////获得端口集合所有数据////获得任务条件查询，返回按任务名分组结果
            List<TaskType> listTaskData = new List<TaskType>();
            listTaskData = taskTypeAllData();

            var result = from ps in listTaskData
                  group ps by ps.TaskSet_ComBox_TaskNum
                     into g
                  select new { name = g.Key, count = g.Count() }; 
            
            return result;
        }
         
        //延时框输入过滤
        private void _taskDelay_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = input_num(sender,e);
        }

        private void _UpLoad_Task_But_Click(object sender, RoutedEventArgs e)
        { 
            if (checkTaskInterity())
            {
                MessageBox.Show("必填项信息不完整！", "提示");
            }
            else
            {
                if (checkLocalIP() || !online)
                {
                    try
                    {
                        string jsonstr = classtojson();
                        //json文件保存到本地
                        WriteNewFile(appPath + "dea", jsonstr);
                        //MessageBox.Show(appPath + "dea.json");
                        //MessageBox.Show("dea文件是否存在本地："+File.Exists(appPath + "dea.json").ToString());
                        //json文件上传到适配器
                        //FTPUpload(ftpServerIP, ftpUserID, ftpPassword, ftpRemotePath, appPath, ".json");
                        Upload(ftpServerIP, ftpUserID, ftpPassword, ftpRemotePath, appPath, filename);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("联机失败，请检查网络设置！", "提示");
                }
                
            }
            
           
        }
        
        public void Upload(string host, string strUser, string strPassword, string Savepath, string localpath, string filename)
        { 
            try
            {
                // 上传文件
                client.Host = host;
                client.Credentials = new NetworkCredential(strUser, strPassword); 
                client.Port = 21;
                //client.ConnectTimeout = 60000;
                client.Connect(); 
                //判断存在就先下载
                if (client.FileExists(filename))
                { 
                    client.DownloadFile(localpath + "\\log\\" + "dea" + DateTime.Now.ToString("yyyymmddhhmiss", DateTimeFormatInfo.InvariantInfo), "/" + filename,true);
                }
                //上传配置
                if (client.UploadFile(localpath+"dea", "/" + filename, true))
                {
                    MessageBox.Show("上传适配器成功！", "提示");
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            } 
          }
        
        ///// <summary>
        ///// FTP上传文件
        ///// </summary>
        ///// <param name="strServer">服务器地址</param>
        ///// <param name="strUser">用户名</param>
        ///// <param name="strPassword">密码</param>
        ///// <param name="Savepath">服务器用于保存的文件夹路径，不是服务器根路径,例如： "/UploadDocumentsSave/"</param>
        ///// <param name="localpath">本地路径</param>
        ///// <param name="filetype">文件类型，例如: ".rte"</param>
        //public void FTPUpload(string strServer, string strUser, string strPassword, string Savepath, string localpath, string filetype)
        //{
        //    FtpClient ftp = new FtpClient();
        //    ftp.Host = strServer;
        //    ftp.Credentials = new NetworkCredential(strUser, strPassword);
        //    ftp.Port = 21;
        //    ftp.Connect();

        //    string[] files = Directory.GetFiles(localpath, "*" + filetype);
        //    if (files.Length != 0)
        //    {
        //        foreach (string file in files)
        //        {
        //            using (var fileStream = File.OpenRead(file))
        //            using (var ftpStream = ftp.OpenWrite(Savepath + System.IO.Path.GetFileName(file)))
        //            {
        //                var buffer = new byte[8 * 1024];
        //                int count;
        //                while ((count = fileStream.Read(buffer, 0, buffer.Length)) > 0)
        //                {
        //                    ftpStream.Write(buffer, 0, count);
        //                }
        //            }
        //        }
        //        MessageBox.Show("上传成功");
        //    }
        //}

        private bool input_num(object sender, KeyEventArgs e)
        {
            //屏蔽非法按键
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal || e.Key == Key.Back)
            {
                if (e.Key == Key.Decimal || e.Key == Key.Clear )
                {
                    e.Handled = true;
                    return true;
                }
                e.Handled = false;
                return false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return true;
                }
                e.Handled = false;
                return false;
            }
            else
            {
                e.Handled = true;
                return true;
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
                    if (ip.ToString().Contains(ftpServerIP.Substring(1,10)))
                    {
                        ipok = true;
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            return ipok;
        }

        //判断所有任务信息输入完整性
        private bool checkTaskInterity()
        { 
            bool null_info = false;

            List<TaskType> listTaskData = new List<TaskType>();
            listTaskData = taskTypeAllData();
            List<DeviceType> listDeviceData = new List<DeviceType>();
            listDeviceData = DeviceTypeAllData();

            if (listTaskData.Count <= 0)
            {
                null_info = true;
            }
            else
            {
                for (int i = 0; i < listTaskData.Count; i++)
                { 
                    PublicClass.TaskType tasktype = new TaskType();
                    tasktype = listTaskData[i];

                    if (tasktype.TaskSet_ComBox_TaskNum == "0" |
                   tasktype.TaskSet_ComBox_DeviceSet == "0" |
                   tasktype.TaskSet_ComBox_ReadWrite == "0" |
                   tasktype.TaskSet_ComBox_DeviceNum == "0")
                    {
                        null_info = true;
                    }
                }
                for (int i = 0; i < listDeviceData.Count; i++)
                {
                    PublicClass.DeviceType devicetype = new DeviceType();
                    devicetype = listDeviceData[i];

                    if (devicetype.DeviceSet_ComBox_DeviceSet == "0" |
                   devicetype.DeviceSet_ComBox_Protocol == "0" |
                   devicetype.DeviceSet_ComBox_Spd == "0" |
                   devicetype.DeviceSet_ComBox_Bit == "0" |
                   devicetype.DeviceSet_ComBox_Sync_Bit == "0" |
                   devicetype.DeviceSet_ComBox_Stop_Bit == "0")
                    {
                        null_info = true;
                    }
                }
            } 
            return null_info;
        }

        private void menu_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void _DataGrid_DeviceSet_RadioBut_Is_Main_Click(object sender, RoutedEventArgs e)
        {

        }
            
        private void menu_account_Click(object sender, RoutedEventArgs e)
        {
         
        }
        
        //private void _DataGrid_TaskSet_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        //{
        //    if (e.AddedCells.Count == 0)
        //        return;
        //    var currentCell = e.AddedCells[0];
        //    if (currentCell.Column == _DataGrid_TaskSet.Columns[1] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[2] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[3] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[4] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[5] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[6])   //Columns[]从0开始  我这的ComboBox在第四列  所以为3  
        //    {
        //        _DataGrid_TaskSet.BeginEdit();    //  进入编辑模式  这样单击一次就可以选择ComboBox里面的值了  
        //    }
        //}
        
        //private void _DataGrid_DeviceSet_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        //{
        //    if (e.AddedCells.Count == 0)
        //        return;
        //    var currentCell = e.AddedCells[0];
        //    if (currentCell.Column == _DataGrid_TaskSet.Columns[1] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[2] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[3] ||
        //        currentCell.Column == _DataGrid_TaskSet.Columns[4])
        //    {
        //        _DataGrid_TaskSet.BeginEdit();    //  进入编辑模式  这样单击一次就可以选择ComboBox里面的值了  
        //    }
        //}

        private void _DataGrid_DeviceSet_RadioBut_Is_Main_Checked(object sender, RoutedEventArgs e)
        {
            //获取DataGrid 里面DataGridComboBoxColumn列控件值
            //获取行
            int deviceRowNum = _DataGrid_DeviceSet.SelectedIndex;
            if (deviceRowNum>=0)
            {
                if (GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, deviceRowNum, "_DataGrid_DeviceSet_ComBox_DeviceSet").Length > 0)
                {
                    if ((bool)((RadioButton)sender).IsChecked)
                    {
                        SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 8, deviceRowNum, "_DropDevice_siteTxtBox", null);
                        SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 9, deviceRowNum, "_DropDevice_stopTxtBox", null);
                        isMainDevice = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, deviceRowNum, "_DataGrid_DeviceSet_ComBox_DeviceSet");
                    }
                }
                else
                {
                    ((RadioButton)sender).IsChecked = false;
                    MessageBox.Show("请先设置端口！", "提示");
                }
            } 
        } 

        private void _DropDevice_siteTxtBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //获取DataGrid 里面DataGridComboBoxColumn列控件值 
            //端口选中行
            int deviceRowNum = _DataGrid_DeviceSet.SelectedIndex;

            bool is_main = Convert.ToBoolean(GetDataTemplateData("RadioButton", _DataGrid_DeviceSet, 7, deviceRowNum, "_DataGrid_DeviceSet_RadioBut_Is_Main"));
            
            if (is_main)
            {
                MessageBox.Show("主站没有故障设置!", "提示");
                SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 8, deviceRowNum, "_DropDevice_siteTxtBox", null); 
                e.Handled = true; 
            }
            else
            {
                e.Handled = false;
            }
        }

        private void _DropDevice_stopTxtBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //获取DataGrid 里面DataGridComboBoxColumn列控件值 
            //端口选中行
            int deviceRowNum = _DataGrid_DeviceSet.SelectedIndex;

            bool is_main = Convert.ToBoolean(GetDataTemplateData("RadioButton", _DataGrid_DeviceSet, 7, deviceRowNum, "_DataGrid_DeviceSet_RadioBut_Is_Main"));
            if (is_main)
            {
                MessageBox.Show("主站没有故障设置!", "提示"); 
                SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 9, deviceRowNum, "_DropDevice_stopTxtBox", null);
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            } 
        }
        //组合框下拉关闭时响应
        private void _DataGrid_TaskSet_ComBox_TaskNum_DropDownClosed(object sender, EventArgs e)
        {
            //根据任务号设置背景颜色//单元格结束编辑时设置任务表背景颜色偶数设置为有背景色
            TaskType tasktype = taskTypeInfo();
            string str = tasktype.TaskSet_ComBox_TaskNum;
            DataGridRow dgrow = (DataGridRow)_DataGrid_TaskSet.ItemContainerGenerator.ContainerFromIndex(_DataGrid_TaskSet.SelectedIndex);

            if (!str.Equals("") && !str.Equals(null) && str.Length>2 && Convert.ToInt16((str.Substring(str.IndexOf("务") + 1, str.Length - 2))) % 2 == 0)
            {
                Color backgroundColor = ((Color)ColorConverter.ConvertFromString("#FFF5B7B7"));
                dgrow.Background = new SolidColorBrush(backgroundColor);
            }
            else
            {
                Color backgroundColor = ((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                dgrow.Background = new SolidColorBrush(backgroundColor);
            }
        }

        private void _DataGrid_TaskSet_ComBox_DeviceSet_DropDownClosed(object sender, EventArgs e)
        {
            //获取DataGrid 里面DataGridComboBoxColumn列控件值
            //获取行 
            int deviceRowNum = _DataGrid_TaskSet.SelectedIndex;
            if (((ComboBox)sender).Text.Equals(isMainDevice))
            {
                SetDataTemplateData("Label", _DataGrid_TaskSet, 3, deviceRowNum, "_DataGrid_TaskSet_Label_Is_Main", "是");
            }
            else
            { 
                SetDataTemplateData("Label", _DataGrid_TaskSet, 3, deviceRowNum, "_DataGrid_TaskSet_Label_Is_Main", "");
            }
        }

        private void _DataGrid_DeviceSet_ComBox_DeviceSet_DropDownClosed(object sender, EventArgs e)
        {
            //获取DataGrid 里面DataGridComboBoxColumn列控件值
            //获取行
            int deviceRowNum = _DataGrid_DeviceSet.SelectedIndex;
            if (Convert.ToBoolean(GetDataTemplateData("RadioButton", _DataGrid_DeviceSet, 7, deviceRowNum, "_DataGrid_DeviceSet_RadioBut_Is_Main")))
                
            {
                isMainDevice = GetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, deviceRowNum, "_DataGrid_DeviceSet_ComBox_DeviceSet");
            }
          
        }
        //联机菜单
        private void menu_con_Click(object sender, RoutedEventArgs e)
        {
            ConnectWind conWind = new ConnectWind();
            conWind.Title = "DEA联机";
            conWind.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            conWind.Owner = this;
            try
            {
                conWind.ShowDialog();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            } 
        }
        
        //判断单行任务信息输入完整性 
        private bool CheckTaskRowInterity(int rowIndex)
        {
            bool isOk = false;
            try
            {
                string taskNum = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, rowIndex, "_DataGrid_TaskSet_ComBox_TaskNum");
                string deviceSet = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, rowIndex, "_DataGrid_TaskSet_ComBox_DeviceSet");
                //string Is_Main = GetDataTemplateData("Label", _DataGrid_TaskSet, 3, rowIndex, "_DataGrid_TaskSet_Label_Is_Main");
                string ReadWrite = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, rowIndex, "_DataGrid_TaskSet_ComBox_ReadWrite");
                string DeviceNum = GetDataTemplateData("ComboBox", _DataGrid_TaskSet, 5, rowIndex, "_DataGrid_TaskSet_ComBox_DeviceNum");
            
                if (taskNum.Length > 0 && deviceSet.Length > 0  && ReadWrite.Length > 0 && DeviceNum.Length > 0 )
                {
                    isOk = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"");
            } 
            return isOk;
        }
       //查询模板控件值内容
        private string GetDataTemplateData(string control,DataGrid l_datagrid,int colnum,int rownum,string deviceName)
        {
            string deviceData = null;
            try
            { 
                switch (control)
                {
                    case "ComboBox":
                        {
                            deviceData = (string)((ComboBox)FindName(l_datagrid, colnum, rownum, deviceName)).Text;
                        }break;
                    case "TextBox":
                        {
                            deviceData = (string)((TextBox)FindName(l_datagrid, colnum, rownum, deviceName)).Text;
                        }
                        break;
                    case "Label":
                        {
                            deviceData = (string)((Label)FindName(l_datagrid, colnum, rownum, deviceName)).Content;
                        }
                        break;
                    case "RadioButton":
                        {
                            deviceData = ((RadioButton)FindName(l_datagrid, colnum, rownum, deviceName)).IsChecked.ToString();
                        }
                        break;
                    default:
                        MessageBox.Show("无效控件！","提示");
                        break;
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            } 
            if (deviceData == null || deviceData.Length<=0)
            {
                deviceData = "0";
            } 
            return deviceData;
        }

        //设置模板控件值内容
        private string SetDataTemplateData(string control, DataGrid l_datagrid, int colnum, int rownum, string deviceName,string data)
        {
            string deviceData = "";
            //try
            //{
                switch (control)
                {
                    case "ComboBox":
                        {
                            ((TextBox)FindName(l_datagrid, colnum, rownum, deviceName)).Text = data;
                        }
                        break;
                    case "TextBox":
                        {
                            ((TextBox)FindName(l_datagrid, colnum, rownum, deviceName)).Text = data;
                        }
                        break;
                    case "Label":
                        {
                            ((Label)FindName(l_datagrid, colnum, rownum, deviceName)).Content = data; 
                        }
                        break;
                    case "RadioButton":
                        {
                            ((RadioButton)FindName(l_datagrid, colnum, rownum, deviceName)).IsChecked = Convert.ToBoolean(data);
                        }
                        break;
                    default:
                        MessageBox.Show("无效控件！", "提示");
                        break;
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "");
            //}
            return deviceData;
        }
        //打开本地任务
        private void menu_Open_Task_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "文本文件|*.json";
            string jsonstr = null;

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    if ((_Grid_DeviceSet.Visibility == Visibility.Visible || 
                         _Grid_TaskSet.Visibility == Visibility.Visible) && 
                         MessageBox.Show("覆盖当前设置，是否继续？" , "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    { 
                        TaskTable.Clear();
                        dt.Clear(); 
                         
                        //增加端口模板控件数据
                        string[] lines = File.ReadAllLines(dialog.FileName); 
                        foreach (string line in lines)
                        {
                            jsonstr+=(line + Environment.NewLine);
                        } 
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        JsonType l_jsontype = new JsonType();
                        l_jsontype = js.Deserialize<JsonType>(jsonstr);

                        //更新端口数据 DviceSetDataGrid 
                        int deviceRow = l_jsontype.serial.Count;
                        DataRow dr;

                        //增加端口列表行
                        dataGirdAddRow(_DataGrid_DeviceSet, l_jsontype.serial.Count);
                           
                        //增加任务列表行
                        for (int i = 0; i < l_jsontype.task.Count; i++)
                        { 
                            dataGirdAddRow(_DataGrid_TaskSet, l_jsontype.task[i].read.Count);
                            dataGirdAddRow(_DataGrid_TaskSet, l_jsontype.task[i].write.Count); 
                        }
                     
                        Window nwindow = new Window();
                        nwindow.Width = 10;
                        nwindow.Height = 10;
                        nwindow.Show();
                        nwindow.Close();

                    

                        string synstr = null;
                            string syndata = null;
                            string fault = null;
                            string stop = null;
                            for (int i = 0; i < deviceRow; i++)
                            {
                                synstr = l_jsontype.serial[i].attribute[2].ToString();
                                if (l_jsontype.serial[i].fault == "0") fault = "";
                                else fault = l_jsontype.serial[i].fault;
                                if (l_jsontype.serial[i].stop == "0") stop = "";
                                else stop = l_jsontype.serial[i].stop;
                                switch (synstr)
                                {
                                    case "0":
                                        {
                                            syndata = "N[0]";
                                        }
                                        break;
                                    case "1":
                                        {
                                            syndata = "O[1]";
                                        }
                                        break;
                                    case "2":
                                        {
                                            syndata = "E[2]";
                                        }
                                        break;
                                    default:
                                        MessageBox.Show("打开同步位数据失败！", "提示");
                                        break;
                                }
                                SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, i, "_DataGrid_DeviceSet_ComBox_DeviceSet", "COM" + l_jsontype.serial[i].port);
                                SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 2, i, "_DataGrid_DeviceSet_ComBox_Protocol", l_jsontype.serial[i].protocol);
                                SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 3, i, "_DataGrid_DeviceSet_ComBox_Spd", l_jsontype.serial[i].attribute[0].ToString());
                                SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 4, i, "_DataGrid_DeviceSet_ComBox_Bit", l_jsontype.serial[i].attribute[1].ToString());
                                SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 5, i, "_DataGrid_DeviceSet_ComBox_Sync_Bit", syndata);
                                SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 6, i, "_DataGrid_DeviceSet_ComBox_Stop_Bit", l_jsontype.serial[i].attribute[3].ToString());
                                SetDataTemplateData("RadioButton", _DataGrid_DeviceSet, 7, i, "_DataGrid_DeviceSet_RadioBut_Is_Main", l_jsontype.serial[i].main.ToString());
                                if (!l_jsontype.serial[i].main)
                                { 
                                    SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 8, i, "_DropDevice_siteTxtBox", fault);
                                    SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 9, i, "_DropDevice_stopTxtBox", stop);
                                }
                                else
                                {
                                    isMainDevice = "COM"+l_jsontype.serial[i].port;
                                }
                            }

                            //增加任务模板控件数据    
                            int rownum = 0;
                            string strTaskNum = null;
                            //任务循环
                            for (int i = 0; i < l_jsontype.task.Count; i++)
                            {//读循环
                                for (int j = 0; j < l_jsontype.task[i].read.Count; j++)
                                {
                                    if (l_jsontype.task[i].read[j].task_number == 0)
                                    {
                                        strTaskNum = "任务" + (i + 1);
                                    }
                                    else {
                                        strTaskNum = "任务" + l_jsontype.task[i].read[j].task_number;
                                    }

                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, rownum, "_DataGrid_TaskSet_ComBox_TaskNum", strTaskNum);
                                    //设置背景色
                                    setTaskBackColor(strTaskNum, rownum);
                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, rownum, "_DataGrid_TaskSet_ComBox_DeviceSet", "COM" + l_jsontype.task[i].read[j].port);
                                    if (l_jsontype.task[i].read[j].is_main != null)
                                    {
                                        if (l_jsontype.task[i].read[j].is_main.Equals("\u662f"))
                                        {
                                            SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", l_jsontype.task[i].read[j].is_main);
                                        }
                                    }
                                    else
                                    {
                                        SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", null);
                                    }
                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, rownum, "_DataGrid_TaskSet_ComBox_ReadWrite", "读");
                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 5, rownum, "_DataGrid_TaskSet_ComBox_DeviceNum", l_jsontype.task[i].read[j].site_number + "号站");
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, rownum, "_DataGrid_TaskSet_textBox_readWriteAdd", l_jsontype.task[i].read[j].address);
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, rownum, "_DataGrid_TaskSet_textBox_readLen", l_jsontype.task[i].read[j].read_length.ToString());
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, rownum, "_DataGrid_TaskSet_textBox_dataLen", l_jsontype.task[i].read[j].data_length.ToString());
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, rownum, "_DataGrid_TaskSet_textBox_writeAdd", null);
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 10, rownum, "_DataGrid_TaskSet_textBox_dataIndex", IntArrTostr(l_jsontype.task[i].read[j].data_index));
                                    rownum = rownum + 1;
                                }
                                //写循环
                                for (int k = 0; k < l_jsontype.task[i].write.Count; k++)
                                {
                                    if (l_jsontype.task[i].write[k].task_number == 0)
                                    {
                                        strTaskNum = "任务" + (i + 1);
                                    }
                                    else
                                    {
                                        strTaskNum = "任务" + l_jsontype.task[i].write[k].task_number;
                                    }
                                    //MessageBox.Show(l_jsontype.task[i].write[k].task_number.ToString());
                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, rownum, "_DataGrid_TaskSet_ComBox_TaskNum",  strTaskNum);
                                    //设置背景色
                                    setTaskBackColor(strTaskNum, rownum);
                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, rownum, "_DataGrid_TaskSet_ComBox_DeviceSet", "COM" + l_jsontype.task[i].write[k].port);
                                    if (l_jsontype.task[i].write[k].is_main != null)
                                    {
                                        if (l_jsontype.task[i].write[k].is_main.Equals("\u662f"))
                                        {
                                            SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", l_jsontype.task[i].write[k].is_main);
                                        }
                                    }
                                    else
                                    {
                                        SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", null);
                                    }
                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, rownum, "_DataGrid_TaskSet_ComBox_ReadWrite", "写");
                                    SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 5, rownum, "_DataGrid_TaskSet_ComBox_DeviceNum", l_jsontype.task[i].write[k].site_number + "号站");
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, rownum, "_DataGrid_TaskSet_textBox_readWriteAdd", null);
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, rownum, "_DataGrid_TaskSet_textBox_readLen", null);
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, rownum, "_DataGrid_TaskSet_textBox_dataLen", null);
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, rownum, "_DataGrid_TaskSet_textBox_writeAdd", l_jsontype.task[i].write[k].address);
                                    SetDataTemplateData("TextBox", _DataGrid_TaskSet, 10, rownum, "_DataGrid_TaskSet_textBox_dataIndex", IntArrTostr(l_jsontype.task[i].write[k].data_index));
                                    rownum = rownum + 1;
                                }
                            }

                        _Grid_DeviceSet.Visibility = Visibility.Visible;
                        _Grid_TaskSet.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        TaskTable.Clear();
                        dt.Clear();

                        //增加端口模板控件数据
                        string[] lines = File.ReadAllLines(dialog.FileName);
                        foreach (string line in lines)
                        {
                            jsonstr += (line + Environment.NewLine);
                        }
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        JsonType l_jsontype = new JsonType();
                        l_jsontype = js.Deserialize<JsonType>(jsonstr);

                        //更新端口数据 DviceSetDataGrid 
                        int deviceRow = l_jsontype.serial.Count;
                        DataRow dr;

                        //增加端口列表行
                        dataGirdAddRow(_DataGrid_DeviceSet, l_jsontype.serial.Count);

                        //增加任务列表行
                        for (int i = 0; i < l_jsontype.task.Count; i++)
                        {
                            dataGirdAddRow(_DataGrid_TaskSet, l_jsontype.task[i].read.Count);
                            dataGirdAddRow(_DataGrid_TaskSet, l_jsontype.task[i].write.Count);
                        }

                        Window nwindow = new Window();
                        nwindow.Width = 10;
                        nwindow.Height = 10;
                        nwindow.Show();
                        nwindow.Close();



                        string synstr = null;
                        string syndata = null;
                        string fault = null;
                        string stop = null;
                        for (int i = 0; i < deviceRow; i++)
                        {
                            synstr = l_jsontype.serial[i].attribute[2].ToString();
                            if (l_jsontype.serial[i].fault == "0") fault = "";
                            else fault = l_jsontype.serial[i].fault;
                            if (l_jsontype.serial[i].stop == "0") stop = "";
                            else stop = l_jsontype.serial[i].stop;
                            switch (synstr)
                            {
                                case "0":
                                    {
                                        syndata = "N[0]";
                                    }
                                    break;
                                case "1":
                                    {
                                        syndata = "O[1]";
                                    }
                                    break;
                                case "2":
                                    {
                                        syndata = "E[2]";
                                    }
                                    break;
                                default:
                                    MessageBox.Show("打开同步位数据失败！", "提示");
                                    break;
                            }
                            SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 1, i, "_DataGrid_DeviceSet_ComBox_DeviceSet", "COM" + l_jsontype.serial[i].port);
                            SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 2, i, "_DataGrid_DeviceSet_ComBox_Protocol", l_jsontype.serial[i].protocol);
                            SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 3, i, "_DataGrid_DeviceSet_ComBox_Spd", l_jsontype.serial[i].attribute[0].ToString());
                            SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 4, i, "_DataGrid_DeviceSet_ComBox_Bit", l_jsontype.serial[i].attribute[1].ToString());
                            SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 5, i, "_DataGrid_DeviceSet_ComBox_Sync_Bit", syndata);
                            SetDataTemplateData("ComboBox", _DataGrid_DeviceSet, 6, i, "_DataGrid_DeviceSet_ComBox_Stop_Bit", l_jsontype.serial[i].attribute[3].ToString());
                            SetDataTemplateData("RadioButton", _DataGrid_DeviceSet, 7, i, "_DataGrid_DeviceSet_RadioBut_Is_Main", l_jsontype.serial[i].main.ToString());
                            if (!l_jsontype.serial[i].main)
                            {
                                SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 8, i, "_DropDevice_siteTxtBox", fault);
                                SetDataTemplateData("TextBox", _DataGrid_DeviceSet, 9, i, "_DropDevice_stopTxtBox", stop);
                            }
                            else
                            {
                                isMainDevice = "COM" + l_jsontype.serial[i].port;
                            }
                        }

                        //增加任务模板控件数据    
                        int rownum = 0;
                        string strTaskNum = null;
                        //任务循环
                        for (int i = 0; i < l_jsontype.task.Count; i++)
                        {//读循环
                            for (int j = 0; j < l_jsontype.task[i].read.Count; j++)
                            {
                                if (l_jsontype.task[i].read[j].task_number == 0)
                                {
                                    strTaskNum = "任务" + (i + 1);
                                }
                                else
                                {
                                    strTaskNum = "任务" + l_jsontype.task[i].read[j].task_number;
                                }

                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, rownum, "_DataGrid_TaskSet_ComBox_TaskNum", strTaskNum);
                                //设置背景色
                                setTaskBackColor(strTaskNum, rownum);
                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, rownum, "_DataGrid_TaskSet_ComBox_DeviceSet", "COM" + l_jsontype.task[i].read[j].port);
                                if (l_jsontype.task[i].read[j].is_main!=null)
                                {
                                    if (l_jsontype.task[i].read[j].is_main.Equals("\u662f"))
                                    {
                                        SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", l_jsontype.task[i].read[j].is_main);
                                    } 
                                }
                                else
                                {
                                    SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", null);
                                }

                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, rownum, "_DataGrid_TaskSet_ComBox_ReadWrite", "读");
                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 5, rownum, "_DataGrid_TaskSet_ComBox_DeviceNum", l_jsontype.task[i].read[j].site_number + "号站");
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, rownum, "_DataGrid_TaskSet_textBox_readWriteAdd", l_jsontype.task[i].read[j].address);
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, rownum, "_DataGrid_TaskSet_textBox_readLen", l_jsontype.task[i].read[j].read_length.ToString());
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, rownum, "_DataGrid_TaskSet_textBox_dataLen", l_jsontype.task[i].read[j].data_length.ToString());
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, rownum, "_DataGrid_TaskSet_textBox_writeAdd", null);
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 10, rownum, "_DataGrid_TaskSet_textBox_dataIndex", IntArrTostr(l_jsontype.task[i].read[j].data_index));
                                rownum = rownum + 1;
                            }
                            //写循环
                            for (int k = 0; k < l_jsontype.task[i].write.Count; k++)
                            {
                                if (l_jsontype.task[i].write[k].task_number == 0)
                                {
                                    strTaskNum = "任务" + (i + 1);
                                }
                                else
                                {
                                    strTaskNum = "任务" + l_jsontype.task[i].write[k].task_number;
                                }
                                //MessageBox.Show(l_jsontype.task[i].write[k].task_number.ToString());
                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 1, rownum, "_DataGrid_TaskSet_ComBox_TaskNum",  strTaskNum);
                                //设置背景色
                                setTaskBackColor(strTaskNum, rownum);
                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 2, rownum, "_DataGrid_TaskSet_ComBox_DeviceSet", "COM" + l_jsontype.task[i].write[k].port);
                                if (l_jsontype.task[i].write[k].is_main != null)
                                {
                                    if (l_jsontype.task[i].write[k].is_main.Equals("\u662f"))
                                    {
                                        SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", l_jsontype.task[i].write[k].is_main);
                                    }
                                }
                                else
                                {
                                    SetDataTemplateData("Label", _DataGrid_TaskSet, 3, rownum, "_DataGrid_TaskSet_Label_Is_Main", null);
                                }
                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 4, rownum, "_DataGrid_TaskSet_ComBox_ReadWrite", "写");
                                SetDataTemplateData("ComboBox", _DataGrid_TaskSet, 5, rownum, "_DataGrid_TaskSet_ComBox_DeviceNum", l_jsontype.task[i].write[k].site_number + "号站");
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 6, rownum, "_DataGrid_TaskSet_textBox_readWriteAdd", null);
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 7, rownum, "_DataGrid_TaskSet_textBox_readLen", null);
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 8, rownum, "_DataGrid_TaskSet_textBox_dataLen", null);
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 9, rownum, "_DataGrid_TaskSet_textBox_writeAdd", l_jsontype.task[i].write[k].address);
                                SetDataTemplateData("TextBox", _DataGrid_TaskSet, 10, rownum, "_DataGrid_TaskSet_textBox_dataIndex", IntArrTostr(l_jsontype.task[i].write[k].data_index));
                                rownum = rownum + 1;
                            }
                        }

                        _Grid_DeviceSet.Visibility = Visibility.Visible;
                        _Grid_TaskSet.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }



        }

        private void dataGirdAddRow(DataGrid myDataGird, int rows)
        {
            ////端口设置DataTable  
            //DataTable dt = new DataTable();
            ////任务设置DataTable  
            //DataTable TaskTable = new DataTable();
            switch (myDataGird.Name)
            {
                case "_DataGrid_TaskSet": 
                    {
                        for (int i = 0; i < rows; i++)
                        {
                            DataRow dr = TaskTable.NewRow();  
                            TaskTable.Rows.Add(dr); 
                        }
                        myDataGird.ItemsSource = TaskTable.DefaultView;
                    }
                        break;
                case "_DataGrid_DeviceSet":
                    {
                        
                        for (int i = 0; i < rows; i++)
                        {
                            DataRow t_dr = dt.NewRow(); 
                            dt.Rows.Add(t_dr);
                        }
                        //myDataGird.ItemsSource = dt.DefaultView;
                    }
                    break;
                        default:
                         break;
            } 
        }
        //将字符串转为int数组
        public int[] strToIntArr(string data)
        {
            int[] intdataindexArr = null;
            if (data!=null)
            {
                string strdata = data;
                string[] strdataindexArr = strdata.Split(',');
                intdataindexArr = new int[strdataindexArr.Length];
                for (int m = 0; m < strdataindexArr.Length; m++)
                {
                    intdataindexArr[m] = Convert.ToInt32(strdataindexArr[m]);
                }
            } 
            return intdataindexArr;
        }

        //将int数组转为字符串
        public string IntArrTostr(int[] data)
        {
            string strdata = null;
            if (data!=null)
            {
                int[] intdata = data; 
                for (int m = 0; m < intdata.Length; m++)
                {
                    strdata = strdata + intdata[m].ToString() + ",";
                }
                strdata = strdata.Substring(0, strdata.Length - 1);
            } 
            return strdata;
        }

        public void setTaskBackColor(string taskNum,int rowNum)
        {
            DataGridRow dgrow = (DataGridRow)_DataGrid_TaskSet.ItemContainerGenerator.ContainerFromIndex(rowNum);
            if (!taskNum.Equals("") && !taskNum.Equals(null) && taskNum.Length > 2 && Convert.ToInt16((taskNum.Substring(taskNum.IndexOf("务") + 1, taskNum.Length - 2))) % 2 == 0)
            {
                Color backgroundColor = ((Color)ColorConverter.ConvertFromString("#FFF5B7B7"));
                dgrow.Background = new SolidColorBrush(backgroundColor);
            }
            else
            {
                Color backgroundColor = ((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                dgrow.Background = new SolidColorBrush(backgroundColor);
            }
        }

    }
      
}

