using Microsoft.EntityFrameworkCore;
using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.PatientManagement
{
    public class PatientManagementDBContext:DbContext
    {
        public PatientManagementDBContext(DbContextOptions<PatientManagementDBContext> options) : base(options)
        {

        }
        public virtual DbSet<PatientInfo> PatientInfo
        {
            get; set;
        }

    }
}
