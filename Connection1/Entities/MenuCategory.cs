using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection1.Entities
{
    public class MenuCategory
    {
        [Key]
        public int CategId { get; set; }
        public string CategName { get; set; }
        public string TagLine { get; set; }
        public DateTime? AddedDate { get; set; }
        public string CategImagePath { get; set; } = "";
    }
}
