﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Finance
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class user70_dbEntities : DbContext
    {
        public user70_dbEntities()
            : base("name=user70_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C7Balance> C7Balance { get; set; }
        public virtual DbSet<C7KategoriPengeluaran> C7KategoriPengeluaran { get; set; }
        public virtual DbSet<C7Tranzakcii> C7Tranzakcii { get; set; }
        public virtual DbSet<C7User> C7User { get; set; }
    }
}
