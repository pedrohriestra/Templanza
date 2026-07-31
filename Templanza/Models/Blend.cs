using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Blend
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Publicado")]
        public bool EsPublicado { get; set; }

        [Display(Name = "Recomendado")]
        public bool EsRecomendado { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<BlendPlanta> BlendPlantas { get; set; } = new List<BlendPlanta>();
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public ICollection<BlendLike> BlendLikes { get; set; } = new List<BlendLike>();
    }
}
