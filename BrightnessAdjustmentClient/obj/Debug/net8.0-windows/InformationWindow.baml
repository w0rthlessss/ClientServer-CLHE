   M S B A M L   `   `   `  ����U  QBrightnessAdjustmentClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:BrightnessAdjustmentClientBrightnessAdjustmentClient  5   	   2    ,BrightnessAdjustmentClient.InformationWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:BrightnessAdjustmentClient     5   	     � Title$/  )Справочная информация��5   	   $	��720��6'   $
��1280��64   ���	#FF595959 � WindowStartupLocation$ CenterScreen=�6X    � WindowStyle$ 
ToolWindow=�6}    � 
ResizeMode$ NoResize=�6�   .��5	      �� $
��Auto=�6   .��5
   
   �� $��50q�6   $
��Left=�6!   $	��Top=�6<   .+�5      �� $��20(�6   $
��Bold"�6&    �� TextAlignment$ Center=�68   $��0,0,0,20q�6O   
�������{yТема проекта: Нормализация яркости изображений полимерной пленки5      �� 5      $��16(�6   $
��Bold"�6&   $��0,0,0,10q�68   
�������42Использованные технологии:6y   �� 5      $��14(�6   $��	10,0,0,20q�6&   
�������• C#�� 5      	• WPF5      �� 5      $��16(�6   $
��Bold"�6&   $��0,0,0,10q�68   
�������Функционал:6j   �� 5      $��14(�6   $
��Wrap=�6&   $��10,0,0,0q�6:   
�������1. �� 5      $
��Bold"�6   .�6+   Ввод данных68   �� 6=   ca• Загрузка изображения с помощью кнопки «Загрузить»�� 5   E   ��• Выбор параметра ограничения осуществляется с помощью соответствующего слайдера�� 5   b   �� 6n   2. �� 5      $
��Bold"�6   .�6+   0.Информация о изображении6E   �� 6J   • Имя файла�� 5      %#• Размер файла в КБ�� 5   %   WU• Разрешение файла в формате «Ширина x Высота»�� 5   @   42• Формат цвета изображения�� 5    ,   �� 68   3. �� 5!      $
��Bold"�6   .�6+   /-Использование алгоритма6D   �� 6I   ��• Нормализация яркости изображения происходит с помощью кнопки «Нормализовать»�� 5"   `   �� 6l   4. �� 5#      $
��Bold"�6   .�6+   /-Отображение изображений6D   �� 6I   ��• В правой области окна программы отображается как загруженное изображение, так и изображение после применения алгоритма. Следует заметить, что изображения заменяют друг друга, как после загрузки нового изображения, так и после применения алгоритма�� 5%   �   �� 6�   5. �� 5&      $
��Bold"�6   .�6+   :8Построение гистограмм яркости6J   �� 6O   ��• В области сетки после использования алгоритма будут построены гистограммы яркости исходного и преобразованного изображения. Они наложены друг на друга для более наглядного отображения результата работы алгоритма.5)      5*      5-      5.      