   M S B A M L   `   `   `  ����B  >PracApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:PracAppPracApp  5   	       PracApp.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:PracApp     5   	   _ [Hardcodet.NotifyIcon.Wpf, Version=2.0.0.0, Culture=neutral, PublicKeyToken=682384a853a08aad)tb http://www.hardcodet.net/taskbar  5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)    � WindowStyle$
 None=�5
   
    � AllowsTransparency$
 True��6   .��5      � .+�5   
   �� 5      � 5   
    � RowDefinitions 5      �� 5      $��35 �6    6.   �� 5      6"   .+�5      � -   5        $��0��5      
�������.+�5      ��  �� Orientation$ 
Horizontal=�6   $��Right=�67   .+�5      �� -   5         ��Name$ MiniButt��5      $	��—��6F   
�������$��50��6e      MaterialDesignFlatButton#��[  6p   6�   �� 5      -   5        $ MaxOrNormAppButt��5      
�������$	��❐��6i   $��50��6u   #��[  6�   6�   �� 5      -   5        $ CloseAppButt��5      $
��🞩��6N   
�������$��50��6n   #��[  6y   6�   5      5      � 5       $��1��6   .+�5!       � -   5          �Name$ NavFrame��5!        � NavigationUIVisibility$ Hidden=�6*   5#      5%      5&      57      58      