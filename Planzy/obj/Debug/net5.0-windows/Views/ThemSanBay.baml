   M S B A M L   `   `   `  ����A  =Planzy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Planzy.ViewsPlanzy.Views  5   	       Planzy.Views.ThemSanBay   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Planzy.Views     5   	     � Title$  
ThemSanBay��5   	   $	��150��6   $	��450��6)    � 
ResizeMode$ NoResize=�65    � WindowStyle$
 None=�6K    � WindowStartupLocation$ CenterScreen=�6^    � Topmost$
 True��6�    � ShowInTaskbar$ False��6�   .��5	      � .+�5
   
   � .+�5      �� $��2q�6   ���	#FF5867AC._�5      ��  �� Orientation$ Vertical=�6   $��Center=�64   $��30 0 0 0q�6O   .+�5      �� $��Thêm sân bay: ��6   $��Bottom=�5      $
��Left=�5      $
��Bold"�5      ���	#FF5867AC$
��Auto��5      $��0 0 0 0q�5      $��13(�5      �� 5      $��1��6!   $��0��6.   $ 
Horizontal=�6>   $��20 5 0 0q�6W   .+�5      �� $��Mã sân bay: ��6    $��Bottom=�5   1   $
��Bold"�5   1   ���	#FF5867AC$
��Auto��5   1   �� 5      $��50��6"   $��Bottom=�5   1   $��10(�5   1   $
��Bold"�5   1   ���	#FF5867AC$��10��6[    � ToolTip$( "Chữ và số in hoa không dấu��5    "   ��5   1   �� *
MaSanBay+5    "   �� 5"      $��Tên sân bay: ��6    $��Bottom=�5#   1   $
��Bold"�5$   1   ���	#FF5867AC$
��Auto��5&   1   $��10 0 0 0q�5'   1   �� 5(      $	��160��6"   $��Bottom=�5)   1   $��10(�5*   1   $
��Bold"�5+   1   ���	#FF5867AC$��10��6[   ��5,   2   �� *	TenSanBay+5.      � 5/      .+�50      �� $��Right=�6%   $ Vertical=�6A   .+�51      �� $'��!Vui lòng nhập đủ thông tin��52      $
��auto��53      $��Right=�54      $��0 0 20 0q�55      
���  ��$��Italic$�57      y�58      �� *CanhBaoNhapDayDuVisible+�� 5:      $��Thêm thành công��5;      $
��auto��5<      $��Right=�5=      $��0 0 20 0q�5>      
��� � �$��DemiBold"�5@      y�5A      �� *ThongBaoNhapThanhCongVisible+5C      �� 5D      $2��,Mã gồm chữ và số in hoa không dấu��5E      $
��auto��5F      $
��Left=�5G      $��20 0 0 0q�5H      
���  ��$��Italic$�5J      y�5K      �� *CanhBaoSaiDinhDangVisible+�� 5L      $#��Mã sân bay đã tồn tại��5M      $
��auto��5N      $
��Left=�5O      $��20 0 0 0q�5P      
���  ��$��Italic$�5R      y�5S      �� *CanhBaoMaDaTonTaiVisible+5T      �� 5U      $��1��6!   $��0��6.   $��0 5 20 0q�6>   $ 
Horizontal=�5V      $��Right=�62   .+�5W      �� $��Thêm��6!   $��Center=�60   $��5 5 0 5q�6K   $��Center=�5X      $	��100��5Y      $��30��5Z      $��4q�5[      ���#000
����·�$��12(�5^      $
��Bold"�5_      ��5`      Z� ��5a   !   �� *XacNhanThemSanBayCommand+�� 5c      $��Hủy��6!   $��Center=�6/   $��5 5 0 5q�6J   $��Center=�5d      $	��100��5e      $��30��5f      $��4q�5g      ���#000
����·�$��12(�5j      $
��Bold"�5k      ��5l      Z� ��5m      �� *HuyThemSanBayCommand+5o      5p      5r      5s      5t      5u      