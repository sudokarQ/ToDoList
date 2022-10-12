using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
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
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string path = $"{Environment.CurrentDirectory}\\todoDataList.json";
        private readonly string pathDone = $"{Environment.CurrentDirectory}\\doneList.json";
        BindingList<ToDoModel> _toDoDataList;
        BindingList<ToDoModel> _DoneList;
        private FileIOService _fileIOServiceToDo;
        private FileIOService _fileIOServiceDone;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOServiceToDo = new FileIOService(path);
            _fileIOServiceDone = new FileIOService(pathDone);

            try
            {
                _toDoDataList = _fileIOServiceToDo.LoadData();
                _DoneList = _fileIOServiceDone.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

            dgToDoList.ItemsSource = _toDoDataList;
            _toDoDataList.ListChanged += _toDoDataList_ListChanged;
            DoneList.ItemsSource = _DoneList;
            _DoneList.ListChanged += _DoneList_ListChanged;
        }

        private void _DoneList_ListChanged(object? sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted
                || e.ListChangedType == ListChangedType.ItemChanged)
            {
                if (e.ListChangedType == ListChangedType.ItemChanged)
                {
                    for (int i = 0; i < _DoneList.Count; i++)
                    {
                        if (!_DoneList[i].IsDone)
                        {
                            _toDoDataList.Add(_DoneList[i]);
                            _DoneList.RemoveAt(i);
                        }
                    }
                }
                try
                {
                    _fileIOServiceDone.SaveData(sender as BindingList<ToDoModel>);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
            }
        }

        private void _toDoDataList_ListChanged(object? sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted
                || e.ListChangedType == ListChangedType.ItemChanged)
            {

                if (e.ListChangedType == ListChangedType.ItemChanged)
                {
                    for (int i = 0; i < _toDoDataList.Count; i++)
                    {
                        if (_toDoDataList[i].IsDone)
                        {
                            _DoneList.Add(_toDoDataList[i]);
                            _toDoDataList.RemoveAt(i);
                        }
                    }
                }
                try
                {
                    _fileIOServiceToDo.SaveData(sender as BindingList<ToDoModel>);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                _toDoDataList.Add(new ToDoModel() { Text = txtBox.Text });
                txtBox.Text = "";
            }
        }
    }
}