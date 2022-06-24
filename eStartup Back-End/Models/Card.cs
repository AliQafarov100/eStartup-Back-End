using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace eStartup_Back_End.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
