using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EchoRoborApi.Models;

public partial class EchoRobotContext : DbContext
{
    public EchoRobotContext()
    {
    }

    public EchoRobotContext(DbContextOptions<EchoRobotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Multimedia> Multimedia { get; set; }

    public virtual DbSet<Publicacion> Publicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost; Database=EchoRobot; Trusted_Connection=true ; TrustServerCertificate = true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdRespuesta).HasName("PK_Respuesta");

            entity.ToTable("Comentario");

            entity.Property(e => e.IdRespuesta).HasColumnName("idRespuesta");
            entity.Property(e => e.Descripcion)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaPublicacion");
            entity.Property(e => e.IdAutor).HasColumnName("idAutor");
            entity.Property(e => e.IdPublicacion).HasColumnName("idPublicacion");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdAutor)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Respuesta_Usuario");

            entity.HasOne(d => d.IdPublicacionNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdPublicacion)
                .HasConstraintName("FK_Respuesta_Publicacion");
        });

        modelBuilder.Entity<Multimedia>(entity =>
        {
            entity.HasKey(e => e.IdArchivo);

            entity.Property(e => e.Direccion).IsUnicode(false);
            entity.Property(e => e.IdPublicacion).HasColumnName("idPublicacion");

            entity.HasOne(d => d.IdPublicacionNavigation).WithMany(p => p.Multimedia)
                .HasForeignKey(d => d.IdPublicacion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Multimedia_Publicacion");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.HasKey(e => e.IdPublicacion);

            entity.ToTable("Publicacion");

            entity.Property(e => e.IdPublicacion).HasColumnName("idPublicacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaPublicacion");
            entity.Property(e => e.IdAutor).HasColumnName("idAutor");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.Publicacions)
                .HasForeignKey(d => d.IdAutor)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Publicacion_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Foto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
