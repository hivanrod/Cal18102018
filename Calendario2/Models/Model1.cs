namespace Calendario2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Contacto> Contactos { get; set; }
        public virtual DbSet<Prioridad> Prioridads { get; set; }
        public virtual DbSet<Tema> Temas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Archivo> Archivos { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Contacto>()
                .HasMany(e => e.Temas)
                .WithOptional(e => e.Contacto)
                .HasForeignKey(e => e.IdContacto);

            modelBuilder.Entity<Prioridad>()
                .HasMany(e => e.Temas)
                .WithOptional(e => e.Prioridad)
                .HasForeignKey(e => e.IdPrioridad);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Temas)
                .WithOptional(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuario);

        }
    }
}
