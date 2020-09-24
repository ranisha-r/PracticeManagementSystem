using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PracticeManagementSystem.Core
{
    public class UserInfo
    {
		[Key]
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public int PracticeId { get; set; }
		public string Location { get; set; }
		public string EmailId { get; set; }
		public string UserType { get; set; }
		public int ActiveFlg { get; set; }
		public int RoleId { get; set; }
		public int CreatedBy { get; set; }
		public DateTime UpdatedDate { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public class Roles
    {
		[Key]
		public int RoleId { get; set; }
		public string RoleName { get; set; }
	}
}




