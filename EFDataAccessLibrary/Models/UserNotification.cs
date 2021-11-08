using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class UserNotification
    {
        public ApplicationUser User { get; set; }
        public Notification Notification { get; set; }
        public string UserID { get; set; }
        public int NotificationID { get; set; }
    }
}
