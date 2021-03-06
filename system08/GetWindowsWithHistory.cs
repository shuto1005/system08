﻿// C1 UI処理部で変更ボタンが押された場合の処理の実装
// -----
// AL18052 坂本 達哉

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace system08
{
    public partial class UIModule
    {
        /// <summary>
        /// 全ウィンドウの情報を保存データと照合して優先度を設定する
        /// <returns>
        /// ウィンドウデータのリスト as List<wdata>.
        /// </returns>
        /// </summary>
        public List<wdata> GetWindowsWithHistory()
        {
            List<wdata> list = priorityModule.GetWindows();

            //1.保存データのハッシュテーブル作成
            int[] txt_data = Enumerable.Repeat<int>(-1, 10000).ToArray();
            if (history == null)
                history = new List<wdata>();
            for (int i = 0; i < history.Count; ++i)
            {
                if (history[i].id < 0 || history[i].id >= 10000)
                    return null;
                txt_data[history[i].id] = history[i].priority;
            }

            //2.新規ウィンドウの登録
            for (int i = 0; i < list.Count && i < 100; ++i) //0～99まで追加する
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
            }
            return list;
        }
    }
}