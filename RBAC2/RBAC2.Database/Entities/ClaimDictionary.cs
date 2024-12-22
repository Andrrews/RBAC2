using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAC2.Database.Entities
{
    public class ClaimDictionary
    {
        public int ClaimId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
