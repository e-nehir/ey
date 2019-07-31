using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace goldStore.Models.ViewModel
{
    public class Login
    {
        [Display(Name="EMAİL")]
        [DataType(DataType.EmailAddress,ErrorMessage =" Geçersiz email adresi girdiniz")]
        public string email { get; set; }

        [Display(Name = "PAROLA")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="En az 6 karater giriniz")]
        [Required(ErrorMessage ="Boş geçme")]
        public string password { get; set; }

        [Display(Name = "BENİ HATIRLA")]
        public bool rememberMe { get; set; }
      
    }
}