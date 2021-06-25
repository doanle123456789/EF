using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { set; get; }
        [StringLength(100)]
        public string Name { set; get; }
        [Column(TypeName ="ntext")]
        public string Description { set; get; }
        //Collect Navagation
        public virtual List<Product> Products { set; get; }

    }
}
