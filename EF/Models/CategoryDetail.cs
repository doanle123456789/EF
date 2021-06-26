using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Models
{
    public class CategoryDetail
    {
        public int CategoryDetailId { set; get; }
        public int UserId { set; get; }

        public DateTime Create { set; get; }

        public DateTime Updated { set; get; }
        public int CountProduct { set; get; }
        public Category category { set; get; }
    }
}
