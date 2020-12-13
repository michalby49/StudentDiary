using LekcjeWindowsForms.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace StudentDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private static string allClassesFinder = "Wszystkie Klasy";

        public bool IsMaximize
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }
        public Main()
        {
            
            InitializeComponent();

            RefreshWindow(allClassesFinder);

            if (IsMaximize)
                WindowState = FormWindowState.Maximized;

            SetColumnsHeader();
            SelectClass(Program.AllClasses);
        }
        private void SelectClass(List<string> allClasses)
        {
            cbbClassFinder.Items.Add(allClassesFinder);
            foreach (var item in allClasses)
            {
                cbbClassFinder.Items.Add(item.ToString());
            }
        }

        public void RefreshWindow(string findClass)
        {
            var students = fileHelper.DeserializeFromFile();
            if (findClass != allClassesFinder)
                students = students.FindAll(x => x.Class == findClass); 

            dgvDiary.DataSource = students;

        }
        public void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Number";
            dgvDiary.Columns[1].HeaderText = "Imie";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy"; 
            dgvDiary.Columns[9].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.Columns[10].HeaderText = "Klasa"; 
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();

            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
            addEditStudent.FormClosing -= AddEditStudent_FormClosing;

        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshWindow(allClassesFinder);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować");
                return;
            }
            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));

            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
            addEditStudent.FormClosing -= AddEditStudent_FormClosing;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz usunąć");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelet =
                MessageBox.Show($"Czy napewno chesz usunąć ucznia " +
                $"{(selectedStudent.Cells[0].Value.ToString() + " " + selectedStudent.Cells[1].Value.ToString()).Trim()}",
                "Usuwanie ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelet == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshWindow(allClassesFinder);
            }
        }

        private void DeleteStudent(int id)
        {
            var students = fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            fileHelper.SerializeToFile(students);
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshWindow(cbbClassFinder.Text);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }
    }
}
