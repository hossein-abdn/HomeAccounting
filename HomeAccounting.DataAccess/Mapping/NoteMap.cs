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

    // Note
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class NoteMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Note>
    {
        public NoteMap()
            : this("dbo")
        {
        }

        public NoteMap(string schema)
        {
            ToTable("Note", schema);
            Property(x => x.NoteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar");
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsOptional();
            Property(x => x.StatusId).HasColumnName(@"StatusId").HasColumnType("int");
            Property(x => x.NotificationId).HasColumnName(@"NotificationId").HasColumnType("int").IsOptional();
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime");
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int");
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");

            // Foreign keys
            HasOptional(a => a.Notification).WithMany(b => b.Notes).HasForeignKey(c => c.NotificationId).WillCascadeOnDelete(false); // FK_Note_Notification
            HasRequired(a => a.User).WithMany(b => b.Notes).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_Note_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
