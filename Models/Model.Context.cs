﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EDMS.Models {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class Entities : DbContext {
        public Entities() : base("name=db_entities") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<ClientDocument> ClientDocuments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<UserData> UsersData { get; set; }
        public DbSet<ModeratorDocument> ModeratorDocuments { get; set; }
    }
}
