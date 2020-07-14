using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace system08

{
    public partial class MainWindow : Window
    {
        private UIModule module;
        private ObservableCollection<N_Window> NewWindow;

        //-----------------------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
            module = new UIModule();

            NewWindow = new ObservableCollection<N_Window>();

            var nwindow = new N_Window("", 0, 0);
            NewWindow.Add(nwindow);


            listView.ItemsSource = module.managedData;


            bool first = true;
            Activated += (s, e) =>
            {
                if (first)
                {
                    first = false;
                    var helper = new System.Windows.Interop.WindowInteropHelper(this);
                    module.SetThisWindowHandle(helper.Handle);
                    Init_UI();
                }
            };
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            module.OnVolumeChanged(sender, e);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            module.PreviewPriorityText(sender, e);
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            module.PreviewPriorityExecuted(sender, e);

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            module.OnPriorityChanged(sender, e);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            module.Destruct(1);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            module.OnButtonClicked(sender, e);

            bool first = true;
            Activated += (s, e) =>
            {
                if (first)
                {
                    first = false;
                    Init_UI();
                }
            };
            MessageBox.Show("更新完了");
        }
        private void SetPriority(object sender, RoutedEventArgs e)
        {
            module.SetPriority(sender, e);
        }
        private void Init_UI()
        {
            module.text_list = new List<TextBox>();
            module.button_list = new List<Button>();
            int changing = -1;
            bool deadlook = true;
            int i = 0; //←処理対象の行番号を入れる
            for (i = 0; i < module.managedData.Count; ++i)
            {

                var listViewItem = listView.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                if (listViewItem == null)
                {
                    var items = listView.ItemsSource as ObservableCollection<wdata>;
                    listView.ScrollIntoView(items[i]);
                    listViewItem = listView.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;

                    RoutedEventHandler handler = null;
                    handler = (object hs, RoutedEventArgs he) =>
                    {
                        //<< listViewItemに対する処理を実行 >>
                        listViewItem.Loaded -= handler;
                    };
                    //listViewItem.Loaded += handler;
                }
                else
                {
                    //<< listViewItemに対する処理を実行 >>
                    int columnNumber = 1; //←対象の列番号を入れる
                    var rowPresenter = FindVisualChild<GridViewRowPresenter>(listViewItem);
                    var cellPresenter = VisualTreeHelper.GetChild(rowPresenter, columnNumber) as ContentPresenter;
                    var template = rowPresenter.Columns[columnNumber].CellTemplate;
                    //var name = template.FindName("name", cellPresenter) as TextBlock; //コントロールの型;
                    //<< コントロールに対する処理を実行 >> //【name】は上手くいかない
                    var priority = template.FindName("priority", cellPresenter) as TextBox;
                    int k = 0;
                    while (priority == null && k < 100)
                    {
                        priority = template.FindName("priority" + k, cellPresenter) as TextBox;
                        ++k;
                    }
                    if (priority != null)
                    {
                        System.Diagnostics.Trace.WriteLine(i + "番目 : " + module.managedData[i].productName);
                        System.Diagnostics.Trace.WriteLine("priority : " + priority.Text + "!!!!!!!!!!!!!!!!!!!!");
                        priority.Name = "priority" + i;
                        module.text_list.Add(priority);
                        if (priority.IsEnabled)
                        {
                            deadlook = false;
                            changing = i;
                            //System.Diagnostics.Trace.WriteLine("TextBox["+i+"] = True");
                        }
                    }
                    else
                        MessageBox.Show("Not Found : priority" + i);

                    var button = template.FindName("button", cellPresenter) as Button;
                    if (button == null)
                        button = template.FindName("button" + (k - 1), cellPresenter) as Button;
                    if (button != null)
                    {
                        System.Diagnostics.Trace.WriteLine("button : OK");
                        button.Name = "button" + i;
                        module.button_list.Add(button);
                        if (button.IsEnabled)
                        {
                            deadlook = false;
                            //System.Diagnostics.Trace.WriteLine("Button[" + i + "] = True");
                        }
                    }
                    else
                        MessageBox.Show("Not Found : button" + i);



                    columnNumber = 2;
                    cellPresenter = VisualTreeHelper.GetChild(rowPresenter, columnNumber) as ContentPresenter;
                    template = rowPresenter.Columns[columnNumber].CellTemplate;
                    var volume = template.FindName("volume", cellPresenter) as Slider;
                    if (volume == null)
                        volume = template.FindName("volume" + (k - 1), cellPresenter) as Slider;
                    if (volume != null)
                    {
                        System.Diagnostics.Trace.WriteLine("volume : " + volume.Value + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        volume.Name = "volume" + i;
                    }
                    else
                        MessageBox.Show("Not Found : volume" + i);
                }
            }
            if (deadlook)
                for (i = 0; i < module.button_list.Count; ++i)
                    module.button_list[i].IsEnabled = true;
            if (changing == -1)
                return;
            for (i = 0; i < module.button_list.Count; ++i)
                if (i != changing)
                    module.button_list[i].IsEnabled = false;
            //else
            //    module.text_list[i].IsEnabled = true;
        }
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    } // end of MainWindow class

    //***********************************************************************************************
    public class N_Window
    {
        public string ProductName { get; set; }
        public int Priority { get; set; }
        public int Volume { get; set; }

        public N_Window(string productName, int priority, int volume)
        {
            this.ProductName = productName;
            this.Priority = priority;
            this.Volume = volume;
        }

        public override string ToString()
        {
            return this.ProductName + "-" + this.Priority + "-" + this.Volume;
        }
    } // end of N_Window class

} // end of namespace