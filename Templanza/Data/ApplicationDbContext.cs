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
                new Categoria { Id = 4, Nombre = "Depurativas", Descripcion = "Hierbas depurativas y diuréticas" },
                new Categoria { Id = 5, Nombre = "Florales y aromáticas", Descripcion = "Flores e infusiones aromáticas" },
                new Categoria { Id = 6, Nombre = "Inmunológicas", Descripcion = "Hierbas que refuerzan las defensas" }
            );

            builder.Entity<Efecto>().HasData(
                new Efecto { Id = 1, Nombre = "Relajante", Descripcion = "Induce calma y reduce el estrés" },
                new Efecto { Id = 2, Nombre = "Digestivo", Descripcion = "Favorece la digestión" },
                new Efecto { Id = 3, Nombre = "Estimulante", Descripcion = "Aumenta la energía y el estado de alerta" },
                new Efecto { Id = 4, Nombre = "Antiinflamatorio", Descripcion = "Reduce inflamación" },
                new Efecto { Id = 5, Nombre = "Diurético", Descripcion = "Favorece la eliminación de líquidos" },
                new Efecto { Id = 6, Nombre = "Inmunoestimulante", Descripcion = "Refuerza las defensas del organismo" },
                new Efecto { Id = 7, Nombre = "Antioxidante", Descripcion = "Ayuda a combatir el daño celular" }
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
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/manzanilla.webp"
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
                    CategoriaId = 1,
                    ImagenUrl = "/images/plantas/menta.webp"
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
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/jengibre.webp"
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
                    CategoriaId = 4,
                    ImagenUrl = "/images/plantas/diente-de-leon.webp"
                },
                new Planta
                {
                    Id = 5,
                    NombreComun = "Té verde",
                    NombreCientifico = "Camellia sinensis",
                    Contraindicaciones = "Sensibilidad a la cafeína, insomnio",
                    ParteUsada = "Hoja",
                    Origen = "China",
                    Precio = 1100m,
                    Stock = 140,
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/te-verde.webp"
                },
                new Planta
                {
                    Id = 6,
                    NombreComun = "Té negro",
                    NombreCientifico = "Camellia sinensis",
                    Contraindicaciones = "Sensibilidad a la cafeína, hipertensión",
                    ParteUsada = "Hoja",
                    Origen = "China",
                    Precio = 1050m,
                    Stock = 130,
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/te-negro.webp"
                },
                new Planta
                {
                    Id = 7,
                    NombreComun = "Té blanco",
                    NombreCientifico = "Camellia sinensis",
                    Contraindicaciones = "Sensibilidad a la cafeína",
                    ParteUsada = "Brote y hoja joven",
                    Origen = "China",
                    Precio = 1350m,
                    Stock = 70,
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/te-blanco.webp"
                },
                new Planta
                {
                    Id = 8,
                    NombreComun = "Té oolong",
                    NombreCientifico = "Camellia sinensis",
                    Contraindicaciones = "Sensibilidad a la cafeína",
                    ParteUsada = "Hoja",
                    Origen = "China",
                    Precio = 1200m,
                    Stock = 90,
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/te-oolong.webp"
                },
                new Planta
                {
                    Id = 9,
                    NombreComun = "Rooibos",
                    NombreCientifico = "Aspalathus linearis",
                    Contraindicaciones = "Sin contraindicaciones relevantes conocidas",
                    ParteUsada = "Hoja",
                    Origen = "Sudáfrica",
                    Precio = 900m,
                    Stock = 110,
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/rooibos.webp"
                },
                new Planta
                {
                    Id = 10,
                    NombreComun = "Hibisco",
                    NombreCientifico = "Hibiscus sabdariffa",
                    Contraindicaciones = "Embarazo, presión arterial baja",
                    ParteUsada = "Flor (cáliz)",
                    Origen = "África",
                    Precio = 820m,
                    Stock = 100,
                    CategoriaId = 4,
                    ImagenUrl = "/images/plantas/hibisco.webp"
                },
                new Planta
                {
                    Id = 11,
                    NombreComun = "Lavanda",
                    NombreCientifico = "Lavandula angustifolia",
                    Contraindicaciones = "Somnolencia, combinar con sedantes",
                    ParteUsada = "Flor",
                    Origen = "Región mediterránea",
                    Precio = 880m,
                    Stock = 90,
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/lavanda.webp"
                },
                new Planta
                {
                    Id = 12,
                    NombreComun = "Anís estrella",
                    NombreCientifico = "Illicium verum",
                    Contraindicaciones = "Uso excesivo en niños pequeños",
                    ParteUsada = "Fruto",
                    Origen = "China",
                    Precio = 990m,
                    Stock = 65,
                    CategoriaId = 1,
                    ImagenUrl = "/images/plantas/anis-estrella.webp"
                },
                new Planta
                {
                    Id = 13,
                    NombreComun = "Canela",
                    NombreCientifico = "Cinnamomum verum",
                    Contraindicaciones = "Embarazo en altas dosis, trastornos hepáticos",
                    ParteUsada = "Corteza",
                    Origen = "Sri Lanka",
                    Precio = 850m,
                    Stock = 120,
                    CategoriaId = 1,
                    ImagenUrl = "/images/plantas/canela.webp"
                },
                new Planta
                {
                    Id = 14,
                    NombreComun = "Cardamomo",
                    NombreCientifico = "Elettaria cardamomum",
                    Contraindicaciones = "Cálculos biliares",
                    ParteUsada = "Semilla",
                    Origen = "India",
                    Precio = 1150m,
                    Stock = 55,
                    CategoriaId = 1,
                    ImagenUrl = "/images/plantas/cardomomo.webp"
                },
                new Planta
                {
                    Id = 15,
                    NombreComun = "Clavo de olor",
                    NombreCientifico = "Syzygium aromaticum",
                    Contraindicaciones = "Trastornos de la coagulación",
                    ParteUsada = "Botón floral",
                    Origen = "Indonesia",
                    Precio = 980m,
                    Stock = 60,
                    CategoriaId = 1,
                    ImagenUrl = "/images/plantas/clavo-de-olor.webp"
                },
                new Planta
                {
                    Id = 16,
                    NombreComun = "Cúrcuma",
                    NombreCientifico = "Curcuma longa",
                    Contraindicaciones = "Cálculos biliares, anticoagulantes",
                    ParteUsada = "Rizoma",
                    Origen = "India",
                    Precio = 920m,
                    Stock = 100,
                    CategoriaId = 4,
                    ImagenUrl = "/images/plantas/curcuma.webp"
                },
                new Planta
                {
                    Id = 17,
                    NombreComun = "Regaliz",
                    NombreCientifico = "Glycyrrhiza glabra",
                    Contraindicaciones = "Hipertensión, embarazo",
                    ParteUsada = "Raíz",
                    Origen = "Europa y Asia",
                    Precio = 860m,
                    Stock = 70,
                    CategoriaId = 1,
                    ImagenUrl = "/images/plantas/regaliz.webp"
                },
                new Planta
                {
                    Id = 18,
                    NombreComun = "Melisa (Toronjil)",
                    NombreCientifico = "Melissa officinalis",
                    Contraindicaciones = "Hipotiroidismo",
                    ParteUsada = "Hoja",
                    Origen = "Europa",
                    Precio = 780m,
                    Stock = 95,
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/melisa.webp"
                },
                new Planta
                {
                    Id = 19,
                    NombreComun = "Valeriana",
                    NombreCientifico = "Valeriana officinalis",
                    Contraindicaciones = "Combinar con sedantes o alcohol",
                    ParteUsada = "Raíz",
                    Origen = "Europa y Asia",
                    Precio = 940m,
                    Stock = 60,
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/valeriana.webp"
                },
                new Planta
                {
                    Id = 20,
                    NombreComun = "Pasiflora",
                    NombreCientifico = "Passiflora incarnata",
                    Contraindicaciones = "Embarazo, combinar con sedantes",
                    ParteUsada = "Hoja y flor",
                    Origen = "América",
                    Precio = 830m,
                    Stock = 75,
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/pasiflora.webp"
                },
                new Planta
                {
                    Id = 21,
                    NombreComun = "Tilo",
                    NombreCientifico = "Tilia platyphyllos",
                    Contraindicaciones = "Uso prolongado sin supervisión",
                    ParteUsada = "Flor",
                    Origen = "Europa",
                    Precio = 750m,
                    Stock = 100,
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/tilo.webp"
                },
                new Planta
                {
                    Id = 22,
                    NombreComun = "Boldo",
                    NombreCientifico = "Peumus boldus",
                    Contraindicaciones = "Obstrucción de vías biliares, embarazo",
                    ParteUsada = "Hoja",
                    Origen = "Chile",
                    Precio = 800m,
                    Stock = 65,
                    CategoriaId = 1,
                    ImagenUrl = "/images/plantas/boldo.webp"
                },
                new Planta
                {
                    Id = 23,
                    NombreComun = "Cedrón",
                    NombreCientifico = "Aloysia citrodora",
                    Contraindicaciones = "Sin contraindicaciones relevantes conocidas",
                    ParteUsada = "Hoja",
                    Origen = "Sudamérica",
                    Precio = 720m,
                    Stock = 85,
                    CategoriaId = 2,
                    ImagenUrl = "/images/plantas/cedron.webp"
                },
                new Planta
                {
                    Id = 24,
                    NombreComun = "Ortiga",
                    NombreCientifico = "Urtica dioica",
                    Contraindicaciones = "Insuficiencia renal o cardíaca",
                    ParteUsada = "Hoja",
                    Origen = "Europa",
                    Precio = 690m,
                    Stock = 80,
                    CategoriaId = 4,
                    ImagenUrl = "/images/plantas/ortiga.webp"
                },
                new Planta
                {
                    Id = 25,
                    NombreComun = "Equinácea",
                    NombreCientifico = "Echinacea purpurea",
                    Contraindicaciones = "Enfermedades autoinmunes",
                    ParteUsada = "Raíz y flor",
                    Origen = "Norteamérica",
                    Precio = 1000m,
                    Stock = 55,
                    CategoriaId = 6,
                    ImagenUrl = "/images/plantas/equinacea.webp"
                },
                new Planta
                {
                    Id = 26,
                    NombreComun = "Romero",
                    NombreCientifico = "Rosmarinus officinalis",
                    Contraindicaciones = "Embarazo en altas dosis, epilepsia",
                    ParteUsada = "Hoja",
                    Origen = "Región mediterránea",
                    Precio = 760m,
                    Stock = 110,
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/romero.webp"
                },
                new Planta
                {
                    Id = 27,
                    NombreComun = "Jazmín",
                    NombreCientifico = "Jasminum officinale",
                    Contraindicaciones = "Sin contraindicaciones relevantes conocidas",
                    ParteUsada = "Flor",
                    Origen = "Asia",
                    Precio = 950m,
                    Stock = 60,
                    CategoriaId = 5,
                    ImagenUrl = "/images/plantas/jazmin.webp"
                },
                new Planta
                {
                    Id = 28,
                    NombreComun = "Rosa mosqueta (Escaramujo)",
                    NombreCientifico = "Rosa canina",
                    Contraindicaciones = "Cálculos renales",
                    ParteUsada = "Fruto",
                    Origen = "Europa",
                    Precio = 1080m,
                    Stock = 50,
                    CategoriaId = 6,
                    ImagenUrl = "/images/plantas/rosa-mosqueta.webp"
                },
                new Planta
                {
                    Id = 29,
                    NombreComun = "Ginseng",
                    NombreCientifico = "Panax ginseng",
                    Contraindicaciones = "Hipertensión, insomnio",
                    ParteUsada = "Raíz",
                    Origen = "Asia",
                    Precio = 1450m,
                    Stock = 40,
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/ginseng.webp"
                },
                new Planta
                {
                    Id = 30,
                    NombreComun = "Yerba mate",
                    NombreCientifico = "Ilex paraguariensis",
                    Contraindicaciones = "Sensibilidad a la cafeína, hipertensión",
                    ParteUsada = "Hoja",
                    Origen = "Sudamérica",
                    Precio = 700m,
                    Stock = 150,
                    CategoriaId = 3,
                    ImagenUrl = "/images/plantas/yerba-mate.webp"
                }
            );

            builder.Entity<PlantaEfecto>().HasData(
                new PlantaEfecto { PlantaId = 1, EfectoId = 1, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 2, EfectoId = 2, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 3, EfectoId = 2, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 3, EfectoId = 3, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 4, EfectoId = 5, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 5, EfectoId = 3, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 5, EfectoId = 7, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 6, EfectoId = 3, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 7, EfectoId = 7, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 8, EfectoId = 3, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 9, EfectoId = 7, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 10, EfectoId = 5, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 11, EfectoId = 1, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 12, EfectoId = 2, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 13, EfectoId = 2, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 14, EfectoId = 2, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 15, EfectoId = 4, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 16, EfectoId = 4, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 17, EfectoId = 2, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 18, EfectoId = 1, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 19, EfectoId = 1, Intensidad = 5 },
                new PlantaEfecto { PlantaId = 20, EfectoId = 1, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 21, EfectoId = 1, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 22, EfectoId = 2, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 23, EfectoId = 1, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 24, EfectoId = 5, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 25, EfectoId = 6, Intensidad = 4 },
                new PlantaEfecto { PlantaId = 26, EfectoId = 3, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 27, EfectoId = 1, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 28, EfectoId = 6, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 28, EfectoId = 7, Intensidad = 3 },
                new PlantaEfecto { PlantaId = 29, EfectoId = 3, Intensidad = 5 },
                new PlantaEfecto { PlantaId = 30, EfectoId = 3, Intensidad = 4 }
            );
        }
    }
}
