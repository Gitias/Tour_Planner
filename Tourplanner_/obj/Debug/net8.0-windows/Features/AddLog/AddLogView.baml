   M S B A M L   `   `   `  ����G  CTourplanner_, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Tourplanner_.Features.AddLogTourplanner_.Features.AddLog  5   	   -    'Tourplanner_.Features.AddLog.AddLogView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:Tourplanner_.Features.AddLog     5   	     � Title$  
AddLogView��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Date:��5      (  "System.Windows.Controls.DatePicker  5   
   $��1��6      SelectedDate 5      �� *Date+�� 5    
   $��2��6   $��Comment:��5!      �� 5#   
   $��3��6   " �� VerticalScrollBarVisibility$
 Auto=�5%      ��5$      �� *	Comment+5%      �� 5'   
   $��4��6   $��
Difficulty��5(      �� 5*   
   $��5��6    �� Text 5+      �� *
Difficulty+.@�5,      �� $��1��6   �� 5-      $��2��6   �� 5.      $��3��6   �� 5/      $��4��6   �� 50      $��5��6   51      �� 53   
   $��6��6   $��Total Distance:��54      �� 56   
   $��7��6   ��57      �� *
Distance+�� 59   
   $��8��6   $��Total Time:��5:      �� 5<   
   $��9��6   ��5=      �� *	TotalTime+�� 5?   
   $��10��6   $��Rating:��5@      �� 5B   
   $��11��6    5C      �� *Rating+.@�5E      �� $��1��6   �� 5F      $��2��6   �� 5G      $��3��6   �� 5H      $��4��6   �� 5I      $��5��6   5K      �� 5M   
   $��12��6   $��
Create Log��5N      ��5O      �� *AddLogCommand+5P      5Q      