using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextLib.Context.Tables
{
    [Table("users_to_project")]
    public class UserToProject
    {
        [Column("id_user ")]
        public int IdUser { get; set; }
        [Column("id_project")]
        public int IdProject { get; set; }
    }
}
