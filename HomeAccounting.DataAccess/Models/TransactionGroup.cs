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

namespace HomeAccounting.DataAccess.Models
{
    using Mapping;

    // TransactionGroup
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class TransactionGroup
    {

        ///<summary>
        /// جدول گروه های تراکنش
        ///</summary>
        [Column(@"TransactionGruopId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int TransactionGruopId { get; set; } // TransactionGruopId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        public string Title { get; set; } // Title (length: 50)

        [Required]
        public int TypeId { get; set; } // TypeId

        public int? ParentId { get; set; } // ParentId

        [Required]
        public int UserId { get; set; } // UserId

        [Required]
        public System.DateTime CreateDate { get; set; } // CreateDate

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child Transactions where [Transaction].[TransactionGroupId] point to this entity (FK_Transaction_TransactionGroup)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Transaction> TransactionGroupId { get; set; } // Transaction.FK_Transaction_TransactionGroup
        /// <summary>
        /// Child Transactions where [Transaction].[TransactionGroupParentId] point to this entity (FK_Transaction_TransactionGroup_Parent)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Transaction> TransactionGroupParent { get; set; } // Transaction.FK_Transaction_TransactionGroup_Parent
        /// <summary>
        /// Child TransactionGroups where [TransactionGroup].[ParentId] point to this entity (FK_TransactionGroup_TransactionGroup)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<TransactionGroup> TransactionGroups { get; set; } // TransactionGroup.FK_TransactionGroup_TransactionGroup

        // Foreign keys

        /// <summary>
        /// Parent TransactionGroup pointed by [TransactionGroup].([ParentId]) (FK_TransactionGroup_TransactionGroup)
        /// </summary>
        public virtual TransactionGroup Parent { get; set; } // FK_TransactionGroup_TransactionGroup

        /// <summary>
        /// Parent User pointed by [TransactionGroup].([UserId]) (FK_TransactionGroup_User)
        /// </summary>
        public virtual User User { get; set; } // FK_TransactionGroup_User

        public TransactionGroup()
        {
            TransactionGroupId = new System.Collections.Generic.List<Transaction>();
            TransactionGroupParent = new System.Collections.Generic.List<Transaction>();
            TransactionGroups = new System.Collections.Generic.List<TransactionGroup>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
