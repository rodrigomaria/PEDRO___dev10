using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PEDRO.Models
{
    public class ArchiveUsersModels
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Arquivo")]
        private string nomeDoArquivo { get; set; }
        [Required]
        [Display(Name = "Tamanho")]
        private double tamanhoArquivo { get; set; }
        [Required]
        [Display(Name = "Tipo de Arquivo")]
        private String tipoArquivo { get; set; }
        [Required]
        [Display(Name = "Data de upload")]
        private DateTime dataUpload { get; set; }
        public virtual ApplicationUser user { get; set; }
        public virtual List<CloudModel> servicosNuvem { get; set; }
    }
}