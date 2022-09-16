using Microsoft.EntityFrameworkCore;
using Policy.API.Models;

namespace Policy.API.Data
{
    public class PolicyDBContext : DbContext
    {
        //Inherits DbContextOptions then pass it to the base class
        public PolicyDBContext(DbContextOptions options) : base(options)
        {
        }

        //DBSet
        public DbSet<PolicyHolder> PolicyHolders { get; set; }
        public DbSet<AvbobPolicy> Policies { get; set; }
    }
}
