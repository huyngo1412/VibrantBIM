   M S B A M L   `   `   `  ����E  AVibrantBIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VibrantBIM.ViewsVibrantBIM.Views  5      )    #VibrantBIM.Views.cPluginEtabsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:VibrantBIM.Views     5      -   5          �Name$  cPluginEtabs��5
      ���	#FFF7F5F5 � WindowStartupLocation$ CenterScreen=�5	       � Title$ Export EDB To RVT��5       � SizeToContent$ WidthAndHeight=�5      .��5      � $��80��6   $	��400��5       � 	Resources 5   
    �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius	   ShadowDepth
   	Direction   Color �� Triggers P� Property P� Value��%�  '��       �� 5      " ��6   .�5      ��  �@J�Effect 5        5      $ 10Q�6+   $	 0Q�6;   $
 -0Q�6K   $ 	#FF9A9AE6��6Z   5      �� 5       � �� �	#FF5791ED6C   �� 5       � ��$ 12(�6-   6:   �� 5       � ��
 �����6?   �� 5       � ��$ 25��6+   68   �� 5       � ��$ 75��6*   67    5      P� 5       �@J�IsMouseOver . . �5      ��  � ��
 �   �6G   5       5"      5#       � ColumnDefinitions 5$   
   �� 5%      6!   �� 5&      $	��100 �6   6-    � RowDefinitions 5(   
   �� 5)      6   �� 5*      $��35 �6   6*   .+�5,   
   �� ���	#FF5791ED$��1q�6)    �� CornerRadius$ 0��6=   $��10,10,-5,10q�6N   ._�5-      �� -   5         ��Name$ tb_Brose��5-      
���   ����	#FFEDE8E86>   5.      �� 5/   
   -   5         ��Name$ 
btn_Browse��52      $��1��5/      $��0��50      $��Browse��51      ��53      �� *	BrowseEDB+��54      ��  �� ElementName$ tb_Brose��6E   �� 55   
   $��1��6   $��0,-5,0,0q�56      $��0��57      $��2��58       �� Orientation$ 
Horizontal=�59      $��Center=�5:      .+�5;      �� -   5        $ 
btn_Export��5;      $��0,0,10,0q�6b   $��Export��6u   ��6D   �� *	ExportEDB+6�   �� 5<      $��10,0,-0.3,0q�6   $��Cancel��6+   6>   5=      5>      5?      