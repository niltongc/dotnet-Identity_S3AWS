using exmaple_identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace exmaple_identity.Data
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.Entity<IdentityUser>(
        //        entity =>
        //        {
        //            entity.HasKey(e => e.Id);
        //        });
        //}
    }

   
}
