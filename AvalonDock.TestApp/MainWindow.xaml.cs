using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Layout;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using System.Resources;
using System.Collections;
using Xceed.Wpf.AvalonDock;
using System.Runtime.InteropServices;
using Xceed.Wpf.AvalonDock.Themes;
using System.ComponentModel;
using System.Windows.Forms;

namespace DEA3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Menu_Sys_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tool_But_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Dp_Project_close_but_Click(object sender, RoutedEventArgs e)
        {
            //Dp_Project.Visibility = Visibility.Hidden;

            System.Windows.MessageBox.Show(Dp_Main.Width.ToString());
            //Dp_work_arer.Width = 2000;
            //dg1.Width = 300;
        }

        private void Tool_But_Project_Click(object sender, RoutedEventArgs e)
        {
            //Dp_Project.Visibility = Visibility.Visible;
        }
        //动态增加选项卡
        int clickCount = 0;
        private void item_AddNew_Click(object sender, RoutedEventArgs e)
        {
            clickCount++;
            LayoutAnchorable layOutAnc = new LayoutAnchorable() { Title = "新选项卡" + clickCount };
            layOutAnc.Content = new System.Windows.Controls.TextBox() { Text = "这是第" + clickCount + "个新选项卡" };
            layOutPaneContent.Children.Add(layOutAnc);
            layOutAnc.Closing += Tab_Close;//添加退出事件处理句柄=>添加的代码
        }
        //添加退出事件处理句柄
        private void Tab_Close(object sender, CancelEventArgs e)
        {
            if (true)
            {
                if (System.Windows.MessageBox.Show("还没有保存，是否要退出？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        //外观方法
        private void miGeneric_Click_1(object sender, RoutedEventArgs e)
        {
            dockingManager.Theme = new GenericTheme();
        }
        private void miDark_Click_1(object sender, RoutedEventArgs e)
        {
            dockingManager.Theme = new ExpressionDarkTheme();
        }
        private void miLight_Click_1(object sender, RoutedEventArgs e)
        {
            dockingManager.Theme = new ExpressionLightTheme();
        }
        private void miMetro_Click_1(object sender, RoutedEventArgs e)
        {
            dockingManager.Theme = new MetroTheme();
        }
        private void miVS_Click_1(object sender, RoutedEventArgs e)
        {
            dockingManager.Theme = new VS2010Theme();
        }


        //
        //
        //以下处理键盘钩子程序
        //
        /// 声明回调函数委托  
        /// </summary>  
        /// <param name="nCode"></param>  
        /// <param name="wParam"></param>  
        /// <param name="lParam"></param>  
        /// <returns></returns>  
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>  
        /// 委托实例  
        /// </summary>  
        HookProc KeyboardHookProcedure;

        /// <summary>  
        /// 键盘钩子句柄  
        /// </summary>  
        static int hKeyboardHook = 0;

        //装置钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState); 

        //卸下钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //获取某个进程的句柄函数  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>  
        /// 普通按键消息  
        /// </summary>  
        private const int WM_KEYDOWN = 0x100;
        /// <summary>  
        /// 系统按键消息  
        /// </summary>  
        private const int WM_SYSKEYDOWN = 0x104;

        //鼠标常量   
        public const int WH_KEYBOARD_LL = 13;

        //声明键盘钩子的封送结构类型   
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode; //表示一个在1到254间的虚似键盘码   
            public int scanCode; //表示硬件扫描码   
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        //大小写状态
        public static bool CapsLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                //MessageBox.Show(bs[0x14].ToString());
                return (bs[0x14] == 1);
            }
        }

        //数值打开状态
        public static bool NumLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x90] == 1);
            }
        }

        //插入覆盖状态
        public static bool InsertLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x2D] == 1);
            }
        }

        /// <summary>  
        /// 截取全局按键，发送新按键，返回  
        /// </summary>  
        /// <param name="nCode"></param>  
        /// <param name="wParam"></param>  
        /// <param name="lParam"></param>  
        /// <returns></returns>  
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                System.Windows.Forms.Keys keyData = (System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode;

                switch (MyKeyboardHookStruct.vkCode)
                {
                    case 20:
                        if (CapsLockStatus) { statusBar_CapsLock.Text = "Caps Lock:[OFF]"; }
                        else { statusBar_CapsLock.Text = "Caps Lock:[ON]"; }
                        break;
                    case 144:
                        if (NumLockStatus) { statusBar_NumLock.Text = "Num Lock:[OFF]"; }
                        else { statusBar_NumLock.Text = "Num Lock:[ON]"; }
                        break;
                    case 45:
                        if (InsertLockStatus) { statusBar_InsertLock.Text = "Insert Lock:[OFF]"; }
                        else { statusBar_InsertLock.Text = "Insert Lock:[ON]"; }
                        break;
                    default: //MessageBox.Show(MyKeyboardHookStruct.vkCode.ToString());
                        break;
                } 
            }
            return 0;
        }
         
        private void MainAppWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            bool retKeyboard = true;

            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            //如果卸下钩子失败   
            if (!(retKeyboard)) throw new Exception("卸下钩子失败！");
        }

        private void MainAppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //实例化委托  
            //启动键盘钩子   
            if (hKeyboardHook == 0)
            {
                //实例化委托  
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        ////
        ////树型鼠标事件
        ////
        ////设置树单选,就是只能有一个树节点被选中
        //private void SetNodeCheckStatus(TreeNode tn, TreeNode node)
        //{
        //    if (tn == null)
        //        return;
        //    if (tn != node)
        //    {
        //        tn.Checked = false;
        //    }
        //    // Check children nodes
        //    foreach (TreeNode tnChild in tn.Nodes)
        //    {
        //        if (tnChild != node)
        //        {
        //            tnChild.Checked = false;
        //        }
        //        SetNodeCheckStatus(tnChild, node);
        //    }
        //}
        ////在树节点被选中后触发
        //private void treeView1_AfterCheacked(object sender, TreeViewEventArgs e)
        //{
        //    //过滤不是鼠标选中的其它事件，防止死循环
        //    if (e.Action != TreeViewAction.Unknown)
        //    {
        //        //Event call by mouse or key-press
        //        foreach (TreeNode tnChild in treeView_Project.Nodes)
        //            SetNodeCheckStatus(tnChild, e.Node);
        //        string sName = e.Node.Text;
        //    }
        //}
        ////获得选择节点
        //private void GetSelectNode(TreeNode tn)
        //{
        //    if (tn == null)
        //        return;
        //    if (tn.Checked == true)
        //    {
        //        m_NodeName = tn.Text;
        //        return;
        //    }
        //    // Check children nodes
        //    foreach (TreeNode tnChild in tn.Nodes)
        //    {
        //        GetSelectNode(tnChild);
        //    }
        //}
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    TreeNode node = null;
        //    foreach (TreeNode tnChild in treeView1.Nodes)
        //    {
        //        GetSelectNode(tnChild);
        //    }
        //    string sName = m_NodeName;
        //}
        ////选择树的节点并点击右键，触发事件
        //private void treeView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    //MessageBox.Show("点击了树！");
        //    textBox1.Text = "1:" + "点击了树";
        //    if (e.Button == MouseButtons.Right)//判断你点的是不是右键
        //    {
        //        textBox1.Text = "2:" + e.Button.ToString();

        //        Point ClickPoint = new Point(e.X, e.Y);
        //        TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);

        //        textBox1.Text = "3:" + ClickPoint.ToString() + "||" + CurrentNode.ToString() + "||" + CurrentNode.Checked;
        //        textBox2.Text = "";

        //        treeView1.SelectedNode = CurrentNode;//选中这个节点

        //        if (CurrentNode != null && CurrentNode.IsSelected == true)
        //        {
        //            textBox2.Text = "4:" + ClickPoint.ToString() + "||" + CurrentNode.ToString() + "||" + CurrentNode.IsSelected.ToString() + "||" + CurrentNode.Level;

        //            switch (CurrentNode.Level)//根据不同节点显示不同的右键菜单，当然你可以让它显示一样的菜单
        //            {
        //                case 0:
        //                    CurrentNode.ContextMenuStrip = contextMenuStrip1;
        //                    break;
        //                case 1:
        //                    CurrentNode.ContextMenuStrip = contextMenuStrip1;
        //                    break;
        //                case 2:
        //                    CurrentNode.ContextMenuStrip = contextMenuStrip1;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        //}
        //private String m_NodeName = null;
        ////右键设置节点可以重命名
        //private void toolStripMenuItem2_Click(object sender, EventArgs e)
        //{
        //    //窗体的LabelEdir为false，因此每次要BeginEdit时都要先自LabelEdit为true
        //    treeView1.LabelEdit = true;
        //    treeView1.SelectedNode.BeginEdit();
        //}
        ////右键添加节点
        //private void toolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    //在Tree选择节点的同一级添加节点
        //    treeView1.LabelEdit = true;
        //    TreeNode CurrentNode = treeView1.SelectedNode.Nodes.Add("Node1");
        //    //更新选择节点
        //    treeView1.SelectedNode.Checked = false;
        //    CurrentNode.Checked = true;
        //    //使添加的树节点处于可编辑的状态
        //    CurrentNode.BeginEdit();
        //}
        ////右键删除节点
        //private void toolStripMenuItem3_Click(object sender, EventArgs e)
        //{
        //    treeView1.SelectedNode.Remove();
        //}


    }
}
