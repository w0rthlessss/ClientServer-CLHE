   M S B A M L   `   `   `  ���� U  QBrightnessAdjustmentClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:BrightnessAdjustmentClientBrightnessAdjustmentClient  +    %BrightnessAdjustmentClient.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     :local(clr-namespace:BrightnessAdjustmentClient       � Title$  
MainWindow��$
��1080��$
��1920�����	#FF595959.���  � ColumnDefinitions �� $
��2.5* ��� $��* � � RowDefinitions �� $
��2.3* ��� $��5* ��� $
��0.7* �.+��� -    ��Name$ SidePanelBorder��$��1��$��3��$��4,0,0,0q�
���   ��� -   $ OpenBtnBorder��$��1��$��2��._�A�  A� Columns$ 2�� A� Rows$ 1��.+��� -   $ OpenImageBtn��
�������$��Center=�$��3q�
���   �$��	40,0,20,0q� �� CornerRadius$ 10�� � Cursor$
 Hand��._��� $��Открыть��$��Center=�$��Center=�$��26(��� -   $ SaveImageBtn��
�������$��Center=�$��3q�
���   �$��	20,0,40,0q�$ 10��._��� $��Сохранить��$��Center=�$��Center=�$��26(��� -   $ 
HistBorder��$��1��$��1��$��0,0,0,4q�
���   �._�]� .+�]� $	��Top=�$��
40,20,40,0q�.+��� $<��6Исходная гистограмма яркости�����#FFFFFF$	��Top=�$��14(��� $J��DНормализованная гистограмма яркости�����	#FF00FF00$��Right=�$
��Left=�$��14(��� -   $ Apply��$��Bottom=�
�������
���   �$��2q�$ 10��$��100,0,100,20q�._��� $ ��Нормализовать��$��Center=�$��14(�]� $��Bottom=�$��40,20,40,20q�.+��� $1��+Параметр ограничения α:��$
��Left=�
�������$��13(��� -    ��Name$ SliderValueTextBox��$��40�����	#FF4A4A4A$��2q�
�������$��Right=�$��5,0,0,0q�$��Right=�$��Center=�$
��Left=�
�������	 �� 
IsReadOnly$
	 True��$	��1.0���� -   
 ��Name$
 AlphaSlider�� � Minimum$ 1Q� � Maximum$ 5Q�$��5q� �� IsSnapToTickEnabled$
 True�� �� TickFrequency$	 0.1Q��� -	    ��Name$ HistogramCanvas�����	#FF6d6d6d$��40,10,40,40q�$��Bottom=��� -
   $ ImagePreviewBorder��$��1��$��0��$��0,0,0,4q�
���   �._��� $��
40,20,40,0q�$	��Top=�.+�]� .+��� 
�������$��14(�$��Имя файла:���� -    ��Name$ 	imageName��
�������$��14(�]� .+��� 
�������$��14(�$��Размер файла:���� -   $ 	imageSize��
�������$��14(�]� .+��� 
�������$��14(�$��Разрешение:���� -   $ imageRes��
�������$��14(�]� .+��� 
�������$��14(�$��Формат цвета:���� -   $ imageColorSize��
�������$��14(��� -   $ About��
�������$��Center=�$��3q�
���   �$��0,20,0,0q�$ 10��$
 Hand��._��� $��О студенте���� -   $ Information��
�������$��Center=�$��3q�
���   �$��0,5,0,0q�$ 10��$
 Hand��._��� $/��)Справочная информация���� -   $ 
MainBorder��$��0��$��3��._��� -   $ 	ImageArea��$��50q����	#FF4A4A4A._��� -    ��Name$ ImagePlaceholder��$��20q�