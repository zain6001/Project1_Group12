   M S B A M L   `   `   `  ����  VisualProject_Group12_WPF*    $VisualProject_Group12_WPF.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Login��5   	   $	��350��6   $	��450��6$   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��Welcome Back! :)��6   $��24(�6,   $
��Bold"�6:   ���#2C3E50$��Center=�5      $��	0,10,0,20q�61   $��0��6D   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Center=�6.   $��1��6K   $��0,5,0,5q�6X   .+�5      �� $��	Username:��6   $
��Bold"�6)   $��5,0,5,5q�6;   �� 5      -   5         ��Name$ UsernameTextBox��5      $	��250��6/   $��30��6;   $��5q�6G   $��Center=�6S   5      �� 5   
   $ 
Horizontal=�6   $��Center=�6.   $��2��6K   $��0,5,0,5q�6X   .+�5      �� $��	Password:��6   $
��Bold"�6)   $��5,0,5,5q�6;   G� 5      -   5         G�Name$ PasswordBox��5      $	��250��6/   $��30��6;   $��5q�6G   $��Center=�6S   5      �� 5"   
   $ 
Horizontal=�6   $��Center=�6.   $��3��6M   $��0,5,0,5q�6Z   .+�5#      �� $��Role:��6   $
��Bold"�6%   $��5,0,5,5q�67   �� 5$      -   5         ��Name$ RoleComboBox��5$      $	��250��6-   $��30��69   $��27,0,0,0q�6E   .@�5%      �� $��Admin��6   �� 5&      $��Manager��6   �� 5'      $��Staff��6   5(      5)      �� 5,   
   $ 
Horizontal=�6   $��Center=�6.   $��4��6K   $��0,20,0,0q�6X   .+�5-      �� -   5         ��Name$ LoginButton��5-   %   $��Login��6   $	��100��6:   $��35��6F   $��14(�5.      $��SemiBold"�6#   ���#3498DB
����������#2980B9$��1q�5/   +    � Cursor$
 Hand��6?   $��	10,0,10,0q�6M   ��50      �� 51      	 �� 
TargetType"	 ��6   .�52      �� 
 �� Property
 � �� �� Value 53      �� 54   "    �� 
TargetType" ��62   .R�55   &   �� #��z��6-    �� CornerRadius$ 5��56   -   #��z��57   -   #��z��58   -   ._�59   *   �� $��Center=�6;   $��Center=�6X   5:   '   5;   #   5=      5>      5@      �� 5A      -   5        $ CreateAccountButton��5A   .   $��Create Account��6   $	��120��6K   $��35��6W   $��14(�5B      $��SemiBold"�6#   ���#2ECC71
����������#27AE60$��1q�5C   +   $
 Hand��6?   $��	10,0,10,0q�6M   ��5D      �� 5E      "	 ��6   .�5F      �� 
 � �� 5G      �� 5H   "   " ��62   .R�5I   &   �� #��z��6-   $ 5��5J   -   #��z��5K   -   #��z��5L   -   ._�5M   *   �� $��Center=�6;   $��Center=�6X   5N   '   5O   #   5Q      5R      5T      5U      5V      5W      