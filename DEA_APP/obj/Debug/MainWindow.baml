   M S B A M L   `   `   `  ����B  >DEA_CFG, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:DEA_APPDEA_APP  5   	       DEA_APP.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:DEA_APP     5   	   -   5          �Name$  Dea_Maim_Window��5   	    � Title$ Data Exchange Adapter��5   	    � WindowStartupLocation$ CenterScreen=�5	   	   ��. $
��1053��5   	   $
��auto��5   	    � 	Resources 5         GetTaskNumList S� 
ObjectType   DEA_APP.GetInfoService S� 
MethodName  GetComNumList  GetReadWriteList  GetDeviceNumList  
GetSpdList  
GetBitList  GetSyncBitList  GetStopBitList  GetProtocolList��%E  &	        &	 7     &	 m     &	 �     &	 �     &	     &	 E    &	 |    &	 �    S� 5   
   "  65   $ GetTaskNumList��6`   6~   S� 5   
   "  64   $ GetComNumList��6`   6}   S� 5   
   "  67   $ GetReadWriteList��6b   6�   S� 5   
   "  66   $ GetDeviceNumList��6b   6�   S� 5   
   "  60   $ 
GetSpdList��6\   6v   S� 5   
   "  60   $ 
GetBitList��6\   6v   S� 5   
   "  64   $ GetSyncBitList��6`   6~   S� 5   
   "  64   $ GetStopBitList��6`   6~   S� 5   
   "  65   $ GetProtocolList��6a   6�   5      .��5      �� -   5         ��Name$ scrollViewer��5      $
��auto��6)   $
��auto��66   $
��Auto=�6D   $��Visible=�6g   .��5   
   ]� -   5        $��panel��5      .+�5      o� -   5         o�Name$
 menu��5      $��25��6!   $	��Top=�6-   .@�5       m� -   5        $��menu_System��5    (   $��系统��6   .@�5!      m� -   5        $��menu_System_Log��5!   +   $��日志��6   6b   m� 5"      -   5        $��menu_account��5"   -   $��操作说明��6   6^   m� 5#      -   5        $��	menu_exit��5#   +   $��退出��6   $��25��6<   6c   5$      m� 5%      -	   5        $��menu_Task_top��5%   (   $��任务��6   .@�5&      m� -
   5        $��menu_con��5&   -   $��联机测试��6   $��25��6=   6b   m� 5'      -   5        $��menu_Open_Task��5'   -   $��打开任务��6   $��25��6C   6o   m� 5(      -   5        $��	menu_Task��5(   -   $��任务管理��6   $��25��6>   6d   5)      50      � 54      -   5         �Name$ _Grid_DeviceSet��54      $	��Top=�6-   $��0,5,0,0q�6C   $
��auto��6T   	 � ShowGridLines	 .  
 � RowDefinitions
 55      �� 56      $��25 �6$   �� 57      $��28 �6$   �� 58      $
��auto �6$    � ColumnDefinitions 5:      �� 5;      $
��auto �6'   �� 5<      $
��auto �6'   �� 5=      $
��auto �6'   .+�5?      �� 
�������$��0��6+   $��3��68   $��端口设置��6L   $
��Bold"�6[   6o   �� 5@      -   5        $��_Add_Devece��5@      $��75��6,   $��25��67   $��增加端口��6C   $��1��6R   $��0��6_   $
��Left=�6o   $��14(�6�   $
��Bold"�6�   6�   &   System.Windows.Controls.DataGrid  5B      -   5        $��_DataGrid_DeviceSet��5B      $��2��5C      $��3��5D         AutoGenerateColumns .     CanUserAddRows .     RowDetailsVisibilityMode$ VisibleWhenSelected=�5G         SelectionMode$ Extended=�5H         SelectionUnit$ FullRow=�5I      ���	#FFCB9797   Columns 5L      4  .System.Windows.Controls.DataGridTemplateColumn  5M      -   5        _DataGrid_DeviceSet_do1��,  &System.Windows.Controls.DataGridColumn   Header$ 操作��5M   R      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6^      CellTemplate 5N      �� 5O   "   .R�5P   &   �� $��_DropDevice_but��6-   -   5        $��1,1,1,1q�5P   D   $��删除��6U   6�   5Q   #   5S        5U      $ 端口 *��61    5V      �� 5W   "   .R�5X   &   �� $*��$_DataGrid_DeviceSet_ComBox_DeviceSet��6/   -   5        $��65��5X   �   $��1,1,1,1q�6�   ��6[   ��  �� Source# [ 6�   5Y   #   5[        5\      $ 端口协议 *��61   -   5        ($_DataGrid_DeviceSet_ComBox_Protocol1�� 5]      �� 5^   "   .R�5_   &   �� $)��#_DataGrid_DeviceSet_ComBox_Protocol��6/   $	��180��6�   $��1,1,1,1q�6�   ��6[   �� # [ 6�   5`   #   5b        5c      $ 传输速度 *��61    5d      �� 5e   "   .R�5f   &   �� $$��_DataGrid_DeviceSet_ComBox_Spd��6/   $��70��6�   $��1,1,1,1q�6�   ��6U   �� # [ 6�   5g   #   5i        5j      $ 位长 *��61    5k      �� 5l   "   .R�5m   &   �� $$��_DataGrid_DeviceSet_ComBox_Bit��6/   $��50��6�   $��1,1,1,1q�6�   ��6U   �� # [ 6�   5n   #   5p        5q      $ 同步位 *��61    5r      �� 5s   "   .R�5t   &   �� $)��#_DataGrid_DeviceSet_ComBox_Sync_Bit��6/   $��60��6�   $��1,1,1,1q�6�   ��6Z   �� # [ 6�   5u   #   5w        5x      $ 停止位 *��61    5y      �� 5z   "   .R�5{   &   �� $)��#_DataGrid_DeviceSet_ComBox_Stop_Bit��6/   $��50��6�   $��1,1,1,1q�6�   ��6Z   �� # [ 6�   5|   #   5~        5�      $ 是否主站��62    5�      �� 5�   "   .R�5�   &   	� $*��$_DataGrid_DeviceSet_RadioBut_Is_Main��6C   -   5         	� 	GroupName$
 test��5�   2   $��1,1,1,1q�6o   $��Center=�6�    d� 	IsChecked$ FalseY�6�   5�   #   5�        5�      $ 故障设备记录元件��61    5�      �� 5�   "   .R�5�   &   �� $��_DropDevice_siteTxtBox��6.   -   5        $��90��5�   N    �� 	MaxLength$ 10��6[   $��1,1,1,1q�6l   $��Center=�6}   $��Stretch=�6�    �� MaxLines$ 15��6�   6/  5�   #   5�        5�      $ 设备停止记录元件��61    5�      �� 5�   "   .R�5�   &   �� $��_DropDevice_stopTxtBox��6/   -   5        $��90��5�   M   $ 10��6Z   $��1,1,1,1q�6k   $��Center=�6|   $��Stretch=�6�   $ 15��6�   6-  5�   #   5�      5�      5�      � 5�      -   5        $ _Grid_TaskSet��5�      $	��Top=�6+   $��0,5,0,0q�6A   $
��auto��6R   	 .  
 5�      �� 5�      $��25 �6$   �� 5�      $��28 �6$   �� 5�      $
��auto �6$   �� 5�      $��25 �6$   �� 5�      $��25 �6$   �� 5�      $��25 �6$    5�      �� 5�      $
��auto �6'   �� 5�      $	��224 �6'   �� 5�      $	��126 �6'   �� 5�      $��99 �6'   �� 5�      $��40 �6'   �� 5�      $
��auto �6'   .+�5�      �� 
�������$��0��6+   $��6��68   $��任务设置��6L   $
��Bold"�6[   6o   �� 5�      -   5        $��_Add_Task_But��5�      $��75��6.   $��25��69   $��	新增站��6E   $��1��6S   $��0��6`   $
��Left=�6p   $	��0,2q�6�   $��14(�6�   $
��Bold"�6�   6�   �� 5�      $��任务延时(毫秒):��6   $��1��6,   $��1��69   $	��100��6I   $
��Left=�6U   $��6,1,0,25q�6p   $��2��6�   6�   �� 5�      -   5        $��	taskDelay��5�      $y�Visible=�6+   $��1��6A   $��1��6N   $��50��6^   $��99,4,0,5q�6i    J� RenderTransformOrigin$ -1.321,0.527&�6{   $
��Left=�6�   $��5��6�   6�     5�      -   5        $��_DataGrid_TaskSet��5�      $��2��5�      $��6��5�       .   .  $ VisibleWhenSelected=�5�      $ Extended=�5�      $ FullRow=�5�      ���	#FFCB9797 5�        5�      -   5        _DataGrid_TaskSet_do��$ 操作��5�   O   $ 50 6[    5�      �� 5�   "   .R�5�   &   �� $��_DropTask_but��6-   -   5        $��1,1,1,1q�5�   B   $��删除��6S   6   5�   #   5�        5�      $ 任务号 *��61    5�      �� 5�   "   .R�5�   &   �� $&�� _DataGrid_TaskSet_ComBox_TaskNum��6/   -   5        $��65��5�   �   $��1,1,1,1q�6�    �� Text$  ��6�   ��6W   �� # [  6�   5�   #   5�        5�      $ 端口 *��61    5�      �� 5�   "   .R�5�   &   �� $(��"_DataGrid_TaskSet_ComBox_DeviceSet��6/   -   5        $��65��5�   �   $��1,1,1,1q�6�   ��6Y   �� # [ 6�   5�   #   5�        5�      $ 主站设备��61    5�      �� 5�   "   .R�5�   &   �� $%��_DataGrid_TaskSet_Label_Is_Main��6,   $��40��6T   $��1,1,1,1q�6_   $��Center=�6p   $��Stretch=�6�   {�.  6�   5�   #   5�        5�      $ 	读/写 *��61    5�      �� 5�   "   .R�5�   &   �� $(��"_DataGrid_TaskSet_ComBox_ReadWrite��6/   -   5        $��65��5�   �   $��1,1,1,1q�6�   ��6Y   �� # [ 6  5�   #   5�        5�      $ 站号 *��61    5�      �� 5�   "   .R�5�   &   �� $(��"_DataGrid_TaskSet_ComBox_DeviceNum��6/   $��65��6�   $��1,1,1,1q�6�   ��6Y   �� # [ 6�   5�   #   5�        5�      $ 读取地址/元件��61    5�      �� 5�   "   .R�5�   &   �� $,��&_DataGrid_TaskSet_textBox_readWriteAdd��6.   -    5        $ 10��5�   \   $	��120��6l   $��1,1,1,1q�6x   $��Center=�6�   $��Stretch=�6�   6N  5   #   5       5     $ 读取长度��61    5     �� 5  "   .R�5  &   �� $'��!_DataGrid_TaskSet_textBox_readLen��6.   -!   5        $ 2��5  W   $��60��6g   $��1,1,1,1q�6r   $��Center=�6�   $��Stretch=�6�    �� IsInputMethodEnabled .  {�. 6x  5  #   5	       5
     $ 数据长度��61    5     �� 5  "   .R�5  &   �� $'��!_DataGrid_TaskSet_textBox_dataLen��6.   -"   5        $ 2��5  X   $��60��6g   $��1,1,1,1q�6r   $��Center=�6�   $��Stretch=�6�   6>  5  #   5       5     $ 写入地址/元件��61    5     �� 5  "   .R�5  &   �� $(��"_DataGrid_TaskSet_textBox_writeAdd��6.   -#   5        $ 10��5  Y   $	��120��6i   $��1,1,1,1q�6u   $��Center=�6�   $��Stretch=�6�   6C  5  #   5       5     $ 数据索引��61    5     �� 5  "   .R�5  &   �� $)��#_DataGrid_TaskSet_textBox_dataIndex��6.   $	��300��6Z   $��1,1,1,1q�6f   $��Center=�6w   $��Stretch=�6�   6�   5  #   5     5      �� 5!     -$   5        $��_UpLoad_Task_But��5!     $��54��61   $��25��6<   $��上传��6H   $��4��6U   $��1��6b   $��Center=�6r   $��2��6�   $��14(�6�   $
��Bold"�6�   6�   �� 5"     -%   5        $��_ToFile_Task_But��5"     $��56��61   $��25��6<   $��保存��6H   $��4��6U   $��2��6b   $��Center=�6r   $��2��6�   $��14(�6�   $
��Bold"�6�   5$     5%     5&     5'     