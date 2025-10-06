using ECommerce.Models;
using ECommerce.Models.CMSModels;
using ECommerce.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactUs> Contacts { get; set; }
        public DbSet<CoupenDetail> CoupenDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderedItem> OrderedItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API configurations can be added here if needed

            // cart items relation-->

            modelBuilder.Entity<CartItem>() // cart item and customer relation
                .HasOne(Ci=>Ci.Customer)
                .WithMany(Cstmr=>Cstmr.CartItems)
                .HasForeignKey(Ci=>Ci.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>() //cart item and product relation
                .HasOne(Ci => Ci.Product)
                .WithMany(Prdct => Prdct.CartItems)
                .HasForeignKey(Ci => Ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>() // cart itme and guest relation
                .HasOne(Ci => Ci._Guest)
                .WithMany(Gst => Gst.CartItems)
                .HasForeignKey(Ci => Ci.GuestId)
                .OnDelete(DeleteBehavior.Cascade);


            //category relations-->
            modelBuilder.Entity<Category>() // category and staff relation
                .HasMany(Ctg => Ctg.Staffs)
                .WithMany(Stf => Stf.Categories)
                .UsingEntity(J => J.ToTable("Staff_Categories_Rel"));

            modelBuilder.Entity<Category>()
                .HasIndex(Ctg => Ctg.Name)
                .IsUnique();

            //coupen details relation-->
            modelBuilder.Entity<CoupenDetail>() // Coupen and staff relation
                .HasMany(Cd => Cd.Staffs)
                .WithMany(stf => stf.CoupenDetails)
                .UsingEntity(J => J.ToTable("Staff_CoupenDetails_Rel"));
            modelBuilder.Entity<CoupenDetail>()
                .HasIndex(CD => CD.CCode)
                .IsUnique();

            //Order details relation-->
            modelBuilder.Entity<OrderDetail>() // order detail and customer relation 
                .HasOne(OrdrDt => OrdrDt.Customer)
                .WithMany(Cstmr => Cstmr.OrderDetails)
                .HasForeignKey(OrdrDt => OrdrDt.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>() // order detail and guest relation
                .HasOne(OrdrDt => OrdrDt._Guest)
                .WithMany(GST => GST.OrderDetails)
                .HasForeignKey(OrdrDt => OrdrDt.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            //Order Item relation-->
            modelBuilder.Entity<OrderedItem>() // order item and product relation
                 .HasOne(OrdrItm => OrdrItm.Prdct)
                 .WithMany(Prdct => Prdct.OrderedItems)
                 .HasForeignKey(OrdrItm => OrdrItm.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderedItem>() // order item and order detail relation
                .HasOne(OrdrItm => OrdrItm.ODetail)
                .WithMany(OrdrDt => OrdrDt.OrderedItems)
                .HasForeignKey(OrdrItm => OrdrItm.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // products relations-->
            modelBuilder.Entity<Product>() // Product and category relation
                .HasOne(Prdct => Prdct.Ctgry)
                .WithMany(Ctg => Ctg.Products)
                .HasForeignKey(Prdct => Prdct.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // relation for the Reviews-->
            modelBuilder.Entity<Review>() // reviews and customer relation
                .HasOne(Rvw => Rvw.Cstmr)
                .WithMany(Cstmr => Cstmr.Reviews)
                .HasForeignKey(Rvw => Rvw.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>() // reviews and guest relation
                .HasOne(Rvw => Rvw._Guest)
                .WithMany(Gst => Gst.Reviews)
                .HasForeignKey(Rvw => Rvw.GuestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>() // reviews and product relation
                .HasOne(Rvw => Rvw.Prdct)
                .WithMany(Prdct => Prdct.Reviews)
                .HasForeignKey(Rvw => Rvw.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //staff relation-->
            modelBuilder.Entity<Staff>() // staff and products  relation
                .HasMany(Stf => Stf.Products)
                .WithMany(Prdct => Prdct.Staffs)
                .UsingEntity(J => J.ToTable("Staff_Products_Rel"));
            modelBuilder.Entity<Staff>()
                .HasOne(Stf => Stf.User)
                .WithOne()
                .HasForeignKey<Staff>(Stf=>Stf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Wish List relation -->
            modelBuilder.Entity<WishList>() // wishlist and customer relation
                .HasOne(WLst => WLst.Cstmr)
                .WithMany(Cstmr => Cstmr.WishLists)
                .HasForeignKey(WLst => WLst.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishList>() // wishlist and Guest relation
                .HasOne(WLst => WLst._Guest)
                .WithMany(Gst => Gst.WishLists)
                .HasForeignKey(WLst => WLst.GuestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishList>() // wishlist and product relation
                .HasOne(WLst => WLst.Prdct)
                .WithMany(Prdct => Prdct.WishLists)
                .HasForeignKey(WLst => WLst.ProductsId)
                .OnDelete(DeleteBehavior.Cascade);

            // refresh token relation with the application user
            modelBuilder.Entity<RefreshToken>()
                .HasOne(RefToken=>RefToken.User)
                .WithMany()
                .HasForeignKey(RefToken=> RefToken.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //customer relation
            modelBuilder.Entity<Customer>()
                .HasOne(Cstmr => Cstmr.User)
                .WithOne()
                .HasForeignKey<Customer>(Cstmr => Cstmr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser configuration
            modelBuilder.Entity<ApplicationUser>()
                .Property(AppUser => AppUser.UserType)
                .HasConversion<string>();
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName).IsUnique(false);
        }

    }
}
