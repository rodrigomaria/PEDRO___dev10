using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PEDRO.Models
{
    public class ArquivosUsuariosModels
    {
        [Required]
        [Display(Name = "Arquivo")]
        private string nomeDoArquivo { get; set; }
        [Required]
        [Display(Name = "Data de upload")]
        private DateTime dataUpload { get; set; }
    }
}