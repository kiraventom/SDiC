using Microsoft.EntityFrameworkCore;

namespace ChemistryDB
{
    public partial class ChemistryContext : DbContext
    {
        public ChemistryContext()
        {
        }

        public ChemistryContext(DbContextOptions<ChemistryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Parameters> Parameters { get; set; }
        public virtual DbSet<ParametersTypes> ParametersTypes { get; set; }
        public virtual DbSet<ParametersValues> ParametersValues { get; set; }
        public virtual DbSet<Units> Units { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=chemistry.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Materials>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Parameters>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Parameters)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Parameters)
                    .HasForeignKey(d => d.UnitId);
            });

            modelBuilder.Entity<ParametersTypes>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ParametersValues>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.MaterialId).HasColumnName("MaterialID");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.HasOne(d => d.Material)
                    .WithMany()
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Parameter)
                    .WithMany()
                    .HasForeignKey(d => d.ParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Units>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Unit).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
