using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Templanza.Models;

namespace Templanza.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Efecto> Efectos { get; set; } = null!;
        public DbSet<Planta> Plantas { get; set; } = null!;
        public DbSet<PlantaEfecto> PlantaEfectos { get; set; } = null!;
        public DbSet<Blend> Blends { get; set; } = null!;
        public DbSet<BlendPlanta> BlendPlantas { get; set; } = null!;
        public DbSet<Comentario> Comentarios { get; set; } = null!;
        public DbSet<BlendLike> BlendLikes { get; set; } = null!;
        public DbSet<Orden> Ordenes { get; set; } = null!;
        public DbSet<ItemOrden> ItemOrdenes { get; set; } = null!;
        public DbSet<CorreoEnviado> CorreosEnviados { get; set; } = null!;
        public DbSet<ReporteVentasItem> ReporteVentas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ----- Planta -----
            builder.Entity<Planta>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(10,2)");

            builder.Entity<Planta>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Plantas)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- PlantaEfecto (N:N Planta <-> Efecto, con Intensidad) -----
            builder.Entity<PlantaEfecto>()
                .HasKey(pe => new { pe.PlantaId, pe.EfectoId });

            builder.Entity<PlantaEfecto>()
                .HasOne(pe => pe.Planta)
                .WithMany(p => p.PlantaEfectos)
                .HasForeignKey(pe => pe.PlantaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PlantaEfecto>()
                .HasOne(pe => pe.Efecto)
                .WithMany(e => e.PlantaEfectos)
                .HasForeignKey(pe => pe.EfectoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----- Blend -----
            builder.Entity<Blend>()
                .HasOne(b => b.Categoria)
                .WithMany(c => c.Blends)
                .HasForeignKey(b => b.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Blend>()
                .HasOne(b => b.Usuario)
                .WithMany()
                .HasForeignKey(b => b.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Acelera el foro (Index/Recomendados filtran por estas dos columnas).
            builder.Entity<Blend>()
                .HasIndex(b => new { b.EsPublicado, b.EsRecomendado });

            // ----- BlendPlanta (N:N Blend <-> Planta, con Cantidad/Unidad) -----
            builder.Entity<BlendPlanta>()
                .HasKey(bp => new { bp.BlendId, bp.PlantaId });

            builder.Entity<BlendPlanta>()
                .Property(bp => bp.Cantidad)
                .HasColumnType("decimal(10,2)");

            builder.Entity<BlendPlanta>()
                .HasOne(bp => bp.Blend)
                .WithMany(b => b.BlendPlantas)
                .HasForeignKey(bp => bp.BlendId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BlendPlanta>()
                .HasOne(bp => bp.Planta)
                .WithMany(p => p.BlendPlantas)
                .HasForeignKey(bp => bp.PlantaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----- Comentario -----
            builder.Entity<Comentario>()
                .HasOne(c => c.Blend)
                .WithMany(b => b.Comentarios)
                .HasForeignKey(c => c.BlendId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comentario>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- BlendLike (N:N Blend <-> Usuario) -----
            builder.Entity<BlendLike>()
                .HasKey(bl => new { bl.BlendId, bl.UsuarioId });

            builder.Entity<BlendLike>()
                .HasOne(bl => bl.Blend)
                .WithMany(b => b.BlendLikes)
                .HasForeignKey(bl => bl.BlendId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BlendLike>()
                .HasOne(bl => bl.Usuario)
                .WithMany()
                .HasForeignKey(bl => bl.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- Orden / ItemOrden -----
            builder.Entity<Orden>()
                .Property(o => o.Total)
                .HasColumnType("decimal(10,2)");

            builder.Entity<Orden>()
                .Property(o => o.Estado)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Entity<Orden>()
                .HasOne(o => o.Usuario)
                .WithMany()
                .HasForeignKey(o => o.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Acelera el filtro por rango de fechas del reporte de ventas.
            builder.Entity<Orden>()
                .HasIndex(o => o.FechaCreacion);

            builder.Entity<ItemOrden>()
                .Property(io => io.PrecioUnitario)
                .HasColumnType("decimal(10,2)");

            builder.Entity<ItemOrden>()
                .HasOne(io => io.Orden)
                .WithMany(o => o.ItemOrdenes)
                .HasForeignKey(io => io.OrdenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ItemOrden>()
                .HasOne(io => io.Planta)
                .WithMany(p => p.ItemOrdenes)
                .HasForeignKey(io => io.PlantaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- Reporte de ventas (resultado de stored procedure, sin tabla propia) -----
            builder.Entity<ReporteVentasItem>(entity =>
            {
                entity.HasNoKey().ToView(null);
                entity.Property(r => r.TotalVendido).HasColumnType("decimal(12,2)");
            });

            // ----- Data Seeding -----
            builder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Digestivas", Descripcion = "Hierbas que favorecen la digestión" },
                new Categoria { Id = 2, Nombre = "Relajantes", Descripcion = "Hierbas con efecto calmante" },
                new Categoria { Id = 3, Nombre = "Energizantes", Descripcion = "Hierbas estimulantes" },
                new Categoria { Id = 4, Nombre = "Depurativas", Descripcion = "Hierbas depurativas y diuréticas" }
            );

            builder.Entity<Efecto>().HasData(
                new Efecto { Id = 1, Nombre = "Relajante", Descripcion = "Induce calma y reduce el estrés" },
                new Efecto { Id = 2, Nombre = "Digestivo", Descripcion = "Favorece la digestión" },
                new Efecto { Id = 3, Nombre = "Estimulante", Descripcion = "Aumenta la energía y el estado de alerta" },
                new Efecto { Id = 4, Nombre = "Antiinflamatorio", Descripcion = "Reduce inflamación" },
                new Efecto { Id = 5, Nombre = "Diurético", Descripcion = "Favorece la eliminación de líquidos" }
            );

            builder.Entity<Planta>().HasData(
                new Planta
                {
                    Id = 1,
                    NombreComun = "Manzanilla",
                    NombreCientifico = "Matricaria chamomilla",
                    Contraindicaciones = "Alergia a plantas de la familia Asteraceae",
                    ParteUsada = "Flor",
                    Origen = "Europa",
                    Precio = 850m,
                    Stock = 100,
                    CategoriaId = 2
                },
                new Planta
                {
                    Id = 2,
                    NombreComun = "Menta",
                    NombreCientifico = "Mentha spicata",
                    Contraindicaciones = "Reflujo gastroesofágico",
                    ParteUsada = "Hoja",
                    Origen = "Asia",
                    Precio = 700m,
                    Stock = 120,
                    CategoriaId = 1
                },
                new Planta
                {
                    Id = 3,
                    NombreComun = "Jengibre",
                    NombreCientifico = "Zingiber officinale",
                    Contraindicaciones = "Cálculos biliares",
                    ParteUsada = "Raíz",
                    Origen = "Sudeste asiático",
                    Precio = 950m,
                    Stock = 80,
                    CategoriaId = 3
                },
                new Planta
                {
                    Id = 4,
                    NombreComun = "Diente de león",
                    NombreCientifico = "Taraxacum officinale",
                    Contraindicaciones = "Obstrucción de vías biliares",
                    ParteUsada = "Raíz y hoja",
                    Origen = "Europa",
                    Precio = 780m,
                    Stock = 60,
                    CategoriaId = 4
                }
            );

            builder.Entity<PlantaEfecto>().HasData(
                new PlantaEfecto { PlantaId = 1, EfectoId = 1, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 2, EfectoId = 2, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 3, EfectoId = 2, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 3, EfectoId = 3, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 4, EfectoId = 5, Intensidad = 4 }
            );
        }
    }
}
