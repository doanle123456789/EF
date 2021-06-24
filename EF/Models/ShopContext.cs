using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Models
{
    class ShopContext:DbContext
    {
        //neu trong DbContext co cac thuoc tinh, khai bao la public, kieu cua thuoc tinh la DbSet, DbSet la bieu dien mot bang
        //cua CSDL, moi dong cua bang CSDL no bieu dien mot doi tuong lop nao do. O day chung ta khai bao ra mot cai DbSet tuc la
        //mot bang bieu dien cac san pham Product,cac dong cua DbSet la Product. Luc nay CSDL trong DbContext no biet la no co mot
        //cai bang ma moi dong cua bang do la tuong ung voi mot phan tu kieu Product.


        //Logging
        //Khi truy van cap nhat dlieu dua tren dbcontext, dua tren EF, we khong phai lam viec truc tiep voi nhung cau lenh SQL 
        //ma thu vien nay tu dong phai sinh ra nhung cau lenh truy van SQL tuong ung va no thi hanh. Trong truong hop chung ta 
        //muon giam sat no thuc su hoat dong ntn va no tao ra nhung cau truy van SQL ra lam sao chung ta can sdung ky thuat
        //ghi lai nhung thong tin do 
        //Trong dbcontext khai bao cho no thuoc tinh co kieu ILoggerFactory. goi pthuc Create(tham so cua no la delegate co kieu
        //la LoggingBuilder, no tao ra 1 doi tuong de hien thi logging 
        //O day chung ta thi hanh 1 pthuc cua builder de we thiet lap logging 
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => 
        {
            //muon hien thi thong tin log nao thi the hien o AddFilter nay. 
            //Nhung loai thong tin ve log lien quan den truy van CSDL o EF chung ta lay duoc o DbLoggerCategory, va muc do log 
            //ma we hien thi we dien tham so la LogLevel 
            builder.AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information);
            //builder.AddFilter(DbLoggerCategory.Database.Name, LogLevel.Information);
            //cuoi cung we phai cho biet la log ay hien thi o console 
            builder.AddConsole();
        });

        //khi gan thuoc tinh [Table("myproduct")] thi thuoc tinh products tuong ung voi bang myproduct tren CSDL
        public DbSet<Product> products { get; set; }
        

        //them ky hieu @ de xuong hang cho de xem
        private const string connectionString = @"
            Data Source = localhost;
            Database=data01;
            User Id=sa;
            Password=123456";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseSqlServer(connectionString); //cho biet Pthuc nay lviec voi CSDL SQLServer
        }
    }
}
