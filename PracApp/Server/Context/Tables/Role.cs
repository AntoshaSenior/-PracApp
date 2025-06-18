using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Context.Tables
{
    [Table("roles")]
    internal class Role
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string? Name { get; set; }
    }
}
