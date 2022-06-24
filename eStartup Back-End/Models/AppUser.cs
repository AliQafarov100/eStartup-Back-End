using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace eStartup_Back_End.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(maximumLength:15)]
        public string FirstName { get; set; }
        [StringLength(maximumLength: 20)]
        public string LastName { get; set; }
    }
}
