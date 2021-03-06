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

    // V_UserRole
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class V_UserRoleMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<V_UserRole>
    {
        public V_UserRoleMap()
            : this("dbo")
        {
        }

        public V_UserRoleMap(string schema)
        {
            ToTable("V_UserRole", schema);
            Property(x => x.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.UserName).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RoleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RoleTitle).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
