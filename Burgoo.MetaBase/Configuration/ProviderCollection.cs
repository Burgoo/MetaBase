using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Configuration.Provider;

namespace Burgoo.MetaBase.Configuration
{
	public class ProviderCollection : System.Configuration.Provider.ProviderCollection
	{
		new public Data.DataProvider this[string name]
		{
			get
			{
				return (Data.DataProvider)base[name];
			}
		}

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			if (!(provider is ProviderBase))
				throw new ArgumentException("Invalid provider type", "provider");

			base.Add(provider);
		}
	}
}