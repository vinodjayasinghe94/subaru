using System;
using System.Collections.Generic;

namespace Subaru.Presistance.Models
{
    public partial class AdgroupInfo
    {
        public AdgroupInfo()
        {
            AdgroupApps = new HashSet<AdgroupApps>();
        }

        public int AdgroupId { get; set; }
        public string AdgroupName { get; set; }

        public virtual ICollection<AdgroupApps> AdgroupApps { get; set; }
    }
}
