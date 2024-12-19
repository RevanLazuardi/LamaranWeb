namespace LamaranWeb.Models
{
    public class Applicant
    {
        public int Id { get; set; }
        public string NamaLengkap { get; set; }
        public string Email { get; set; }
        public string NoHp { get; set; }
        public string CVName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
