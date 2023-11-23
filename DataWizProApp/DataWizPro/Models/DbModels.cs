using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWizPro.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public bool available { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public DateTime created_at  { get; set; }
        public string status { get; set; }
    }

    public class UserExtended
    {
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public int Age { get; set; } // New property
        public UserStatus Status { get; set; } // New property
                                               // Other properties...
    }
}
