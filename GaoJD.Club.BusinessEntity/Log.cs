using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GaoJD.Club.BusinessEntity
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Type { get; set; }

        public string Opbusinessline { get; set; }

        public int ApplicationID { get; set; }

        public string Application { get; set; }

        public string Message { get; set; }

        public DateTime CreateTime { get; set; }

        public string Extras { get; set; }
    }
}
