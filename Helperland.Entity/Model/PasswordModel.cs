using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helperland.Entity.Model
{
    public class PasswordModel
    {
        public string? Email { get; set; }

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }

        public bool IsError { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
