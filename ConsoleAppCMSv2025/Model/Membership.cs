using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Model
{
    public class Membership
    {
        public int MembershipId { get; set; }
        public string MembershipType { get; set; }
        public bool IsActive { get; set; }
    }
}