using Microsoft.EntityFrameworkCore;
using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.PracticeManagement
{
    public class PracticeMgmntDBContext:DbContext
    {
        public PracticeMgmntDBContext(DbContextOptions<PracticeMgmntDBContext> options) : base(options)
        {
        }
        public virtual DbSet<PracticeInfo> PracticeInfo { get; set; }
    }
}
