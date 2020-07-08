using System;
using System.Collections.Generic;
using System.Text;

namespace system08
{
    public partial class UIModule
    {
        /// <summary>
        /// GetWindowsから情報を取得し、保存データと照合
        /// </summary>
        /// <returns></returns>
        public List<wdata> GetWindowsWithHistory()
        {
            return priorityModule.GetWindows();
        }
    }
}
