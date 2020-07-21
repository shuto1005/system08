// C1 UI処理部で更新ボタンが押された場合の処理の実装
// -----
// AL18052 坂本 達哉

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
        /// 【更新ボタン】が押された時に呼び出す
        /// 不要なデータの削除・新規データの登録を行う
        /// <param name="sender">変更ボタンの情報</param>
        /// <param name="e">イベントの情報</param>
        /// </summary>
        public void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            //1.全ウィンドウを取得
            List<wdata> list = priorityModule.GetWindows();
            int[] num = Enumerable.Repeat<int>(-1, list.Count).ToArray();

            //2.破棄されたウィンドウのデータを削除
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

            //3.新規ウィンドウの優先度を設定
            int[] txt_data = Enumerable.Repeat<int>(-1, 10000).ToArray();
            if (history == null)
                history = new List<wdata>();
            for (int i = 0; i < history.Count; ++i)
            {
                if (history[i].id < 0 || history[i].id >= 10000)
                    return;
                txt_data[history[i].id] = history[i].priority;
            }

            //4.新規ウィンドウのデータを登録:void Add(wdata data)
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
                        history.Add(list[i]);
                    }
                    managedData.Add(list[i]);
                }
            }

            //5.ウィンドウを切り替え
            priorityModule.assignPriority(IntPtr.Zero, 100, managedData);
        }
    }
}