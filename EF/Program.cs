using EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace EF
{
    class Program
    {
        //ham tao co so du lieu. Su dung context nay de tao du lieu
        static void CreateDatabase()
        {
            //khai bao bien dbContext khoi tao bang context cua chung ta 
            //khi co duoc dbcontext roi we can lay duoc ten cua CSDL ma no dang lam viec
            using var dbcontext = new ProductDbContext(); //su dung tu khoa using de no tu dong giai phong tai nguyen

            //de lay ten CSDL we truy cap vao dbcontext, truy cap vao thuoc tinh Database, goi pthuc GetDbConnection(),
            //no tra ve mot dtuong, trong dtuong nay co thuoc tinh Database, day chinh la ten CSDL
            string dbname = dbcontext.Database.GetDbConnection().Database;
            //Console.WriteLine(dbname);

            //de tao ra CSDL tu dbContext, chung ta co the goi phthuc EnsureCreate() tu thuoc tinh Database nhu sau:
            //phthuc nay thi hanh thi no se kiem tra tren server, dlieu do khong co thi no se tao ra CSDL do, neu trong CSDL
            //do khong co cac bang do dbcontext bieu dien thi no se tao ra nhung bang do, kq chay se tra ve true neu thanh cong
            //va tra ve false neu that bai.
            var kq = dbcontext.Database.EnsureCreated();
            if (kq)
            {
                Console.WriteLine($"tao db {dbname} thanh cong");
            }
            else
            {
                Console.WriteLine($"khong tao duoc {dbname}");
            }


        }

        static void DropDatabase()
        {
            using var dbcontext = new ProductDbContext();
            string dbname = dbcontext.Database.GetDbConnection().Database;
            var kq = dbcontext.Database.EnsureDeleted();
            if (kq)
            {
                Console.WriteLine($"xoa {dbname} thanh cong");
            }
            else
            {
                Console.WriteLine($"khong xoa duoc {dbname}");
            }
        }

        static void InsertProduct()
        {
            using var dbcontext = new ProductDbContext();
            /*
             * -De chen 1 dong dl nao do vao dbcontext(vao CSDL) thi we phai thuc hien tao dtuong model(Product), sau do goi pthuc
             * Add hoac AddAsync.
             * -De ra lenh cho dbcontext cap nhat vao SQL server thi chung ta goi pthuc SaveChange hoac SaveChangeAsync
             * -Pthuc SaveChange tra ve so dong bi tac dong 
             * 
             * -Pthuc AddRange cho phep them vao DbContext ca 1 mang cac model, cac model co the khac nhau 
             * -ta khoi tao ra 1 mang cac doi tuong, moi phan tu la cac doi tuong model 
             */

            //B1: Tao model
            //var p1 = new Product() 
            //{ 
            //    ProductName = "San pham 2",
            //    Provider = "Cong ty 2"
            //};
            var products = new object[] { 
                new Product(){ProductName="San pham 3", Provider="Cong ty A"},
                new Product(){ProductName="San pham 4", Provider="Cong ty B"},
                new Product(){ProductName="San pham 5", Provider="Cong ty C"},
            };

            //B2: Add hoac AddAsync
            //dbcontext.Add(p1);
            dbcontext.AddRange(products);

            //B3: SaveChange hoac SaveChangeAsync
            int number_rows = dbcontext.SaveChanges();
            Console.WriteLine($"Da chen {number_rows} du lieu");

        }

        static void ReadProduct()
        {
            using var dbcontext = new ProductDbContext();
            /*
             * Su dung linq de truy van dlieu. Nguon truy van la cac thuoc tinh bieu dien cac table trong DbContext. vd thuoc tinh
             * product
             */

            Console.WriteLine("..........lay tat ca sp product.................");
            //lay tat ca sp product, truy cap vao thuoc tinh product, goi pthuc ToList(), luu duoc tat ca sp, duyet qua nhung sp nay
            //va in thong tin cua no ra, we can dung vong lap Foreach hoac dung API cua linq 
            var products = dbcontext.products.ToList();
            products.ForEach(product => product.PrintInfo());

            Console.WriteLine("..........lay tat ca sp co productId >= 3.................");
            //we viet cau truy van linq, ket qua cau truy van luu vao bien qr
            //luc nay ket qua truy van luu trong qr la 1 tap hop cac sp tim duoc
            //can dung vong lap de in sp
            var qr = from product in dbcontext.products
                     where product.ProductId >= 3
                     select product;
            qr.ToList().ForEach(product => product.PrintInfo());

            Console.WriteLine("......Lay tat ca sp trong ten cty co chu CTY, kq sap xep theo productId tu cao xuong thap....");
            var qr1 = from product in dbcontext.products
                      where product.Provider.Contains("CTY")
                      orderby product.ProductId descending
                      select product;
            qr1.ToList().ForEach(product => product.PrintInfo());

            Console.WriteLine("..........lay ra sp co Id =4.................");
            //cau truy van nay tra ve 1 tap hop cac kq. de lay ra kq dau tien we goi pthuc FirstOrDefault(). Neu tim thay thi no 
            //se tra ve kq, neu khg tim thay thi no tra ve gtri la null
            Product pro = (from product in dbcontext.products
                      where product.ProductId == 4
                      select product).FirstOrDefault();
            if (pro != null)
                pro.PrintInfo();

            Console.WriteLine("..........lay ra sp dau tien do CTY A SX.................");
            Product proA = (from p in dbcontext.products
                            where p.Provider.Contains("A")
                            select p).FirstOrDefault();
            if (proA != null)
                proA.PrintInfo();
        }


        static void RenameProduct(int id, string newName)
        {
            using var dbcontext = new ProductDbContext();
            //lay ra sp theo id truyen vao
            Product proTheoId = (from p in dbcontext.products
                                where p.ProductId == id
                                select p).FirstOrDefault();

            //sau khi thay doi model de dbcontext cap nhat thi goi pthuc SaveChange()


            if (proTheoId != null)
            {
                proTheoId.ProductName = newName;
                int number_rows = dbcontext.SaveChanges();
                Console.WriteLine($"Da cap nhat {number_rows} dong du lieu");
            }


            //khi su dung linq de truy van dbcontext, lay ra duoc model product thi ngoai no tra ve cho we chinh doi tuong product nay
            //thi Dbcontext bat dau theo doi doi tuong nay luon. Trong dbcontext se co 1 doi tuong co kieu la EntityEntry<ben trong 
            //no chua phan tu co kieu cua model>
            //chung ta co the lay dtuon EntityEntry cua dtuong Product nay bang cach goi dbcontext.Entry(dien ten model). Dtuong nay
            //duoc dung de theo doi su thay doi cua model product 
            //Sau khi co model product chung ta lay doi tuong Entry cua no, tuc la doi tuong giam sat su thay doi cua product 
            //trong dbcontext 

            //Bay gio cung ta se gan entry co thuoc tinh State duoc gan bang EntityState.Detached => tach ra khong bi theo doi boi
            //dbcontext nua. Bay gio moi su thay doi cua product khong con nam trong su giam sat cua dbcontext nua. chung ta goi 
            //SaveChange thi cung khong co thay doi gi

            //if(proTheoId != null)
            //{
            //    EntityEntry<Product> entry = dbcontext.Entry(proTheoId);
            //    entry.State = EntityState.Detached;
            //    proTheoId.ProductName = newName;
            //    int number_rows = dbcontext.SaveChanges();
            //    Console.WriteLine($"da cap nhat {number_rows} dong du lieu");
            //}
        }

        //xoa sp theo id
        static void DeleteProduct(int id)
        {
            using var dbcontext = new ProductDbContext();
            Product proTheoId = (from p in dbcontext.products
                                 where p.ProductId == id
                                 select p).FirstOrDefault();
            if(proTheoId != null)
            {
                dbcontext.Remove(proTheoId);
                int number_rows = dbcontext.SaveChanges();
                Console.WriteLine($"da xoa {number_rows} dong du lieu");
            }
        }


        static void Main(string[] args)
        {
            //CreateDatabase();
            //DropDatabase();
            //Insert, Select, Update, Delete
            //InsertProduct();
            //ReadProduct();
            RenameProduct(2, "Lap top 02");
            //DeleteProduct(1);
            
        }
    }
}
