using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourStocks.Models
{
    public class Stock
    {
        [Key]
        public string id { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено!")]
        public DateTime Date { get; set; }        
        [Required(ErrorMessage = "Поле должно быть установлено!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено!")]
        public double Cost { get;set;}
    }

  
    
}
