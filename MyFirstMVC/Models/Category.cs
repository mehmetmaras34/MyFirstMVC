using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFirstMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Proje Kategorisi")]
        public string Name { get; set; }
        [MaxLength(4000)]
        [Display(Name = "Proje Tanımı")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public virtual ICollection<Project> Projects{ get; set; }

    }
}