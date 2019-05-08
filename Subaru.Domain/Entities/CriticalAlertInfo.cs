using System;
using System.Collections.Generic;
using System.Text;

namespace Subaru.Domain.Entities
{
    public class CriticalAlertInfo
    {
        public CriticalAlertInfo()
        {
            AuditCriticalAlertInfo = new HashSet<AuditCriticalAlertInfo>();
        }

        public int AlertId { get; set; }
        public string DisplayText { get; set; }
        public bool? IsVisible { get; set; }
        public bool IsControlledByDateRange { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual ICollection<AuditCriticalAlertInfo> AuditCriticalAlertInfo { get; set; }
    }
}
