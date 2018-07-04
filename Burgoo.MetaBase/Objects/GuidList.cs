using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burgoo.MetaBase.Objects
{
	public class GuidList : List<Guid>
	{
		public string ToStringOfBinary( )
		{
			var rtrn = "";
			foreach (Guid i in this)
			{
				if (rtrn.Length > 0) rtrn += ",";

				rtrn += string.Format("'{0}'", i.ToString("N"));
			}
			return rtrn;
		}
	}
}
