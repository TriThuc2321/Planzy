   M S B A M L   `   `   `  ����A  =Planzy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Planzy.ViewsPlanzy.Views  5   	   5clr-namespace:Planzy.ViewModelsPlanzy.ViewModels  5   R   '    !Planzy.Views.InternetCheckingView   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Planzy.Views     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   0
viewmodelsclr-namespace:Planzy.ViewModels  6R   -   5        $��InternetChecking��5   	     � 
ResizeMode$  NoResize=�5	   	    � WindowStyle$
 None=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	   
�����F�   MaterialDesignFont#���   5   	    � Title$ InternetCheckingView��5   	   $��80��6&   $	��200��62   .��5      � *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5   
   +  %Microsoft.Xaml.Behaviors.EventTrigger  5         	EventName$ Loaded��6   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 5      �� *LoadWindowCommand+   CommandParameter 5      �� 	 �� ElementName$	 InternetChecking��5      
 � RowDefinitions
 5   
   �� 5      $��10 �6   �� 5      �� 5      .+�5!   
   � $��1��6   .+�5"      ��  �� Orientation$ 
Horizontal=�6   $��Center=�5#      $��Center=�5$      .+�5%      �  % !MaterialDesignCircularProgressBar#��[ 5&       � Value$ 0Q�5'       � IsIndeterminate$
 True��5(      
�������$��Center=�5*      $��Center=�5+      �� 5.      $$��Không có kết nối Interet��5/      
�������$��Italic$�51      $��20��52      $��15 0 0 0q�53      y�54      �� *TextVisibility+57      5:      � 5<   
   $��2��6   .+�5=      �� $ 
Horizontal=�6   $��0 5 0 0q�62   .+�5>      �� 
�������$��Thử lại��60     MaterialDesignRaisedButton#��[ 5?      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5@      $	��Top=�5A      $��70��5B      $��20��5C      $��20 0 0 0q�5D      $��10(�5E      ��5F      �� *TryAgainCommand+��5G      �� $	 InternetChecking���� 5J      
�������$��Đóng��60   #��[ 5K      $ 5��5L      $	��Top=�5M      $��70��5N      $��20��5O      $��20 0 0 0q�5P      $��10(�5Q      ��5R      �� *ExitCommand+��5S      �� $	 InternetChecking��5U      5W      5Y      5Z      