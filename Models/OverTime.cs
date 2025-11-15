using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTXYZ_OvertimeApp.Models
{
    [Table("OverTime")]
    public class OverTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OverTimeId { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Time Start is required")]
        [Display(Name = "Time Start")]
        public DateTime TimeStart { get; set; }

        [Required(ErrorMessage = "Time Finish is required")]
        [Display(Name = "Time Finish")]
        public DateTime TimeFinish { get; set; }

        [Display(Name = "Actual OT Hours")]
        [Column(TypeName = "decimal")]
        public decimal ActualOTHours { get; set; }

        [Display(Name = "Calculated OT Hours")]
        [Column(TypeName = "decimal")]
        public decimal CalculatedOTHours { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}
