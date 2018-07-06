using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burgoo.MetaBase.Objects
{

	class Query
	{
		// TODO

		public Expression Exp { get; set; }
	}



	class Expression
	{
		// TODO		

		public Operand Left { get; set; }
		public Operator Op { get; set; }
		public Operand Right { get; set; }

	}


	class Operator
	{
		public string value { get; set; }
	}


	class Operand
	{
		public string value { get;set; }

	}

	class Literal
	{
		public string value { get;set; }
	}

}
