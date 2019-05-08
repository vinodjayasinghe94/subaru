using System;
using System.Collections.Generic;
using System.Text;

namespace Subaru.Domain.Entities
{
    public class AdgroupApps
    {
        public int AdGroupAppId { get; set; }
        public int AdGroupId { get; set; }
        public int AppId { get; set; }
        public bool? IsActive { get; set; }

        public virtual AdgroupInfo AdGroup { get; set; }
        public virtual ApplicationInfo App { get; set; }
    }
}
