using Connection1.Global;
using System.Data.Entity;
using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connection1.Entities;

namespace Connection1.Connection
{
    public class ConnDbContext : DbContext
    {
        public ConnDbContext() : base("ConnDbContext")
        {
        }
        public DbSet<MenuCategory> menucategories { get; set; }
        public DbSet<Product> productlist { get; set; }

        
    }
}
