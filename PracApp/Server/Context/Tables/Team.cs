using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Context.Tables
{
    [Table("teams")]
    internal class Team
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("team")]
        public string? Name { get; set; }
    }
}
