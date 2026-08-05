using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    // Receta del foro o recomendada por el Admin (según EsRecomendado).
    public class Blend
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(150, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(1000, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Publicado")]
        public bool EsPublicado { get; set; }

        [Display(Name = "Recomendado")]
        public bool EsRecomendado { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<BlendPlanta> BlendPlantas { get; set; } = new List<BlendPlanta>();
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public ICollection<BlendLike> BlendLikes { get; set; } = new List<BlendLike>();
    }
}
