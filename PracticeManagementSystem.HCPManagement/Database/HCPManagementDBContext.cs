using Microsoft.EntityFrameworkCore;
using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.HCPManagement
{
    public class HCPManagementDBContext: DbContext
    {
        public HCPManagementDBContext(DbContextOptions<HCPManagementDBContext> options) : base(options)
        {
        }        
        public virtual DbSet<HCPInteractionInfo> HCPInteractionInfo { get; set; }
        public virtual DbSet<DocumentInfo> DocumentInfo { get; set; }
    }
}
