using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PEDRO.Models
{
    public class SharedArchiveModel
    {
        [Key]
        public int id { get; set; }

        public string proprietario_id { get; set; }

        public string usuario_id { get; set; }

        public int arquivo_id { get; set; }
    }
}