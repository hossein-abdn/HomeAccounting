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

    // UserRole
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class UserRoleMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
            : this("dbo")
        {
        }

        public UserRoleMap(string schema)
        {
            ToTable("UserRole", schema);
            Property(x => x.UserRoleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int");
            Property(x => x.RoleId).HasColumnName(@"RoleId").HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.Role).WithMany(b => b.UserRoles).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false); // FK_UserRole_Role
            HasRequired(a => a.User).WithMany(b => b.UserRoles).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_UserRole_User
        }
    }

}
// </auto-generated>
