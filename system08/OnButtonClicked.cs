//制作者：AL18052 坂本達哉

//内部関数：
//  void OnButtonClicked(object sender, RoutedEventArgs e)
//		【新規ウィンドウ取得ボタン】を押した時に呼び出す。

//未完成：
//  

using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace system08
{
    partial class UIModule
    {
        /// <summary>
        /// 【新規ウィンドウ取得ボタン】を押した時に呼び出す。
        /// 【新規ウィンドウ取得関数】を呼び出し
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            //【新規ウィンドウ取得関数】を呼び出し??

            //1.全ウィンドウ取得
            List<wdata> list = priorityModule.GetWindows();
            int[] num = Enumerable.Repeat<int>(-1, list.Count).ToArray();

            //2.破棄されたウィンドウの削除:void Remove(int id)
            for (int j = managedData.Count - 1; j >= 0; --j)
            {
                if (!priorityModule.CheckWindow(managedData[j].hwnd))
                {
                    managedData.RemoveAt(j);
                    continue;
                }

                for (int i = 0; i < list.Count; ++i)
                {
                    if (managedData[j].hwnd == list[i].hwnd)
                    {
                        num[i] = 1;
                        break;
                    }
                }
            }

            //3.新規ウィンドウの優先度設定
            int[] txt_data = Enumerable.Repeat<int>(-1, 10000).ToArray();
            if (history == null)
                history = new List<wdata>();
            for(int i=0;i<history.Count;++i)
            {
                if (history[i].id < 0 || history[i].id >= 10000)
                    return;
                txt_data[history[i].id] = history[i].priority;
            }

            //4.新規ウィンドウの登録:void Add(wdata data)
            for (int i = 0; i < num.Length && i < 100; ++i) //num.Length==list.Count///0～99まで追加する
            {
                if (num[i] == -1)//新規ウィンドウ
                {
                    int check = txt_data[list[i].id];
                    if (check != -1)
                    {
                        list[i].priority = check;   /// 一致したら、SetPriority();  で【priority】を設定
                    }
                    else
                    {
                        list[i].priority = 99 - i;  /// 一致しなかったら、SetPriority(99-i)
                    }
                    managedData.Add(list[i]);
                }
            }

            //5.ウィンドウ切り替え
            priorityModule.assignPriority(99, managedData);

            //6.リストを優先度選択画面に反映
            //MainWindow mw = new MainWindow();
            //mw.Update_UI(managedData,text_list,button_list);
        }
    }

    /*
    partial class MainWindow
    {
        public void Update_UI(ObservableCollection<wdata> managedData,List<TextBox> text_list, List<Button> button_list)
        {
            int i;
            for (i = 0; i < managedData.Count; ++i)
            {
                ////////////////////実際は生成する 
                StackPanel sp = this.FindName("line" + i) as StackPanel;
                if (sp == null)
                    return;
                TextBox name_ui = sp.FindName("name" + i) as TextBox;
                TextBox priority_ui = sp.FindName("priority" + i) as TextBox;
                Button button_ui = sp.FindName("button" + i) as Button;
                Slider volume_ui = sp.FindName("volume" + i) as Slider;
                TextBox muted_ui = sp.FindName("muted" + i) as TextBox;
                if (name_ui != null)
                {
                    name_ui.Text = managedData[i].productName;
                }
                if (priority_ui != null)
                {
                    priority_ui.Text = managedData[i].priority.ToString();
                    priority_ui.IsEnabled = false;
                    text_list.Add(priority_ui);
                }
                if (button_ui != null)
                {
                    button_ui.IsEnabled = true;
                    button_list.Add(button_ui);
                }
                if (muted_ui != null)
                {
                    if (managedData[i].volume > 0 && volume_ui != null)
                    {
                        muted_ui.Visibility = Visibility.Hidden;
                        volume_ui.Value = managedData[i].volume;
                    }
                    else
                        muted_ui.Visibility = Visibility.Visible;
                }
                //テンプレートを使う場合
                //var border = TestButton.Template.FindName("TestBorder", TestButton) as Border;
                //if (null != border)
                //    border.BorderThickness = new Thickness(1);
            }
        }
    }
    //*/
}