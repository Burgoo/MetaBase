using Burgoo.MetaBase.Objects;
using System;
using System.Configuration;
using System.Web.Configuration;
using System.Reflection;
using System.Data;
using System.Collections.Generic;

namespace Burgoo.MetaBase.Configuration
{
	public class MetaProvider : System.Configuration.Provider.ProviderBase
	{
		private static Data.DataProvider _provider = null;
		private static MetaBase.Configuration.ProviderCollection _providers = null;
		private static object _lock = new object();


		public static void SaveObject(object obj)  
		{
			LoadProviders();

			var t = obj.GetType();

			Guid ent = _provider.CreateEntity  (t.Name);

			foreach(PropertyInfo pi in t.GetProperties() )
			{

				Guid att = _provider.CreateAttribute(ent, pi.Name, pi.GetValue(obj));
			}
		}


		//
		// Get an object
		//
		public static T GetObject<T>(Guid EntityID)
		{
			var rtrn = (T)Activator.CreateInstance(typeof(T), new object[] { });
			foreach (PropertyInfo pi in rtrn.GetType().GetProperties())
			{


				var ml = _provider.GetAttributes(EntityID, new List<string> { pi.Name });


				pi.SetValue(_provider.GetAttributes(EntityID, pi.Name), rtrn);
			}
			return rtrn;
		}



		public static void Open()
		{
			LoadProviders();
			_provider.Open();  
		}


		public static void Close()
		{
			LoadProviders();
			_provider.Close();  
		}


		public static DataSet Export(string Schema = "")
		{
			LoadProviders();
			return _provider.Export(Schema);
		}


		public static void CreateSchema(string Schema = "")
		{
			LoadProviders();
			_provider.CreateSchema(Schema);
		}


		public static Guid CreateEntity(string TypeName)
		{
			LoadProviders();
			var rtrn = _provider.CreateEntity(TypeName);
			return rtrn;
		}


		public static GuidList GetEntities(string TypeName)
		{
			LoadProviders();
			var rtrn = _provider.GetEntities (TypeName);
			return rtrn;
		}


		public static Guid CreateAttribute(Guid EntityID, string Name, string Value)
		{
			LoadProviders();
			var rtrn = _provider.CreateAttribute(EntityID, Name, Value);
			return rtrn;
		}

		public static MetaAttributeList  GetAttributes(GuidList EntityID)
		{
			LoadProviders();
			var rtrn = _provider.GetAttributes(EntityID);
			return rtrn;
		}


		public static MetaAttributeList FindAttributes(string Name, string Value, string Schema = "") 
		{
			LoadProviders();
			var rtrn = _provider.FindAttributes(Name, Value, Schema);
			return rtrn;
		}


	 	private static void LoadProviders ()
		{
			if (_provider == null)
			{
				lock (_lock)
				{
					// Do this again to make sure _provider is still null
					if (_provider == null)
					{
						ProviderSection section = (ProviderSection)ConfigurationManager.GetSection("MetaBase");

						if (section == null)
							throw new System.Exception(string.Format("Section Not Found : {0}", "MetaBase"));


						_providers = new ProviderCollection();

						ProvidersHelper.InstantiateProviders(section.Providers, _providers, typeof(Data.DataProvider));
						_provider = (Data.DataProvider)_providers[section.DefaultProvider];

						if (_provider == null)
							throw new System.Configuration.Provider.ProviderException("Unable to load Data Provider");
					}
				}
			}
		}	   
	}
}