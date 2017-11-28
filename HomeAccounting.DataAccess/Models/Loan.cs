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

    // Loan
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Loan
    {

        ///<summary>
        /// جدول وام
        ///</summary>
        [Column(@"LoanId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int LoanId { get; set; } // LoanId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "عنوان")]
        public string Title { get; set; } // Title (length: 50)

        [Required]
        [Display(Name = "مبلغ")]
        public int Amount { get; set; } // Amount

        public int? TransactionId { get; set; } // TransactionId

        [Required]
        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime CreateDate { get; set; } // CreateDate

        [Required]
        [Display(Name = "تاریخ شروع")]
        public System.DateTime StartDate { get; set; } // StartDate

        [Required]
        [Display(Name = "تعداد پرداخت شده")]
        public int PaidInstallment { get; set; } // PaidInstallment

        [Required]
        [Display(Name = "تعداد باقیمانده")]
        public int RemainInstallment { get; set; } // RemainInstallment

        [Required]
        [Display(Name = "وضعیت تسویه")]
        public int SettleStatusId { get; set; } // SettleStatusId

        [Required]
        [Display(Name = "مبلغ پرداخت شده")]
        public int PaidSettle { get; set; } // PaidSettle

        [Required]
        [Display(Name = "مبلغ باقیمانده")]
        public int RemainSettle { get; set; } // RemainSettle

        [Required]
        public int UserId { get; set; } // UserId

        [Required]
        [Display(Name = "وضعیت")]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child Installments where [Installment].[LoanId] point to this entity (FK_Installment_Loan)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Installment> Installments { get; set; } // Installment.FK_Installment_Loan
        /// <summary>
        /// Child LoanLabels where [LoanLabel].[LoanId] point to this entity (FK_LoanLabel_Loan)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<LoanLabel> LoanLabels { get; set; } // LoanLabel.FK_LoanLabel_Loan

        // Foreign keys

        /// <summary>
        /// Parent Transaction pointed by [Loan].([TransactionId]) (FK_Loan_Transaction)
        /// </summary>
        public virtual Transaction Transaction { get; set; } // FK_Loan_Transaction

        /// <summary>
        /// Parent User pointed by [Loan].([UserId]) (FK_Loan_User)
        /// </summary>
        public virtual User User { get; set; } // FK_Loan_User

        public Loan()
        {
            PaidInstallment = 0;
            RemainInstallment = 0;
            PaidSettle = 0;
            Installments = new System.Collections.Generic.List<Installment>();
            LoanLabels = new System.Collections.Generic.List<LoanLabel>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
