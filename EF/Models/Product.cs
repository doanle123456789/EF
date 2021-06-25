using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]//ProductName phai khac null
        [StringLength(50)]
        [Column("Tensanpham", TypeName ="ntext")]
        public string Name { get; set; }
        [Column(TypeName ="money")]
        public decimal Price { get; set; }
        public int CateId { get; set; }
        //Reference Navigation
        //ForeignKey
        [ForeignKey("CateId")]
        //[Required]
        public virtual Category Category { get; set; }

        public int? CateId2 { get; set; }
        //Reference Navigation
        //ForeignKey
        [ForeignKey("CateId2")]
        [InverseProperty("Products")]
        public virtual Category Category2 { get; set; }

        public void PrintInfo() => Console.WriteLine($"{ProductId} - {Name} - {Price} - {CateId}");

    }
}


/*
 * [Table("TableName")]
 * [Key] -> Primary Key (PK)
 * [Required] -> not null
 * [StringLength(50)] -> string - nvarchar
 * [Column("Tensanpham", TypeName ="ntext")]
 * [ForeignKey("CateId")]
 * [InverseProperty("Products")]
 * 
 * Reference Navigation -> Foreign Key (1 - nhieu)
 * Collect Navigation - > Khong AH den SQL Server, no chi tao ra cac Property de we truy cap va use boi EF, khong tao FK
 * 
 * Mac dinh thi EF se can cu vao kieu dlieu khai bao trong properties de xd kieu tuong ung tren SQL Server.
 * Truong hop we muon khi khai bao dlieu tren model se tuong ung voi truong dl tren server, we use thuoc tinh Column
 * 
 * Trong truong hop we muon chi ra ten thuoc tinh khac ten truong dlieu tren server cung su dung Colum
 */