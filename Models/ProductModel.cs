using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductDetails.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "This feild is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Name should be min of 3 characters and maximum of 250 characters")]
        public string Name { get; set; }



        [Required(ErrorMessage = "Description is required")]
        [StringLength(4000, MinimumLength = 3, ErrorMessage = "Description should be of minimum 50 characters and maximum of 4000 characters")]

        public string Description { get; set; }


        [Required(ErrorMessage = "Expiry date is required")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }


        [Required(ErrorMessage = "select the Image")]
        public string Image { get; set; }

        [Required(ErrorMessage = "select the status")]
        public string Status { get; set; }



        [Required(ErrorMessage = "creation date is required")]
        public DateTime CreationDate { get; set; }
/*
        [NotMapped]
        public IFormFile ProductImage { get; set; }
     */
    }
}
