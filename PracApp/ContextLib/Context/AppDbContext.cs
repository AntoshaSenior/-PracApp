﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ContextLib.Context.Tables;
using System.IO;

namespace ContextLib.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tables.Task> Tasks { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserToProject> UserToProjects { get; set; }
        public DbSet<JsonUserWork> JsonUserWorks { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                // Добавьте эту строку для подключения к PostgreSQL
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToProject>().HasNoKey();
        }
    }
}