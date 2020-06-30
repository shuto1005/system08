using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace system08
{
	public partial class MainWindow
	{
        private List<Button> button_list = new List<Button>();
        private List<TextBox> text_list = new List<TextBox>();
        //private List<Win_data> win_data = new List<Win_data>();
        private int button_num = 0;
        private bool enter = false;

        //テキストボックス（優先度入力）を有効にする
        private void setPriority(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            string str = button.Name.Remove(0, 6);
            //str.Replace("button", "");
            if (!int.TryParse(str, out button_num))
            {
                MessageBox.Show(str);
                return;
            }

            for (int i = 0; i < button_list.Count; ++i)
                button_list[i].IsEnabled = false;
            text_list[button_num].IsEnabled = true;
        }

        private void ChangePriority1(object sender, TextCompositionEventArgs e)
        {
            bool flag;
            //入力文字列が２文字以下であるかを判定
            switch (text_list[button_num].Text.Length)
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

        private void ChangePriority2(object sender, KeyEventArgs e)
        {
            int num = button_num;
            if (e.Key == Key.Return && text_list[num].Text.Length >= 1)
            {
                enter = true;
                if (text_list[num].Text[0] == '0' && text_list[num].Text.Length == 2)
                    text_list[num].Text = text_list[num].Text[1] + "";
                text_list[num].IsEnabled = false;

                //優先度を保持
                //win_data[num].SetPriority(int.Parse(text_list[num].Text));
                //優先度選択画面の次に前面のウィンドウの優先度変更

                for (int i = 0; i < button_list.Count; ++i)
                    button_list[i].IsEnabled = true;
            }
        }

        // 貼り付けを許可しない
        private void ChangePriority3(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
