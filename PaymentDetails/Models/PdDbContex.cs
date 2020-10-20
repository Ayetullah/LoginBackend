using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentDetails.Models
{
    public class PdDbContex: DbContext
    {
        public PdDbContex(DbContextOptions<PdDbContex> options) : base(options)
        {

        }

        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
