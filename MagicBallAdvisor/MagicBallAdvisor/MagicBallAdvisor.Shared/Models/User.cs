using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicBallAdvisor.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Unique, MaxLength(150)]
        public string Name { get; set; }
    }
}
