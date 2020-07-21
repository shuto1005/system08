// C1 UI処理部で変更ボタンが押された場合の処理の実装
// -----
// AL18052 坂本 達哉

//内部関数：
//  void SetPriority(object sender, RoutedEventArgs e)
//      優先度変更ボタンを押した時に呼び出す
//  void OnPriorityChanged1(object sender, TextCompositionEventArgs e)
//      優先度欄に文字を入力した時に呼び出す
//  void OnPriorityChanged2(object sender, KeyEventArgs e)
//      優先度欄に【Enter】を入力した時に呼び出す
//  void OnPriorityChanged3(object sender, KeyEventArgs e)
//      優先度欄へのコピペを無効にする

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using system08;

namespace system08
{
    partial class UIModule
    {
        //優先度変更のButtonのList
        public List<Button> button_list = new List<Button>();
        //優先度のTextBoxのList
        public List<TextBox> text_list = new List<TextBox>();

        private int button_num = 0; //ボタン識別変数
        private bool enter = false; //Enter識別変数


        /// <summary>
        /// 優先度欄への入力を許可する
        /// <param name="sender">更新ボタンの情報</param>
        /// <param name="e">イベントの情報</param>
        /// </summary>
        public void SetPriority(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            string str = button.Name.Remove(0, 6); //str.Replace("button", "");
            if (!int.TryParse(str, out button_num))
            {
                MessageBox.Show("Not Integer : " + str);
                return;
            }
            if (!priorityModule.CheckWindow(managedData[button_num].hwnd))
            {
                MessageBox.Show("Removed : " + str);
                return;
            }
            //【変更ボタン】を全て無効にする
            for (int i = 0; i < button_list.Count; ++i)
                button_list[i].IsEnabled = false;
            //押された【変更ボタン】に対応する【優先度欄】のみを有効にする
            text_list[button_num].IsEnabled = true;
        }

        /// <summary>
        /// 優先度欄への入力を数字2文字まで受け付ける
        /// 誤入力の場合はエラーを表示する
        /// <param name="sender">優先度欄の情報</param>
        /// <param name="e">イベントの情報</param>
        /// </summary>
        public void PreviewPriorityText(object sender, TextCompositionEventArgs e)
        {
            var text = sender as TextBox;
            if (text == null)
                return;
            string str = text.Name.Remove(0, 8);
            if (!int.TryParse(str, out button_num))
            {
                MessageBox.Show("Not Integer : " + str);
                return;
            }

            if (!priorityModule.CheckWindow(managedData[button_num].hwnd))
            {
                MessageBox.Show("Removed : " + str);
                return;
            }

            bool flag;
            //入力文字列が２文字以下であるかを判定
            switch (text.Text.Length)
            {
                case 0:
                    flag = !new Regex("[0-9]").IsMatch(e.Text);
                    if (flag) //新たに入力された１文字が[0-9]でない場合、エラー表示
                    {
                        MessageBox.Show("[0～99]の数値を入力して下さい");
                    }
                    e.Handled = flag;
                    break;
                case 1:
                    flag = !(new Regex("[0-9]").IsMatch(e.Text) | enter);
                    if (flag) //新たに入力された１文字が[0-9]でも[Enter]でもない場合、エラー表示
                    {
                        MessageBox.Show("[0～99]の数値、または、[Enter]を入力して下さい");
                    }
                    e.Handled = flag;
                    enter = false;
                    break;
                default: // ２文字以上なら読み込まない
                    if (!enter)
                        MessageBox.Show("[0～99]の数値を超えてしまいます");
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// 優先度欄への入力時に【Enter】が押されたら、呼び出される
        /// 変更された優先度を保存して反映する
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// </summary>
        public void OnPriorityChanged(object sender, KeyEventArgs e)
        {
            var text = sender as TextBox;
            if (text == null)
            {
                MessageBox.Show("Cannot Change");
                return;
            }
            string str = text.Name.Remove(0, 8);
            if (!int.TryParse(str, out button_num))
            {
                MessageBox.Show("Not Integer : " + str);
                return;
            }

            if (!priorityModule.CheckWindow(managedData[button_num].hwnd))
            {
                MessageBox.Show("Removed : " + str);
                return;
            }

            int num = button_num;
            if (e.Key == Key.Return && text.Text.Length >= 1)
            {
                enter = true;
                //【01】などの文字列を【1】に正す
                if (text.Text[0] == '0' && text.Text.Length == 2)
                    text.Text = text.Text[1] + "";
                //入力完了した【優先度欄】を無効にする
                text_list[num].IsEnabled = false;

                //【変更された優先度】を保持する
                managedData[num].priority = int.Parse(text.Text);

                //【変更された優先度】を保存する 
                if (history == null)
                    history = new List<wdata>();
                bool exist = false;
                for (int i = 0; i < history.Count; ++i)
                {
                    if (history[i].id == managedData[num].id)
                    {
                        exist = true;
                        history[i] = managedData[num];
                        break;
                    }
                }       
                if (!exist)
                    history.Add(managedData[num]);

                //ウィンドウ切り替え関数を呼び出す
                System.Diagnostics.Trace.WriteLine("push");
                priorityModule.assignPriority(managedData[num].hwnd, managedData[num].priority, managedData);

                //全ての【変更ボタン】を有効にする
                for (int i = 0; i < button_list.Count; ++i)
                    button_list[i].IsEnabled = true;
            }
        }

        /// <summary>
        /// 入力時に【貼り付け】を許可させない
        /// <param name="sender">優先度欄の情報</param>
        /// <param name="e">イベントの情報</param>
        /// </summary>
        public void PreviewPriorityExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// このウィンドウ自身のハンドルを保持する
        /// <param name="hWnd">このウィンドウ自身のハンドル</param>
        /// </summary>
        public void SetThisWindowHandle(IntPtr hWnd)
        {
            priorityModule.this_window_hwnd = hWnd;
        }
    }
}