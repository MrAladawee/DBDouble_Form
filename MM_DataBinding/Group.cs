using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MM_DataBinding
{
    // Структура класса будет такая же
    // Как и структура таблицы в DB = Student
    public class Group : INotifyPropertyChanged // интервейс использутеся для уведомления каких-то компонентов об изменинии состава свойств
                                                // данный интерфейс необходимо реализовать
    {

        //Создаем поля, к которым будем обращаться через свойства
        // и реализуем геттер, сеттер

        private string _num;
        private int _year;
        private string _spec;
        private string _departument;
        private string _level;

        // Добавляем атрибуты
        // Они работают непосредственно в DataGridView при помощи
        // ComponentModel
        // Они задаются перед полем
        [DisplayName("Номер группы")]
        public string Num { get => _num; set
            {
                _num = value;
                onPorepryChange(nameof(Num));
            }
        } // номер группы
        [DisplayName("Год поступления")]
        public int Year { get => _year; set
            {
                _year = value;
                onPorepryChange(nameof(Year));
            }
        } // год поступления
        [DisplayName("Специальность")]
        public string Spec { get => _spec; set
            {
                _spec = value;
                onPorepryChange(nameof(Spec));
            }
        } // специальность
        [DisplayName("Институт")]
        public string Department { get => _departument; set
            {
                _departument = value;
                onPorepryChange(nameof(Department));
            }
        } // институт
        [DisplayName("Уровень образования")]
        public string Level { get => _level; set
            {
                if (value.ToUpper() != "БАКАЛАВРИАТ" && value.ToUpper() != "МАГИСТРАТУРА" && value.ToUpper() != "СПЕЦИАЛИТЕТ") throw new Exception("Неверный уровень образования");
                _level = value;
                onPorepryChange(nameof(Level));
            }
        } // уровень образования (бак / маг)

        internal void CopyTo(Group gr_copy)
        {
            gr_copy.Num = Num;
            gr_copy.Year = Year;
            gr_copy.Spec = Spec;
            gr_copy.Department = Department;
            gr_copy.Level = Level;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void onPorepryChange([CallerMemberName] string? propertyName = null)
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
