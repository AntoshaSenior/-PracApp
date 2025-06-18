using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Context.Tables
{
    [Table("users")]
    internal class User
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("username")]
        public string? Username { get; set; }
        [Column("password_hash")]
        public string? Password { get; set; }
        [Column("fname")]
        public string? FName { get; set; }
        [Column("mname")]
        public string? MName { get; set; }
        [Column("lname")]
        public string? LName { get; set; }
        [Column("role_id")]
        public int RoleID { get; set; }
        [Column("team_id")]
        public int TeamID { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("is_active")]
        public bool isActive { get; set; }
    }
}
