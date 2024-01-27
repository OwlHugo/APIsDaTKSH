using System.ComponentModel.DataAnnotations;

namespace APIsDaTKSH.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo não pode ter mais de 100 caracteres.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A mensagem é obrigatória.")]
        public string Message { get; set; }
    }
}
