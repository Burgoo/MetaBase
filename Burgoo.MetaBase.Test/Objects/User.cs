using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burgoo.MetaBase.Test.Objects
{
	public class User
	{
		public string LoginID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public DateTime  DOB { get; set; }

		public string PhoneNumber { get; set; }
		public string EmailAddress { get; set; }

		// public Address Address;

	}
}
