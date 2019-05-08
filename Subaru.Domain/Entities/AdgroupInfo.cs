using System;
using System.Collections.Generic;
using System.Text;

namespace Subaru.Domain.Entities
{
    public class AdgroupInfo
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
