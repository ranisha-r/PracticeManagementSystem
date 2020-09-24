using Microsoft.EntityFrameworkCore;
using PracticeManagementSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace PracticeManagementSystem.RegistrationLogin
{
    public class RegistrationLoginDBContext:DbContext
    {
        public RegistrationLoginDBContext(DbContextOptions<RegistrationLoginDBContext> options) : base(options)
        {
        }
        public virtual DbSet<UserInfo> UserInfo
        {
            get; set;
        }
        public virtual DbSet<Roles> Roles
        {
            get; set;
        }
    }
}
