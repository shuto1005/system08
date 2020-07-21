// TextBoxのVisibilityを切り替える関数の実装
// -----
// AL18058 重田悠登

using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows;

namespace system.WPF
{
    /// <summary>
    /// URLの参照
    /// </summary>
    /// <see>http://www.kanazawa-net.ne.jp/~pmansato/wpf/wpf_custom_ListView.htm</see>

    public class BooleanVisiblityConverter : IValueConverter
    {

        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool param = this.GetConverterParameter(parameter);
            bool selected = (bool)value;

            return param == selected ? Visibility.Visible : Visibility.Collapsed;
        }

        //---------------------------------------------------------------------------------------------
   
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not Implemented");
        }

        //---------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool GetConverterParameter(object parameter)
        {
            bool result = false;

            try
            {
                if (parameter != null)
                    result = System.Convert.ToBoolean(parameter);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return result;
        }
    }
}
