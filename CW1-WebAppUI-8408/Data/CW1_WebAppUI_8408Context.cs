using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CW1_WebAppUI_8408.Models;

namespace CW1_WebAppUI_8408.Data
{
    public class CW1_WebAppUI_8408Context : DbContext
    {
        public CW1_WebAppUI_8408Context (DbContextOptions<CW1_WebAppUI_8408Context> options)
            : base(options)
        {
        }

        public DbSet<CW1_WebAppUI_8408.Models.Car> Car { get; set; }

        public DbSet<CW1_WebAppUI_8408.Models.Category> Category { get; set; }
    }
}
