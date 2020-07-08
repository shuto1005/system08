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
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            module.OnVolumeChanged(sender,e);
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