// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAccounting.DataAccess.Mapping
{
    using Models;

    // Logs
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class LogMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Log>
    {
        public LogMap()
            : this("dbo")
        {
        }

        public LogMap(string schema)
        {
            ToTable("Logs", schema);
            Property(x => x.LogId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CallSite).HasColumnName(@"CallSite").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Level).HasColumnName(@"Level").HasColumnType("nvarchar").IsOptional();
            Property(x => x.Type).HasColumnName(@"Type").HasColumnType("nvarchar").IsOptional();
            Property(x => x.Message).HasColumnName(@"Message").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.Exception).HasColumnName(@"Exception").HasColumnType("nvarchar(max)").IsOptional();
            Property(x => x.PersianDate).HasColumnName(@"PersianDate").HasColumnType("nvarchar").IsOptional();
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int").IsOptional();
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
