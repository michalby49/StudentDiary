using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentDiary
{
    public partial class AddEditStudent : Form 
    {
        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        private int _studentId;
        private Student _student;

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            _studentId = id;

            GetStudentData();

            SelectClass(Program.AllClasses);
        }

        private void SelectClass(List<string> allClasses)
        {
            foreach (var item in allClasses)
            {
                cbbClass.Items.Add(item.ToString());
            }
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie ucznia";

                var students = fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak uzytkownika o padanym ID");

                FillTextBoxes();
            }
            tbName.Select();
        }

        private void FillTextBoxes()
        {
            tbID.Text = _student.Id.ToString();
            tbName.Text = _student.FirstName.ToString();
            tbLastName.Text = _student.LastName.ToString();
            tbPhysice.Text = _student.Physics.ToString();
            tbPolishLang.Text = _student.PolishLang.ToString();
            rtbCommends.Text = _student.Commend.ToString();
            tbEnglishlang.Text = _student.EnglishLang.ToString();
            tbMath.Text = _student.Math.ToString();
            tbTechnology.Text = _student.Technology.ToString();
            ckbActivieties.Checked = _student.Activities;
            
        }
        
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssingIdToNewStudent(students);

            AddNewUserToList(students);
                              
            fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student()
            {
                Id = _studentId,
                FirstName = tbName.Text,
                LastName = tbLastName.Text,
                Commend = rtbCommends.Text,
                Math = tbMath.Text,
                Physics = tbPhysice.Text,
                Technology = tbTechnology.Text,
                PolishLang = tbPolishLang.Text,
                EnglishLang = tbEnglishlang.Text,
                Activities = ckbActivieties.Checked,
                Class = cbbClass.Text
            };

            students.Add(student);
        }
        private void AssingIdToNewStudent(List<Student> students)
        {
            var studentsWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentsWithHighestId == null ? 1 : studentsWithHighestId.Id + 1;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
