using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Context.Tables
{
    [Table("projects")]
    internal class Project
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string? name { get; set; }
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("deadline")]
        public DateTime DeadLine { get;set; }
        [Column("status_id")]
        public int StatusID {  get; set; }

    }
}
