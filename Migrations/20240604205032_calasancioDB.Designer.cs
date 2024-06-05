﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CpCalasancio.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240604205032_calasancioDB")]
    partial class calasancioDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CarnetSocio", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int?>("Carritoid")
                        .HasColumnType("int");

                    b.Property<int>("clienteid")
                        .HasColumnType("int");

                    b.Property<int>("precio")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("Carritoid");

                    b.HasIndex("clienteid");

                    b.ToTable("CarnetSocios");
                });

            modelBuilder.Entity("Carrito", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<bool>("enviado")
                        .HasColumnType("bit");

                    b.Property<decimal>("total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("id");

                    b.ToTable("Carritos");
                });

            modelBuilder.Entity("Cliente", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int?>("Carritoid")
                        .HasColumnType("int");

                    b.Property<string>("apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dni")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Carritoid");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("Evento", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<bool>("asiste")
                        .HasColumnType("bit");

                    b.Property<DateTime>("fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("lugar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Eventos");
                });

            modelBuilder.Entity("Jugador", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int?>("Partidoid")
                        .HasColumnType("int");

                    b.Property<decimal?>("altura")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("goles")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("numeroCamiseta")
                        .HasColumnType("int");

                    b.Property<bool>("partidosJugados")
                        .HasColumnType("bit");

                    b.Property<string>("pie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("posicion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Partidoid");

                    b.ToTable("Jugadores");
                });

            modelBuilder.Entity("Partido", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<DateTime>("fecha")
                        .HasColumnType("datetime2");

                    b.Property<bool>("ganado")
                        .HasColumnType("bit");

                    b.Property<int>("jornadaId")
                        .HasColumnType("int");

                    b.Property<string>("nombrePartido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("primerEquipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("puntuacion")
                        .HasColumnType("int");

                    b.Property<string>("segundoEquipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Partidos");
                });

            modelBuilder.Entity("Producto", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int?>("Carritoid")
                        .HasColumnType("int");

                    b.Property<string>("nombreProducto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("id");

                    b.HasIndex("Carritoid");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("Usuario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int?>("Eventoid")
                        .HasColumnType("int");

                    b.Property<string>("contraseña")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombreUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Eventoid");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("CarnetSocio", b =>
                {
                    b.HasOne("Carrito", null)
                        .WithMany("carnetSocios")
                        .HasForeignKey("Carritoid");

                    b.HasOne("Cliente", "cliente")
                        .WithMany()
                        .HasForeignKey("clienteid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cliente");
                });

            modelBuilder.Entity("Cliente", b =>
                {
                    b.HasOne("Carrito", null)
                        .WithMany("clientes")
                        .HasForeignKey("Carritoid");
                });

            modelBuilder.Entity("Jugador", b =>
                {
                    b.HasOne("Partido", null)
                        .WithMany("jugadores")
                        .HasForeignKey("Partidoid");
                });

            modelBuilder.Entity("Producto", b =>
                {
                    b.HasOne("Carrito", null)
                        .WithMany("items")
                        .HasForeignKey("Carritoid");
                });

            modelBuilder.Entity("Usuario", b =>
                {
                    b.HasOne("Evento", null)
                        .WithMany("usuarios")
                        .HasForeignKey("Eventoid");
                });

            modelBuilder.Entity("Carrito", b =>
                {
                    b.Navigation("carnetSocios");

                    b.Navigation("clientes");

                    b.Navigation("items");
                });

            modelBuilder.Entity("Evento", b =>
                {
                    b.Navigation("usuarios");
                });

            modelBuilder.Entity("Partido", b =>
                {
                    b.Navigation("jugadores");
                });
#pragma warning restore 612, 618
        }
    }
}
