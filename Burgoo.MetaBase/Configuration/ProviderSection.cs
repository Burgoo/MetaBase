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
using System.Configuration;

namespace Burgoo.MetaBase.Configuration
{

	internal class ProviderSection : ConfigurationSection
	{
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)this["providers"];
			}
		}

		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("defaultProvider", DefaultValue = "SqlDataProvider")]
		public string DefaultProvider
		{
			get
			{
				return System.Convert.ToString(this["defaultProvider"]);
			}
			set
			{
				this["defaultProvider"] = value;
			}
		}
	}

}