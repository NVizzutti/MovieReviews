namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MovieReviews.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "MovieReviews.Models.ApplicationDbContext";
        }
         
        protected override void Seed(MovieReviews.Models.ApplicationDbContext context)
        {

        }
    }
}
