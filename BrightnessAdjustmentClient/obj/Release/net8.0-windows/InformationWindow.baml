   M S B A M L   `   `   `  ���� U  QBrightnessAdjustmentClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:BrightnessAdjustmentClientBrightnessAdjustmentClient  2    ,BrightnessAdjustmentClient.InformationWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     :local(clr-namespace:BrightnessAdjustmentClient       � Title$/  )Справочная информация��$	��720��$
��1280�����	#FF595959 � WindowStartupLocation$ CenterScreen=� � WindowStyle$ 
ToolWindow=� � 
ResizeMode$ NoResize=�.���� $
��Auto=�.���� $��50q�$
��Left=�$	��Top=�.+��� $��20(�$
��Bold"� �� TextAlignment$ Center=�$��0,0,0,20q�
�������{yТема проекта: Нормализация яркости изображений полимерной пленки�� $��16(�$
��Bold"�$��0,0,0,10q�
�������42Использованные технологии:�� $��14(�$��	10,0,0,20q�
�������• C#�� 	• WPF�� $��16(�$
��Bold"�$��0,0,0,10q�
�������Функционал:�� $��14(�$
��Wrap=�$��10,0,0,0q�
�������1. �� $
��Bold"�.�Ввод данных�� ca• Загрузка изображения с помощью кнопки «Загрузить»�� ��• Выбор параметра ограничения осуществляется с помощью соответствующего слайдера�� �� 2. �� $
��Bold"�.�0.Информация о изображении�� • Имя файла�� %#• Размер файла в КБ�� WU• Разрешение файла в формате «Ширина x Высота»�� 42• Формат цвета изображения�� �� 3. �� $
��Bold"�.�/-Использование алгоритма�� ��• Нормализация яркости изображения происходит с помощью кнопки «Нормализовать»�� �� 4. �� $
��Bold"�.�/-Отображение изображений�� ��• В правой области окна программы отображается как загруженное изображение, так и изображение после применения алгоритма. Следует заметить, что изображения заменяют друг друга, как после загрузки нового изображения, так и после применения алгоритма�� �� 5. �� $
��Bold"�.�:8Построение гистограмм яркости�� ��• В области сетки после использования алгоритма будут построены гистограммы яркости исходного и преобразованного изображения. Они наложены друг на друга для более наглядного отображения результата работы алгоритма.