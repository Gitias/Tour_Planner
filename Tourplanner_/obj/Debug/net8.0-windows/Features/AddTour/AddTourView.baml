   M S B A M L   `   `   `  ����G  CTourplanner_, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:Tourplanner_.Features.AddTourTourplanner_.Features.AddTour  5   	   /    )Tourplanner_.Features.AddTour.AddTourView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:Tourplanner_.Features.AddTour     5   	     � Title$  AddTourView��5   	   $	��450��6   $	��800��6*   .��5	      �  � RowDefinitions 5
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
��Auto �6   .+�5   
   �� $��0��6   $��Name:��5      �� 5   
   $��1��6   ��5      �� *Name+�� 5   
   $��2��6   $��Description:��5      �� 5!   
   $��3��6   " �� VerticalScrollBarVisibility$
 Auto=�5#      ��5"      �� *Description+5#      �� 5%   
   $��4��6   $��From:��5&      �� 5(   
   $��5��6   ��5)   
   �� *From+.��6    ��5*      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5+   -   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6-    �� 
TargetType" ��6    �� 	Resources 5,         CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch !� Visual��%�   &	        !� 5-      $
 Left=�6=   $ Center=�6O   $
 None=�6c    5.      �� 5/   "   $��Land, PLZ + ORT, Adresse��6(   
�������6K   51      52      	 �� Triggers	 53      P� 54      
 P� Property
 � �� �� Empty P� Value# Z 62   . �55      ��  �� Property � �� �� Value# [  6;   56      P� 57      
 � �� 62   Z� . �58      ��  � ��# [  6;   59      P� 5:      
 �@J�IsKeyboardFocused$
 True��6?   . �5;      ��  � ��
 �����6;   5<      5>      .��5?      5@      �� 5B   
   $��6��6   $	��To:��5C      �� 5E   
   $��7��6   ��5F   
   �� *To+.��6   ��5G      1&clr-namespace:System;assembly=mscorlibSystem 5H   -   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6-   " ��6    5I      ��%�   &	        !� 5J      $
 Left=�6=   $ Center=�6O   $
 None=�6c    5K      �� 5L   "   $��Land, PLZ + ORT, Adresse��6(   
�������6K   5N      5O      	 5P      P� 5Q      
 � ��# Z 62   . �5R      ��  � ��# [  6;   5S      P� 5T      
 � �� 62   Z� . �5U      ��  � ��# [  6;   5V      P� 5W      
 �@J�IsKeyboardFocused$
 True��6?   . �5X      ��  � ��
 �����6;   5Y      5[      .��5\      5]      �� 5_   
   $��8��6   $��Transport Type��5`      �� 5b   
   $��9��6    �� Text 5c      �� *TransportType+.@�5d      �� $��Biking��6   �� 5e      $��Hiking��6   �� 5f      $��Running��6   �� 5g      $��Vacation��6   5h      �� 5j   
   $��10��6   $��Create Tour��5k      ��5l      �� *AddTourCommand+5m      5n      