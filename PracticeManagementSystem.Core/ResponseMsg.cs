using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Core
{
    public class ResponseMsg
    {
        public string successMsg { get; set; }
    }
    public class UserHistory
    {
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Location { get; set; }
		public string EmailId { get; set; }
		public string UserType { get; set; }
		public bool ActiveUser { get; set; }
		public string GST { get; set; }
		public DateTime LastLogin { get; set; }		
	}
}
