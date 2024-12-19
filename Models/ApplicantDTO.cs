using System.ComponentModel.DataAnnotations;

namespace LamaranWeb.Models
{
    public class ApplicantDTO
    {
        [Required(ErrorMessage = "Nama Lengkap wajib diisi.")]
        public string NamaLengkap { get; set; }

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "No HP wajib diisi.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "No HP hanya boleh berisi angka.")]
        public string NoHp { get; set; }

        public IFormFile? CV { get; set; }
    }
}

