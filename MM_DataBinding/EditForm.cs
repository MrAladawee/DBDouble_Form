using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM_DataBinding
{
    public partial class EditForm : Form
    {
        private Group _gr;
        private bool newRow;
        private Exception _bindingException;

        public EditForm(Group gr, bool newRow = false)
        {
            this.newRow = newRow;
            InitializeComponent();
            _gr = gr;

            button1.Text = (newRow ? "Добавить" : "Изменить");

            // Производим связку данных
            // для textbox1 выполняем метод DataBindings
            // добавляем привязку через Add
            // и используем такие параметры:
            // имя свойства, в компоненте textBox1 который будет связываться с каким-то значением ; источник данных (откуда берем данные) ; название свойства внутри класса данных, который связываем с textBox1
            textBox1.DataBindings.Add(nameof(TextBox.Text), _gr, nameof(Group.Num));
            numericUpDown1.DataBindings.Add(nameof(NumericUpDown.Value), _gr, nameof(Group.Year));
            textBox3.DataBindings.Add(nameof(TextBox.Text), _gr, nameof(Group.Spec));
            textBox4.DataBindings.Add(nameof(TextBox.Text), _gr, nameof(Group.Department));
            // Обработка ошибок ввиду создания exception на уровне Group
            var cbBinding = comboBox1.DataBindings.Add(nameof(ComboBox.Text), _gr, nameof(Group.Level));
            cbBinding.FormattingEnabled = true; // включить механизм обработки исключительных ситуаций
            cbBinding.BindingComplete += CbBinding_BindingComplete; // подписываемся на событие
        }
        // Метод обработки события
        private void CbBinding_BindingComplete(object? sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception)
            {
                e.Cancel = false; // событие будет идти до конца
                                  // чтобы работали кнопки добавить и отмена

                //e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate

                comboBox1.BackColor = Color.LightCoral;

            }
            _bindingException = e.Exception;
        }

        private void EditForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                if (_bindingException != null)
                {
                    MessageBox.Show("Указаны неверные данные");
                }
                else
                {
                    if (newRow) DBHelper.GetInstance().InsertNew(_gr); //метод внутри DBHelper, формирующий добавление этой группы в базу
                    else
                    {
                        //DBHelper.GetInstance().Update()
                    }
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch { MessageBox.Show("Ошибка добавления новой записи в базу данных"); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = Color.White;
        }
    }
}
