using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burgoo.MetaBase.Objects
{
	public class MetaAttribute
	{
		public Guid ID { get; set; }
		public Guid EntityID { get; set; }
		public string Name { get; set; }
		public object Value { get; set; }
	}
}
