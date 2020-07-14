//制作者：AL18052 坂本達哉

//内部関数：
//  void SetPriority(object sender, RoutedEventArgs e)
//      優先度変更ボタンを押した時に呼び出す
//  void OnPriorityChanged1(object sender, TextCompositionEventArgs e)
//      優先度欄に文字を入力した時に呼び出す
//  void OnPriorityChanged2(object sender, KeyEventArgs e)
//      優先度欄に【Enter】を入力した時に呼び出す
//  void OnPriorityChanged3(object sender, KeyEventArgs e)
//      優先度欄へのコピペを無効にする

//未完成：
//  【button_list, text_list】の読み込み処理が無いため。
//   ボタンや優先度欄の有効/無効の切り替えが出来ない

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
        //優先度変更のButtonのList  Run()で読み込む予定
        public List<Button> button_list = new List<Button>();
        //優先度のTextBoxのList  Run()で読み込む予定
        public List<TextBox> text_list = new List<TextBox>();

        private int button_num = 0; //ボタン識別変数
        private bool enter = false; //Enter識別変数


        /// <summary>
        /// 【優先度変更ボタン】を全て無効にする
        /// 押された【優先度変更ボタン】に対応する【優先度欄】(TextBox)のみを有効にする
        /// 優先度変更の重複を避けるため。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

            for (int i = 0; i < button_list.Count; ++i)
                button_list[i].IsEnabled = false;
            text_list[button_num].IsEnabled = true;///////////////////////////////
        }
        //*/


        /// <summary>
        /// 優先度は数字2文字までを受け付ける
        /// 0文字で【Enter】が押されたらエラー表示する
        /// 2文字を超えるとエラー表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PreviewPriorityText(object sender, TextCompositionEventArgs e)
        {
            ///////////////////////////////////////////////
            var text = sender as TextBox;
            if (text == null)
                return;
            string str = text.Name.Remove(0, 8); //str.Replace("priority", "");
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
            ///////////////////////////////////////////////////////

            bool flag;
            //入力文字列が２文字以下であるかを判定
            //switch (text_list[button_num].Text.Length)
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
        /// 【Enter】が押されたら、呼び出される
        /// 【01】などを【1】に戻す
        /// 【優先度欄】を無効にする
        /// 【優先度変更ボタン】を全て有効にする
        /// 【ウィンドウを更新する関数】を呼び出す
        /// 【変更された優先度】を保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPriorityChanged(object sender, KeyEventArgs e)
        {
            ///////////////////////////////////////////////
            var text = sender as TextBox;
            if (text == null)
            {
                MessageBox.Show("Cannot Change");
                return;
            }
            string str = text.Name.Remove(0, 8); //str.Replace("priority", "");
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
            ///////////////////////////////////////////////

            int num = button_num;
            //if (e.Key == Key.Return && text_list[num].Text.Length >= 1)
            if (e.Key == Key.Return && text.Text.Length >= 1)
            {
                enter = true;
                //if (text_list[num].Text[0] == '0' && text_list[num].Text.Length == 2)
                if (text.Text[0] == '0' && text.Text.Length == 2)
                    //text_list[num].Text = text_list[num].Text[1] + "";
                    text.Text = text.Text[1] + "";
                text_list[num].IsEnabled = false;

                //優先度を保持
                //managedData[num].priority = int.Parse(text_list[num].Text);
                managedData[num].priority = int.Parse(text.Text);

                //ウィンドウ切り替え関数()  ただし優先度選択画面が最前面
                //priorityModule.assignPriority(managedData[num].priority, managedData);
                System.Diagnostics.Trace.WriteLine("push");
                priorityModule.assignPriority(managedData[num].hwnd, managedData[num].priority, managedData);

                //優先度変更ボタンを有効にする
                for (int i = 0; i < button_list.Count; ++i)
                    button_list[i].IsEnabled = true;
            }
        }



        /// <summary>
        /// 貼り付けを許可しない
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PreviewPriorityExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        public void SetThisWindowHandle(IntPtr hWnd)
        {
            priorityModule.this_window_hwnd = hWnd;
        }
    }
}