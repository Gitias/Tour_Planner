   M S B A M L   `   `   `  ����G  CTourplanner_, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:Tourplanner_.Features.TourViewTourplanner_.Features.TourView  5      -    'Tourplanner_.Features.TourView.TourView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:Tourplanner_.Features.TourView     5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   o� $��0��6   $	��Top=�6   .@�5      m� $��_File��6   .@�5      m� $��Import��6    m� Command 6+   �� *ImportCommand+m� 5      $��Export��6    6+   �� *ExportCommand+5      m� 5      $
��_Pdf��6   .@�5      m� $��
Single PDF��6    6/   �� *TourPdfCommand+m� 5      $��Summarize PDF��6    62   �� *SummarizePdfCommand+5      5      �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��Center=�6V   $��10q�6s   .+�5      �� -   5         ��Name$ SearchTextBox��5      $	��250��6-   $��25��69   �� 5      $��Search��6   $��25��5      $��5,0,0,0q�5       ��5!      �� *SearchCommand+��67   �� *Text+ �� ElementName$ SearchTextBox��5"      � 5$   
   $��2��6   $��10q�6    � ColumnDefinitions 5%      �� 5&      $	��200 �6#   �� 5'      $��* �6#     5)      �� 5*      $
��Auto �6    �� 5+      $��* �6    �� 5,      $
��Auto �6    �� 5-      $��* �6    .+�50      �� $ Vertical=�6   $��0��51      $��0��52      .+�54      �� $	��Add��6   $��60��55      $��5q�56      ��57      �� *AddTourCommand+�� 59      $
��Edit��6   $��60��5:      $��5q�5;      ��5<      �� *EditTourCommand+�� 5>      $��Delete��6   $��60��5?      $��5q�5@      ��5A      �� *DeleteTourCommand+�� 5C      $��Show Favorites��6   $��5q�5E       d� 	IsChecked 5D      �� *ShowFavoritesOnly+5E      5F      �� 5H      $��1��6   $��0��5I      ��5J      �� *Tours+ �� SelectedItem 5K      �� *SelectedTour+��5M      �� 5N      .R�5O      �� ��6$   �� *Name+5P      5S      �� 5U      $��0��6   $��1��5V      $��2��5W      .@�5Y      �� $��General��6   .��5Z      �� $��10q�6!   .+�5[      �� $��Name:��6$   $
��Bold"�61   �� 5\      ��6$   �� *SelectedTour.Name+�� 5^      $��Description:��6$   $
��Bold"�68   �� 5_      ��6$   �� *SelectedTour.Description+�� 5a      $��From:��6$   $
��Bold"�61   �� 5b      ��6$   �� *SelectedTour.From+�� 5d      $	��To:��6$   $
��Bold"�6/   �� 5e      ��6$   �� *SelectedTour.To+�� 5g      $��TransportType:��6$   $
��Bold"�6:   �� 5h      ��6$   �� *SelectedTour.TransportType+�� 5j      $��ChildFriendliness:��6$   $
��Bold"�6>   �� 5k      ��6$   �� *ChildFriendliness+�� 5m      $��Popularity:��6$   $
��Bold"�67   �� 5n      ��6$   �� *
Popularity+�� 5p      $��Favorite��6#   $��0,10,0,0q�5s   #   {�5q   #   �� *SelectedTour+ 5r   #   �� *SelectedTour.IsFavorite+5s   #   5t      5v      �� 5x      $��Route��6   .��5y      �� $��10q�6!   $ Vertical=�6-   .+�5z      �� $��Distance(in m)��6$   $��18(�6:   $
��Bold"�6H   �� 5{      ��6$   �� *RouteDistance+�� 5}      $��Time(in sec)��6$   $��18(�68   $
��Bold"�6F   �� 5~      ��6$   �� *	RouteTime+�� 5�      $
��Fill=�6A   ��6    �� *RouteMapImage+6A   5�      5�      �� 5�      $
��Misc��6   5�      5�      �� 5�      $ 
Horizontal=�6   $��2��5�      $��1��5�      $��Right=�5�      .+�5�      �� $��Add Log��6   $��80��5�      $��5q�5�      ��5�      �� *AddTourLogCommand+�� 5�      $��Edit Log��6   $��80��5�      $��5q�5�      ��5�      �� *EditTourLogCommand+�� 5�      $��
Delete Log��6   $��80��5�      $��5q�5�      ��5�      �� *DeleteTourLogCommand+5�      �� 5�      $��3��6   $��1��5�      ��5�      �� *
TourLogs+ 5�      �� *SelectedTourLog+ �� View 5�      �� 5�      .P�5�      �� $
��Date��6)   	 �� DisplayMemberBinding	 67   �� *Date+�� 5�      $��
Difficulty��6)   	 6=   �� *
Difficulty+�� 5�      $��Distance��6)   	 6;   �� *
Distance+�� 5�      $��	TotalTime��6)   	 6<   �� *	TotalTime+�� 5�      $��Rating��6)   	 69   �� *Rating+�� 5�      $��Comment��6)   	 6:   �� *	Comment+5�      5�      5�      5�      5�      