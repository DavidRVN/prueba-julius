using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prueba_julius.Models
{

    [Table("usuarios")]
    public class usuarios
    {
        [Key]
        public int Id { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }

        [Required(ErrorMessage ="El email no puede estar repetido")]      
        public string? email { get; set; }

        [Required(ErrorMessage ="el usuario no puede estar repetido")]
        public string? usuario { get; set; }

        [Required(ErrorMessage = "el usuario no puede estar vacía")]
        public string? contrasena { get; set; }
        public string? celular { get; }
    }
}
