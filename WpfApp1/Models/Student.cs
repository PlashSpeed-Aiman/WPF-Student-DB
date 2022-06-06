using System;

namespace WpfApp1.Models
{
    public class Student
    {
        private string name = "Default";
        private int age = 0;
        private string dept = "Engineering";

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Age
        {
            get => age;
            set => age = value;
        }

        public string Dept
        {
            get => dept;
            set => dept = value;
        }
    }
}