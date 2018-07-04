using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SQLite;

namespace MetaBase
{
	class Context : DbContext 
	{

		public MetaProviderBase DataProvider { get; set; } 

		public Context(MetaProviderBase D) : base(D.ConnectionString)
		{
			DataProvider = D;
		}


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			this.Configuration.LazyLoadingEnabled = false ;


			var tblEnt = modelBuilder.Entity<Objects.MetaEntity>();

			tblEnt.ToTable(string.Format("{0}entity", this.DataProvider.TablePrefix));
			tblEnt.Property(x => x.ID).HasColumnName(string.Format("{0}ID", this.DataProvider.ColumnPrefix)).IsRequired();
			tblEnt.Property(x => x.Type).HasColumnName(string.Format("{0}Type", this.DataProvider.ColumnPrefix));

			var tblAttr = modelBuilder.Entity<Objects.MetaAttribute >();

			tblAttr.ToTable(string.Format("{0}entity", this.DataProvider.TablePrefix));
			tblAttr.Property(x => x.ID).HasColumnName(string.Format("{0}ID", this.DataProvider.ColumnPrefix)).IsRequired();
			tblAttr.Property(x => x.EntityID ).HasColumnName(string.Format("{0}EntityID", this.DataProvider.ColumnPrefix));
			tblAttr.Property(x => x.Name).HasColumnName(string.Format("{0}Name", this.DataProvider.ColumnPrefix));
			tblAttr.Property(x => x.Value).HasColumnName(string.Format("{0}Value", this.DataProvider.ColumnPrefix));

		}


		public DbSet<Objects.MetaEntity> MetaEntity { get; set; }
		public DbSet<Objects.MetaAttribute> MetaAttribute { get; set; }



	}
}
