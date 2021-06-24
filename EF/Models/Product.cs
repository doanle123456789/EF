using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]//ProductName phai khac null
        [StringLength(50)]
        public string ProductName { get; set; }

        [StringLength(50)]
        public string Provider { get; set; }
        public void PrintInfo() => Console.WriteLine($"{ProductId} - {ProductName} - {Provider}");

    }
}
