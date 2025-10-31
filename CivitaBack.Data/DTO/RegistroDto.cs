using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CivitaBack.Data.DTO
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")] 
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El formato de correo no es válido.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
