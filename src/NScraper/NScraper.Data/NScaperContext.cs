namespace NScraper.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NScaperContext : DbContext
    {
        public NScaperContext()
            : base("name=NScaperContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
