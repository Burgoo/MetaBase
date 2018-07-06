using Burgoo.MetaBase.Objects;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data;

namespace Burgoo.MetaBase.Data 
{
	public abstract class DataProvider : ProviderBase 
	{
		public abstract string ApplicationName{get;set;}

		public abstract void Open();
		public abstract void Close();	
		public abstract void CreateSchema(string Schema = "");
		public abstract Guid CreateEntity(string TypeName, string Schema = "");
		public abstract Guid CreateAttribute<t>(Guid EntityID, string Name, t Value, string Schema = "");		
		public abstract GuidList GetEntities (string TypeName, string Schema = "");
		public abstract MetaAttributeList GetAttributes(GuidList EntityIDs, List<string> Names = null, string Schema = "");


		//public abstract Objects.MetaAttribute GetAttributeByID(Guid AttributeID, string Schema = "");
		

		public abstract Objects.MetaAttributeList GetAttribute(Guid EntityID, List<string> Name, string Schema = "");
		public abstract MetaAttributeList FindAttributes(string Name, string Value, string Schema = "");
		public abstract DataSet Export(string Schema = "");
		public abstract void Import(DataSet ds, string Schema = "");

	}
}
