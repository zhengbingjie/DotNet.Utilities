﻿using System.Data.Entity;
using YanZhiwei.DotNet.Framework.Data;

namespace YanZhiwei.DotNet.Framework.DataTests
{
    internal class AdventureWorks2014DbContext : DbContextBase<int>
    {
        public AdventureWorks2014DbContext() : base(@"Data Source=DESKTOP-N3GTH4E\SQLEXPRESS;Initial Catalog=AdventureWorks2014;Persist Security Info=True;User ID=sa;Password=sasa", null)
        {
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AdventureWorks2014DbContext>(null);//从不创建数据库
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Address> Addresses
        {
            get;
            set;
        }
    }
}