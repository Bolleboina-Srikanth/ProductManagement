using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductDetails.Context;
using ProductDetails.Entity;
using ProductDetails.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductDetails.Services
{
    public class ProductService
    {
        private readonly ProductContext _context;

        public ProductService(ProductContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductEntity> GetAllProducts()
        {
            var result = _context.ProductModel.FromSqlRaw("SP_GETALLPRODUCTS").ToList();
            return result;
             
        }


        public void CreateProduct(ProductModel productModel)
        {
            _context.CreateProductWithStoredProcedure(productModel.Code, productModel.Name, productModel.Description, productModel.ExpiryDate, productModel.Category, productModel.Image, productModel.Status, productModel.CreationDate);
        }


       
            public void UpdateProductWithStoredProcedure(int productId, string code, string name, string description, DateTime expiryDate, string category, string image, string status, DateTime creationdate)
            {
                string sql = "EXEC sp_UpdatePRODUCT @ProductId, @Code, @Name, @Description, @ExpiryDate, @Category, @Image, @Status, @CreationDate";
                _context.Database.ExecuteSqlRaw(sql,
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@Code", code),
                new SqlParameter("@Name", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@ExpiryDate", expiryDate),
                new SqlParameter("@Category", category),
                new SqlParameter("@Image", image),
                new SqlParameter("@Status", status),
                new SqlParameter("@CreationDate", creationdate));
            }


             public void DeleteProductWithStoredProcedure(int? id)
             {
               // string sql = "EXEC sp_DeletePRODUCT @ProductId";
                _context.Database.ExecuteSqlRaw("EXEC sp_DELETEPRODUCTS @ProductId",
                new SqlParameter("@ProductId", id));
             
             }


       

        public IEnumerable<ProductModel> GetProductById(int? productId)
        {
            var result = _context.ProductModel.FromSqlRaw("EXEC SP_GetProductById @ProductId",
                   new SqlParameter("@ProductId", productId))
                .Select(ProductEntity => new ProductModel
                {
                    ProductId = ProductEntity.ProductId,
                    Code = ProductEntity.Code,
                    Name = ProductEntity.Name,
                    Description = ProductEntity.Description,
                    ExpiryDate = ProductEntity.ExpiryDate,
                    Category = ProductEntity.Category,
                    Image = ProductEntity.Image,
                    Status = ProductEntity.Status,
                    CreationDate = ProductEntity.Creationdate
                })
                .ToList();

            return result;
        }







    }
}
