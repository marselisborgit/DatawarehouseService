using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Table("User", Schema = "dbo")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(40)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        public string UserIdentifier { get; set; }

        [StringLength(255)]
        public string CaseWorkerIdentifier { get; set; }

        [Required]
        [StringLength(256)]
        //[RegularExpression(@"([^>\(\)\[\]\\,;:@\s]{0,191}@[^>\(\)\[\]\\,;:@\s]{1,64})")]
        public string Email { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        public DateTime? Deleted { get; set; }

        public bool ShowIntro { get; set; }

        public int? DefaultEntityTypeId { get; set; }

        //[ForeignKey("DefaultEntityTypeId")]
       // public virtual EntityType DefaultEntityType { get; set; }

        // Activity.Participants
        //public virtual ICollection<Activity> Activities { get; set; }

        [StringLength(10)]
        public string CultureName { get; set; }

        public int? DepartmentId { get; set; }

       // [ForeignKey("DepartmentId")]
        //public Department Department { get; set; }

        [Column("Department")]
        public string DepartmentName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
