using System.ComponentModel;

namespace MM_DataBinding
{
    public partial class Form1 : Form
    {
        // Привязываемый список
        BindingList<Group> groups = new();
        // Экземпляр DBHelper'а

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            var dbh = DBHelper.GetInstance(
                "localhost",
                3306,
                "root",
                "",
                "Student"
                );
            groups = dbh.GetGroups();
            dataGridView1.DataSource = groups;
            
        }

        private void увеличитьГодНа1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var group in groups)
            {
                group.Year++;
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newGr = new Group()
            {
                Department = "ИММ",
                Year = DateTime.Now.Year
            };

            var ef = new EditForm(newGr, true);

            // Форма закрылась успешно будем получать при нажатии на "добавить"
            // То есть работая в ef и нажмем на "добавить", то перейдем в if на сравнение OK
            if (ef.ShowDialog() == DialogResult.OK)
            {
                // Трай нужен, чтобы в случае появления нового объекта сущ.примари кей ничего не сломалось
                groups.Add(newGr);
                
            }



        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var index = dataGridView1.SelectedRows[0].Index;
                var gr = groups[index];
                var gr_copy = new Group();
                gr.CopyTo(gr_copy);

                var ef = new EditForm(gr_copy, false);
                if (ef.ShowDialog() == DialogResult.OK)
                {
                    // Метод вызывается до копирования
                    //dbh.Update(); метод обновления строки
                    gr_copy.CopyTo(gr);
                }
            }
        }
    }
}