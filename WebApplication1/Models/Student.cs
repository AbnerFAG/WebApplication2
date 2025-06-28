namespace WebApplication1.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int GradeId { get; set; }
        public Grade Grade { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
