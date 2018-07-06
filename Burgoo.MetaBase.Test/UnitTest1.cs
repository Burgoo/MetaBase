using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Burgoo.MetaBase.Configuration;
using Burgoo.MetaBase.Objects;
using System.Diagnostics;
using System.Data;

namespace MetaBase.Test
{
	[TestClass]
	public class UnitTest1
	{
		[ClassInitialize]
		public static void ClassInitialize(TestContext tc)
		{
			MetaProvider.Open();		
		}

		[ClassCleanup]
		public static void ClassCleanup( )
		{
			var ds = MetaProvider.Export();
			var writer = new System.IO.StringWriter();
			ds.WriteXml(writer);
			Trace.WriteLine (writer.ToString()); 
			MetaProvider.Close();
		}

		[TestMethod]
		public void CreateEntity()
		{
			var rtrn = MetaProvider.CreateEntity("User");
			MetaProvider.CreateAttribute(rtrn, "LoginID", "rbforee");

			Trace.WriteLine(rtrn);
		}


		[TestMethod]
		public void GetEntity()
		{
			var rtrn = MetaProvider.GetEntities("User");

			var ml = MetaProvider.GetAttributes(rtrn);

			foreach (Guid k in ml.Keys)
			{
				Trace.WriteLine(string.Format("{0} {1} {2} {3}", ml[k].ID, ml[k].EntityID, ml[k].Name, ml[k].Value));
			}
		}



		[TestMethod]
		public void FindAttributes()
		{
			var ml = MetaProvider.FindAttributes("LoginID", "rbforee" );

			foreach (Guid k in ml.Keys)
			{
				Trace.WriteLine(string.Format("{0} {1} {2} {3}", ml[k].ID, ml[k].EntityID, ml[k].Name, ml[k].Value));
			}
		}


		[TestMethod]
		public void TC_1_1_SaveUserObject()
		{
			var u = new  Burgoo.MetaBase.Test.Objects.User() {LoginID = "rbforee" , Age =  43, DOB = new DateTime(1974,10,24) };
					
			MetaProvider.SaveObject(u);

		}



		[TestMethod]
		public void TC_1_2_GetUserObject()
		{
			var rtrn = MetaProvider.FindAttributes("LoginID", "rbforee");

			foreach (Guid i in rtrn.Keys )
			{
				var user = MetaProvider.GetObject<Burgoo.MetaBase.Test.Objects.User>(i);

				Assert.IsNotNull(user);

				Trace.WriteLine(string.Format("{0} = {1}", "LoginID", user.LoginID ));
			}
		}	
	}
}
