namespace Templanza.Models
{
    public class BlendLike
    {
        public int BlendId { get; set; }
        public Blend? Blend { get; set; }

        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
