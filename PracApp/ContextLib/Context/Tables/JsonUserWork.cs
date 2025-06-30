using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ContextLib.Context.Tables
{
    [Table("json_user_work")]
    public class JsonUserWork
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("user_id")]
        public int UserID { get; set; }
        [Column("jsonwork", TypeName = "json")]
        public string JsonWork {get; set; }
    }
}
