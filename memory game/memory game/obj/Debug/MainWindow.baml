   M S B A M L   `   `   `  ����F  Bmemory game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:memory_gamememory_game  5   	       memory_game.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:memory_game     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��666��6)    � 	Resources 5	      $   memory_game.ImagePathConverter   ImagePathConverter��%   &	          5
   
   6#   5      .��5      � .+�5   
   � -   5         �Name$ 
PagInitial��5      .+�5      �� -   5         ��Name$ WelcomeLabel��5       K� ZIndex$ 3��6*   $'��!Welcome & Enjoy this memory game!��6;   $��Center=�6k   $��0,22,0,0q�6�   $	��Top=�6�   $	��610��6�   $��63��6�   $��36(�6�   �� 5       �� DisplayMemberPath$
 Name��6l   $	��NaN��6�   $��45,101,417,78q�6�   $	��NaN��6�   ��6   �� *	Players+ �� SelectedItem 66   �� *SelectedPlayer+ �� Mode$ TwoWay=�6�   �� 5      $
��Left=�6   $	��170��6/   $��361,101,0,0q�6<   $	��Top=�6Q   $	��203��6i   ��5      �� 5       �� Path$ SelectedPlayer.AvatarPath�6   	 �� UpdateSourceTrigger$	 PropertyChanged=�5      
 �� 	Converter#
 [  5      5      �� 5      -   5         ��Name$ ButtonNewUser��5      $��New User��6,   $
��Left=�6?   $��
64,372,0,0q�6Z   $	��Top=�6n   $��32��6�   $��75��6�   6�   �� 5      -   5        $ ButtonDeleteUser��5      $��Delete User��6/   $
��Left=�6E   $��210,372,0,0q�6`   $	��Top=�6u   $��32��6�   $��75��6�   6�   �� 5      -   5        $ 
ButtonPlay��5      $
��Play��6)   $
��Left=�68   $��361,372,0,0q�6S   $	��Top=�6h   $��32��6�   $��75��6�   6�   �� 5      -   5        $ ButtonCancel��5      $��Cancel��6+   $
��Left=�6<   $��530,372,0,0q�6W   $	��Top=�6l   $��32��6�   $��75��6�   6�   � 5      -   5        $ NewUserGrid��5      
�������$y�Hidden=�6;   .+�5      �� $��Add New User��6   $��Center=�6/   $��	0,122,0,0q�6L   $	��Top=�6_   $��38��6w   $	��126��6�   $��18(�6�   �� 5      -   5         ��Name$ PlayerNameTextBox��5      $��Center=�65   $	��Top=�6R   $��	0,183,0,0q�6j   $	��300��6}   $��25��6�   �� 5      $��Enter your name��6   $
��Left=�62   $��183,157,0,0q�6M   $	��Top=�6b    J� RenderTransformOrigin$ -0.211,0.425&�6z   �� 5       -	   5        $ SelectImageButton��5       $��Select Avatar��64   $
��Left=�6L   $	��Top=�6g   $��183,231,0,0q�6   $	��100��6�   $��32��6�   6�   �� 5!      -
   5        $ ButtonAddUser��5!      $��AddUser��60   $
��Left=�6B   $��183,291,0,0q�6]   $	��Top=�6r   $��33��6�   $	��115��6�   6�   �� 5"      -   5         ��Name$ AvatarImage��5"      $
��Left=�6.   $	��178��6I   $��377,217,0,0q�6V   $	��Top=�6k   $	��197��6�   5#      5$      5%      5&      