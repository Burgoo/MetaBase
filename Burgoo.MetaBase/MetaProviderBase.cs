using Burgoo.MetaBase.Objects;
using System;
using System.Configuration.Provider;
using System.Data;

namespace Burgoo.MetaBase 
{
	public abstract class MetaProviderBase : ProviderBase 
	{
		public abstract string ApplicationName{get;set;}

		//
		// abstract functions
		//
		public abstract void Open();
		public abstract void Close();

		public abstract void CreateSchema(string Schema = "");
		public abstract Guid CreateEntity(string TypeName, string Schema = "");
		public abstract Guid CreateAttribute<t>(Guid EntityID, string Name, t Value, string Schema = "");

		//
		// returns a list f guids representing the type names
		//
		public abstract GuidList GetEntities (string TypeName, string Schema = "");
		public abstract MetaAttributeList GetAttributes(GuidList EntityIDs, string Schema = "");
		public abstract MetaAttributeList FindAttributes(string Name, string Value, string Schema = "");

		//
		//
		//
		public abstract DataSet ExportTables(string Schema);
	}
}
