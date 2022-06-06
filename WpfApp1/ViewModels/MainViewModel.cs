using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Commands;
using System.Text.Json;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly string[] _comboList = { "Name", "Age", "Department" };
        private string _name = String.Empty;
        private int _age = 0;
        private string _dept = String.Empty;
        private ObservableCollection<Student> _studentsList;
        private ObservableCollection<Student> _queryList;
        private string _queryType = String.Empty;
        private ICommand _addCommand;
        private ICommand _queryCommand;
        private ICommand _removeCommand;
        public MainViewModel()
        {
          
            if (!File.Exists("data.json"))
            {
                File.Create("data.json");
                _studentsList = new ObservableCollection<Student>()
                {
                    new Student()
                };
            }
            else if (File.Exists("data.json") )
            {
                try
                {
                    _studentsList =
                        JsonSerializer.Deserialize<ObservableCollection<Student>>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data.json")));
                }
                catch
                {
                    _studentsList = new ObservableCollection<Student>()
                    {
                        new Student()
                    };
                }
            }
           


            _queryList = new ObservableCollection<Student>();
            
        }

        public ObservableCollection<Student> StudentsList
        {
            get => _studentsList;
            set
            {
                if (Equals(value, _studentsList)) return;
                this._studentsList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Student> QueryList => _queryList;
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value == _age) return;
                _age = value;
                OnPropertyChanged();
            }
        }

        public string Dept
        {
            get => _dept;
            set
            {
                if (value == _dept) return;
                _dept = value;
                OnPropertyChanged();
            }
        }

        public string[] ComboList
        {
            get => _comboList;
        }

        public string QueryType
        {
            get => _queryType;
            set
            {
                if (value == _queryType) return;
                _queryType = value;
                OnPropertyChanged();
            }
        }
    
        public ICommand QueryCommand
        {
            get
            {
                if (_queryCommand == null)
                {
                    _queryCommand = new RelayCommand(param => this.SearchBy(param as string),null);
                }

                return _queryCommand;
            }
        }
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(param => this.AddToList(),
                        null);
                }
                return _addCommand;
            }
        }

        public ICommand RemoveCommand2
        {
            get
            {
                if (_removeCommand == null)
                {
                    _removeCommand = new RelayCommand(param => this.RemoveFromList(param),
                        null);
                }
                return _removeCommand;
            }
        }

        private void RemoveFromList(object o)
        {
            
            _studentsList.Remove(o as Student);
            Trace.WriteLine("Command Executed!");
            MoveToJSON();
        }

        private void AddToList()
        {
            _studentsList.Add(new Student
            {
                Name = _name,
                Age = _age
                ,Dept = _dept
            });
            Name = String.Empty;
            Age = 0;
            Dept  = String.Empty;
            MoveToJSON();
        }
        private async void MoveToJSON()
        {
            if (!File.Exists("data.json"))
            {
                await Task.Run(() => { File.Create("data.json"); });
            }
            var x = JsonSerializer.Serialize(_studentsList);
            await Task.Run(
                () => 
                { File.WriteAllText("data.json", x); 
                });

        }
        private void SearchBy(object query)
        {
            _queryList.Clear();
            var query_string = query as string;
            Trace.WriteLine(query);
            if (query_string.Contains("Name"))
            {

                if ((from student in _studentsList
                     where student.Name.Contains(QueryType)
                     select student).Any())
                {
                    using (IEnumerator<Student> x = (from student in _studentsList
                                                     where student.Name.Contains(QueryType)
                                                     select student).GetEnumerator())
                        while (x.MoveNext())
                        {
                            _queryList.Add(new Student
                            {
                                Name=x.Current.Name,
                                Age = x.Current.Age,
                                Dept = x.Current.Dept
                            });
                        }
                }
               

            }
            else if (query_string.Contains("Age"))
            {
                int age = 0;
                try
                {
                    age = Int32.Parse(QueryType);
                }
                catch
                {
                    age = 0;
                }

                if ((from student in _studentsList
                        where student.Age.Equals(age)
                        select student).Any())
                {
                    using (IEnumerator<Student> x = (from student in _studentsList
                               where student.Age.Equals(age)
                               select student).GetEnumerator())
                        while (x.MoveNext())
                        {
                            _queryList.Add(new Student
                            {
                                Name=x.Current.Name,
                                Age = x.Current.Age,
                                Dept = x.Current.Dept
                            });
                        }
                }
            }
            else if (query_string.Contains("Department"))
            {
               
                if ((from student in _studentsList
                        where student.Dept.Contains(QueryType)
                        select student).Any())
                {
                    using (IEnumerator<Student> x = (from student in _studentsList
                               where student.Dept.Contains(QueryType)
                               select student).GetEnumerator())
                        while (x.MoveNext())
                        {
                            _queryList.Add(new Student
                            {
                                Name=x.Current.Name,
                                Age = x.Current.Age,
                                Dept = x.Current.Dept
                            });
                        }
                }
            }
            
        }
    }
}