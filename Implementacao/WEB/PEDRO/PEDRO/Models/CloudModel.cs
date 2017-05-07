using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PEDRO.Models
{
    public class CloudModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Nome do serviço")]
        public string nome { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email utilizado no serviço")]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string pass { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("pass", ErrorMessage = "As senhas informadas não são iguais.")]
        public string confirmPass { get; set; }
    }
}