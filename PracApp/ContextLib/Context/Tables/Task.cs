using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextLib.Context.Tables
{
    [Table("tasks")]
    public class Task
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("status_id")]
        public int StatusID { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
    }
}
