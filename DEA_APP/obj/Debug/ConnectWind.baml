   M S B A M L   `   `   `  ����B  >DEA_CFG, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:DEA_APPDEA_APP  5   	       DEA_APP.ConnectWind   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:DEA_APP     5   	     � Title$  	DEA联机��5   	   $	��150��6   $	��330��6$    � 
ResizeMode$ NoResize=�60   .��5      �  � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��50 �6    � ColumnDefinitions 5   
   �� 5      $��80 �6   �� 5      $	��150 �6   �� 5      $	��100 �6   .+�5   
   �� -   5        $��dea_id��5      $��1��6   $��0��6+   $��
0,13,14,13q�6;   $��51��6O   $��24��6Z   $��
适配器:��6g   $��Center=�6v   $��Right=�6�   �� 5   
   -   5        $��_DeaIp��5      $��1��6!   $��
78,13,0,13q�6.   $	��140��6B   $��24��6N    �� SelectionOpacity$ 2Q�6Z   $
��Left=�6p   $��Center=�6�   $��***.***.***.***��6�   $��Center=�6�    � ToolTip$ DEA适配器IP地址！��6�   $��2��6�   �� 5   
   -   5        $��_Connect_but��5      $��1��6&   $��2��63   $��联机��6C   $��0q�6P   $��55��6[   $��25��6f   $
��Left=�6�   $��14(�6�   $
��Bold"�6�   5      5      