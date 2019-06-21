namespace sadna192
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
       // public virtual DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
            .HasMany<Owner>(m => m.owner)
            .WithRequired(o => o.user)
            .HasForeignKey<int>(o => o.userRef);

          /*  modelBuilder.Entity<Store>()
          .HasMany<Owner>(s => s.owners)
          .WithRequired(o => o.store)
          .HasForeignKey<string>(o => o.storeName);*/

            modelBuilder.Entity<Owner>().
              HasMany(o => o.has_Apointed).
              WithMany()
              .Map(o =>
              {
                  o.ToTable("ownerApointed");
                  o.MapLeftKey("ownerID");
                  o.MapRightKey("ApointedOwnerID");
              });
        }
    }
}
