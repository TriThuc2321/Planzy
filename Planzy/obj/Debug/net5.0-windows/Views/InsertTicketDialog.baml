   M S B A M L   `   `   `  ����A  =Planzy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Planzy.ViewsPlanzy.Views  5   	   5clr-namespace:Planzy.ViewModelsPlanzy.ViewModels  5
   R   %    Planzy.Views.InsertTicketDialog   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Planzy.Views     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
   	   0
viewmodelsclr-namespace:Planzy.ViewModels  6R   -   5        $��InsertticketView��5   	     � 
ResizeMode$  NoResize=�5   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � Title$ InsertTicketDialog��5   	   $	��170��6$   $	��500��61   .��5      � .+�5   
   �� $��2q�6   ���	#FF5867AC._�5      ��  �� Orientation$ Vertical=�6   $��Center=�60   $��30 0 0 0q�6K   .+�5      �� $��Thêm loại hạng vé: ��6   $��Bottom=�5      $
��Left=�5      $
��Bold"�5      ���	#FF5867AC$
��Auto��5      $��0 0 0 0q�5      $��13(�5      �� 5      $��1��6   $��0��6&   $ 
Horizontal=�66   $��20 5 0 0q�6O   .+�5      �� $��Mã hạng vé: ��6   $��Bottom=�5   1   $
��Bold"�5   1   ���	#FF5867AC$
��Auto��5   1   �� 5      $��30��6   $��Bottom=�5    1   $��10(�5!   1   $
��Bold"�5"   1   ���	#FF5867AC$��10��6[   ��5#   1   �� *Id+�� 5%      $��Tên hạng vé: ��6   $��Bottom=�5&   1   $
��Bold"�5'   1   ���	#FF5867AC$
��Auto��5)   1   $��10 0 0 0q�5*   1   �� 5+      $	��110��6   $��Bottom=�5,   1   $��10(�5-   1   $
��Bold"�5.   1   ���	#FF5867AC$��10��6[   ��5/   2   �� *Name+�� 51      $��Tỷ lệ: ��6   $��Bottom=�52   1   $
��Bold"�53   1   ���	#FF5867AC$
��Auto��55   1   $��10 0 0 0q�56   0   �� 57      $��40��6   $��Bottom=�58   1   $��10(�59   1   $
��Bold"�5:   1   ���	#FF5867AC$��10��6[   ��5;   2   �� *Ratio+�� 5=      $��%��6   $��Bottom=�5>   1   $
��Bold"�5?   1   ���	#FF5867AC$
��Auto��5A   1   5C      �� 5D      $
��auto��5F      $��Right=�5G      $��0 0 40 0q�5H      
���  ��$��Italic$�5J      ��5E      �� *ErrorMessage+y�5K      �� *ErrorMessageVisibility+�� 5M      $��Thêm thành công��5N      $
��auto��5O      $��Right=�5P      $��0 0 40 0q�5Q      
��� � �$��DemiBold"�5S      y�5T      �� *SucessMessageVisibility+�� 5V      $��1��6   $��0��6*   $��0 5 40 0q�6:   $ 
Horizontal=�5W      $��Right=�62   .+�5X      �� $��Thêm��6   $��Center=�6,   $��5 5 0 5q�6G   $��Center=�5Y      $	��100��5Z      $��30��5[      $��4q�5\      ���#000
����·�$��12(�5_      $
��Bold"�5`      ��5a      Z� ��5b      �� *InsertCommand+��5c      ��  �� ElementName$ InsertticketView���� 5e      $��Hủy��6   $��Center=�6'   $��5 5 0 5q�6B   $��Center=�5f      $	��100��5g      $��30��5h      $��4q�5i      ���#000
����·�$��12(�5l      $
��Bold"�5m      ��5n      Z� ��5o      �� *CancelCommand+��5p      �� $ InsertticketView��5r      5s      5u      5v      5w      