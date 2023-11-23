using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWizPro.Models
{
    public enum UserRole
    {
        Admin,
        User,
        Manager,
        Guest // New role
              // Add other roles as needed
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive,
        Suspended, // New status
        Deleted // New status
                // Add other statuses as needed
    }
}
