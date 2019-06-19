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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
            .HasMany<Owner>(m => m.owner)
            .WithRequired(o => o.user)
            .HasForeignKey<int>(o => o.userRef);

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
