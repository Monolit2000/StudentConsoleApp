namespace StudentConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Get the list of subjects
            var subjects = Subject.Fill();

            // 2. Get the list of students
            var students = Student.Fill();

            // 3. Link subjects to students
            for (int i = 0; i < students.Count; i++)
            {
                // Assign two subjects to each student
                var studentSubjects = subjects.Skip(i * 2).Take(2).ToList();
                foreach (var subject in studentSubjects)
                {
                    subject.StudentId = students[i].Id;
                }
                students[i].SetSubjects(studentSubjects);
            }

            // 4. Calculate the average grade for each student
            // 5. Set the Grant value
            students.ForEach(student =>
            {
                student.CalculateAverageGrade();
                student.SetGrant();
            });

            var selectedStudent = students.FirstOrDefault();

            if (selectedStudent != null)
            {
                Console.WriteLine($"Student: {selectedStudent.FirstName} {selectedStudent.SecondName}, Age: {selectedStudent.Age}");
                Console.WriteLine($"Average Grade: {selectedStudent.AverageGrade}");
                Console.WriteLine($"Grant: {selectedStudent.Grant}");
                Console.WriteLine("Subjects:");
                foreach (var subject in selectedStudent.Subjects)
                {
                    Console.WriteLine($" - {subject.Name}: {subject.Grade} ({subject.Date.ToShortDateString()})");
                }
            }
        }
    }

    public enum Grant
    {
        None,
        Regular,
        Increased
    }

    public class Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid StudentId { get; set; }
        public int Grade { get; set; }
        public DateTime Date { get; set; }

        public static List<Subject> Fill()
        {
            return new List<Subject>
            {
                new Subject { Id = Guid.NewGuid(), Name = "Math", StudentId = Guid.NewGuid(), Grade = 85, Date = DateTime.Now },
                new Subject { Id = Guid.NewGuid(), Name = "Physics", StudentId = Guid.NewGuid(), Grade = 92, Date = DateTime.Now },
                new Subject { Id = Guid.NewGuid(), Name = "Chemistry", StudentId = Guid.NewGuid(), Grade = 78, Date = DateTime.Now },
                new Subject { Id = Guid.NewGuid(), Name = "Biology", StudentId = Guid.NewGuid(), Grade = 65, Date = DateTime.Now },
                new Subject { Id = Guid.NewGuid(), Name = "History", StudentId = Guid.NewGuid(), Grade = 88, Date = DateTime.Now }
            };
        }

        public static List<Subject> GetByStudentId(List<Subject> subjects, Guid studentId)
        {
            return subjects.Where(subject => subject.StudentId == studentId).ToList();
        }
    }

    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public double AverageGrade { get; private set; }
        public Grant Grant { get; private set; }

        public static List<Student> Fill()
        {
            return new List<Student>
            {
                new Student { Id = Guid.NewGuid(), FirstName = "Robert", SecondName = "Paulson", Age = 20 },
                new Student { Id = Guid.NewGuid(), FirstName = "Ryan", SecondName = "Gosling", Age = 22 }
            };
        }

        public void SetSubjects(List<Subject> subjects)
        {
            Subjects = subjects;
        }

        public void CalculateAverageGrade()
        {
            if (Subjects.Any())
            {
                AverageGrade = Subjects.Average(s => s.Grade);
            }
        }

        public void SetGrant()
        {
            if (AverageGrade < 60)
            {
                Grant = Grant.None;
            }
            else if (AverageGrade >= 60 && AverageGrade < 90)
            {
                Grant = Grant.Regular;
            }
            else
            {
                Grant = Grant.Increased;
            }
        }
    }

}
