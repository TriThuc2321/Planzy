   M S B A M L   `   `   `  ����A  =Planzy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Planzy.ViewsPlanzy.Views  5   	   5clr-namespace:Planzy.ViewModelsPlanzy.ViewModels  5
   R   "    Planzy.Views.UpdateInforUser   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Planzy.Views     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
   	   0
viewmodelsclr-namespace:Planzy.ViewModels  6R   -   5        $��
UpdateUser��5   	     � 
ResizeMode$  NoResize=�5   	    � WindowStyle$
 None=�5	   	    � WindowStartupLocation$ CenterScreen=�5   	      MaterialDesignPaper#���   5   	     MaterialDesignFont#���  5   	    � Title$ UpdateInforUser��5   	   $	��450��6!   $	��400��6.   .��5      � *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5   
   +  %Microsoft.Xaml.Behaviors.EventTrigger  5         	EventName$ Loaded��6   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 5      �� *LoadWindowCommand+   CommandParameter 5      �� 	 �� ElementName$	 
UpdateUser��5      .+�5   
   �� $��2q�6   ���	#FF5867AC._�5      � 
 � RowDefinitions
 5      �� 5      $��40 �6$   �� 5      �� 5      �� 5      �� 5       �� 5!      �� 5"      �� 5#      �� 5$      .+�5'      � $��1��6   .+�5(      �� $��CẬP NHẬT THÔNG TIN��5)      $	��Top=�5*      $��Center=�5+      $��25(�5,      ���	#FF5867AC$
��Bold"�5.      50      � 52      $��2��6   .+�53      �� $��0��6!   $��0��6.    �� Orientation$ 
Horizontal=�54   !   $��	40 0 0 10q�55   !   $	��Top=�56   !   .+�57      �� $��Họ và tên��6    $��Bottom=�58   ,   $
��Bold"�59   ,   ���	#FF5867AC$	��100��5;   ,   �� 5<      $	��220��6"   $��Bottom=�5>   "   $
��Bold"�5?   "   ���	#FF5867AC$��0��6L   ��5=   "   �� *
UserName+5?   L   5B      5C      � 5E      $��3��6   .+�5F      �� $��0��6!   $��0��6.   $ 
Horizontal=�5G   -   $��	40 0 0 10q�5H   -   $	��Top=�5I   !   .+�5J      �� $��Gmail: ��6    $��Bottom=�5K   ,   $
��Bold"�5L   ,   ���	#FF5867AC$	��100��5N   ,   �� 5O      $��Bottom=�5P   ,   $
��Bold"�5Q   ,   ���	#FF5867AC$	��220��5S   ,   ��5O       �� *Gmail+5S   ,   5T      5U      � 5W      $��4��6   .+�5X      �� .+�5Y      �� $��0��6%   $��0��62   $ 
Horizontal=�5Z   %   $��	40 0 0 10q�5[   %   $	��Top=�5\   %   .+�5]      �� $��Điện thoại: ��6$   $��Bottom=�5^   ,   $
��Bold"�5_   ,   ���	#FF5867AC$	��100��5a   ,   �� 5b      $	��220��6&   $��Bottom=�5c   "   $
��Bold"�5d   "   ���	#FF5867AC$��0��6L   ��5e   "   �� *PhoneNumber+5g      �� 5i      $,��&Số điện thoại không hợp lệ��6    
���  ��$��Italic$�5k       $��	217 0 0 0q�5l       y�5m       �� *InvalidPhoneNumberVisibility+5o      5q      � 5s      $��5��6    � ColumnDefinitions 5t      �� 5u      $	��26* �6+   �� 5v      $��7* �6+   .+�5x      �� $��2��6!   .+�5y      �� $��0��6%   $��0��62   $ 
Horizontal=�5z   !   $��	40 0 0 10q�5{   !   $	��Top=�5|   !   .+�5~      �� $��CMND: ��6$   $��Bottom=�5   ,   $
��Bold"�5�   ,   ���	#FF5867AC$	��100��5�   ,   �� 5�      $	��220��6&   $��Bottom=�5�   "   $
��Bold"�5�   "   ���	#FF5867AC$��0��6L   ��5�   "   �� *CMND+5�   L   5�      �� 5�      $/��)Chứng minh nhân dân không hợp lệ��6    
���  ��$��Italic$�5�       $��	171 0 0 0q�5�       y�5�       �� *InvalidCMNDVisibility+5�      5�      � 5�      $��6��6   .+�5�      �� .+�5�      �� $��0��6%   $��0��62   $ 
Horizontal=�5�   !   $��	40 0 0 10q�5�   !   $	��Top=�5�   !   .+�5�      �� $��Địa chỉ: ��5�   !   $��Bottom=�5�   !   $
��Bold"�5�   !   ���	#FF5867AC$	��100��5�   !   �� 5�      $	��220��6&   $��Bottom=�5�   "   $
��Bold"�5�   "   ���	#FF5867AC$��0��6L   ��5�   "   �� *	Address+5�   L   5�      �� 5�      $��Lưu thành công��6    
��� � �$��Italic$�5�       $��90��5�       $��Right=�5�       $��	245 0 0 0q�5�       y�5�       �� *TextMessageVisibility+5�      5�      � 5�      $��7��6   .+�5�      �� $ 
Horizontal=�6!   .+�5�      �� $	��100��5�       " MaterialDesignFlatDarkBgButton#��[ 5�      $
��Lưu��5�      
����·�$��85 0 0 0q�5�      ��5�      �� *Save+��5�      �� $	 
UpdateUser���� 5�      $	��100��5�      #��[ 5�      $��Hủy��5�      
����·�$��50 0 0 0q�5�      ��5�      �� *Cancel+��5�      �� $	 
UpdateUser��5�      5�      5�      5�      5�      5�      