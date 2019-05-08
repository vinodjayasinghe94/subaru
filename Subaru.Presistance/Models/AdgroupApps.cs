    using System;
using System.Collections.Generic;

namespace Subaru.Presistance.Models
{
    public partial class AdgroupApps
    {
        public int AdGroupAppId { get; set; }
        public int AdGroupId { get; set; }
        public int AppId { get; set; }
        public bool? IsActive { get; set; }

        public virtual AdgroupInfo AdGroup { get; set; }
        public virtual ApplicationInfo App { get; set; }
    }
}
