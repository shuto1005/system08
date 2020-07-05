using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel; // ObservableCollection
using System.Diagnostics;

namespace EditableListViewTest
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<NWindow> NewWindow;

        //-----------------------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();

            NewWindow = new ObservableCollection<NWindow>();

            var n_window = new NewWindow("", "", "");
            NewWindow.Add(n_window);

            

            listView.ItemsSource = NewWindow;
        }
    } // end of MainWindow class

    //***********************************************************************************************
    public class NewWindow
    {
        public string ProductName { get; set; }
        public int Priority { get; set; }
        public int Volume { get; set; }

        public NWindow(string productName, int priority, int volume)
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