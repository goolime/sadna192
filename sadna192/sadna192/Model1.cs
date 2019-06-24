namespace sadna192
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        //public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<MultiplePolicy> MultiplePolicies { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<multipleDiscount> MultipleDiscounts { get; set; }
        public virtual DbSet<ProductInStore> ProductInStores { get; set; }
      /*  public virtual DbSet<ItemsInCart> ItemsInCarts { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ShopingBasket> ShopingBaskets { get; set; }*/



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        
            modelBuilder.Entity<Member>()
            .HasMany<Owner>(m => m.owner)
            .WithRequired(o => o.user)
            .HasForeignKey<int>(o => o.userRef);
            
           modelBuilder.Entity<Store>()
          .HasMany<Owner>(s => s.owners)
          .WithRequired(o => o.store)
          .HasForeignKey<int>(o => o.storeRef);

           modelBuilder.Entity<Store>()
          .HasMany<ProductInStore>(s => s.productInStores)
          .WithRequired(p => p.store)
          .HasForeignKey<int>(p => p.storeID);

         /*   modelBuilder.Entity<ShoppingCart>()
          .HasMany<ItemsInCart>(s => s.shoppingCartContent)
          .WithRequired(i => i.shopping)
          .HasForeignKey<int>(i => i.shopCartRef);*/

           /* modelBuilder.Entity<Owner>().
              HasMany(o => o.has_Apointed).
              WithMany()
              .Map(o =>
              {
                  o.ToTable("ownerApointed");
                  o.MapLeftKey("ownerID");
                  o.MapRightKey("ApointedOwnerID");
              });*/

           /* modelBuilder.Entity<ShopingBasket>().
              HasMany(b => b.shoppingCarts).
              WithMany()
              .Map(c =>
              {
                  c.ToTable("shopingBasketDetailed");
                  c.MapLeftKey("id");
                  c.MapRightKey("shoppingCartID");
              });*/


            modelBuilder.Entity<MultiplePolicy>().
             HasMany(p => p.Policies).
             WithMany()
             .Map(p =>
             {
                 p.ToTable("MultiplePolicy");
                 p.MapLeftKey("MultiplePolicyID");
                 p.MapRightKey("PolicyID");
             });

            modelBuilder.Entity<multipleDiscount>().
             HasMany(d => d.discount).
             WithMany()
             .Map(d =>
             {
                 d.ToTable("MultipleDiscount");
                 d.MapLeftKey("MultipleDiscountID");
                 d.MapRightKey("DiscountID");
             });

            

        }
    }
}
