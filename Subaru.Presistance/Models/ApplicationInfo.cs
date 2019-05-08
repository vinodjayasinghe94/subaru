using System;
using System.Collections.Generic;

namespace Subaru.Presistance.Models
{
    public partial class ApplicationInfo
    {
        public ApplicationInfo()
        {
            AdgroupApps = new HashSet<AdgroupApps>();
        }

        public int AppId { get; set; }
        public string AppName { get; set; }
        public string AppUrl { get; set; }
        public string AppAuthType { get; set; }
        public bool? IsActive { get; set; }
        public string AppIconPath { get; set; }

        public virtual ICollection<AdgroupApps> AdgroupApps { get; set; }
    }
}
