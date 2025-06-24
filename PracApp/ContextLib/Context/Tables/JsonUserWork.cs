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
        public int ID { get; set; }
        public int UserID { get; set; }
        public string JsonWork {get; set; }
    }
}
