using StoreSolution.Core.Models.Interface;
using System.ComponentModel.DataAnnotations;

namespace StoreSolution.Core.Models.Base
{
    public class BaseEntity : IAuditableEntity
    {
        public int Id { get; set; }

        [MaxLength(40)]
        public string? CreatedBy { get; set; }

        [MaxLength(40)]
        public string? UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
