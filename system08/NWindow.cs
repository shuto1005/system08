using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel; // ObservableCollection
using System.Diagnostics;

namespace EditableListViewTest
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<N_Window> NewWindow;

        //-----------------------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();

            NewWindow = new ObservableCollection<N_Window>();

            var nwindow = new N_Window("", 0, 0);
            NewWindow.Add(nwindow);

            

            listView.ItemsSource = NewWindow;
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