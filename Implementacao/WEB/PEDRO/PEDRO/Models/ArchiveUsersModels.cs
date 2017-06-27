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
        public string nomeDoArquivo { get; set; }
        [Required]
        [Display(Name = "Tamanho")]
        public double tamanhoArquivo { get; set; }
        [Required]
        [Display(Name = "Tipo de Arquivo")]
        public String extensao { get; set; }
        [Required]
        [Display(Name = "Data de upload")]
        public DateTime dataUpload { get; set; }
        public string hashFileName { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}