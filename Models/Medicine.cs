using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MediFlowApi.Models
{
    public class Medicine 
    {
        [Key]
        public int Id { get; set; }
        [Required , MaxLength(100)]
        public string Name { get; set; }
        [Required,Range(0,100000) ]
        public double Price { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
       
        public DateTime CreatedAt { get; set; }=DateTime.Now;
    }
}
