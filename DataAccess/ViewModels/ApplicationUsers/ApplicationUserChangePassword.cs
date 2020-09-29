using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.ViewModels.ApplicationUsers
{
    public class ApplicationUserChangePassword
    {
        public string UserId{ get; set; }

        [Display(Name = "Κωδικός")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Ο κωδικός πρέπει να απαρτίζεται απο τουλάχιστον 8 χαρ. με κεφαλαία - πεζά - σύμβολα - αριθμούς - συμβολα")]
        public string Password1 { get; set; }

        [Display(Name = "Επανάληψη κωδικού")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Ο κωδικός πρέπει να απαρτίζεται απο τουλάχιστον 8 χαρ. με κεφαλαία - πεζά - σύμβολα - αριθμούς - συμβολα")]
        public string Password2 { get; set; }

        public string ReturnUrl { get; set; }

    }
}
