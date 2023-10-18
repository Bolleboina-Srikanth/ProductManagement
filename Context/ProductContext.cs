using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductDetails.Entity;
using System;

namespace ProductDetails.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : 
            base(options)
        {
        }
        public DbSet<ProductEntity> ProductModel { get; set; }


        public void CreateProductWithStoredProcedure(string code, string name, string description, DateTime expiryDate, string category, string image, string status, DateTime creationdate)
        {
            // Execute the stored procedure using FromSqlRaw
            Database.ExecuteSqlRaw("EXEC sp_UpsertProduct @CODE, @NAME, @DESCRIPTION, @EXPIRYDATE, @CATEGORY, @IMAGE, @STATUS, @CREATIONDATE",
                new SqlParameter("@CODE", code),
                new SqlParameter("@NAME", name),
                new SqlParameter("@DESCRIPTION", description),
                new SqlParameter("@EXPIRYDATE", expiryDate),
                new SqlParameter("@CATEGORY", category),
                new SqlParameter("@IMAGE", image),
                new SqlParameter("@STATUS", status),
                new SqlParameter("@CREATIONDATE", creationdate));
        }

    }
}
