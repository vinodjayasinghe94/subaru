using System;
using System.Collections.Generic;
using System.Text;

namespace Subaru.Domain.Entities
{
    public class AuditCriticalAlertInfo
    {
        public int AuditId { get; set; }
        public int AlertId { get; set; }
        public string DisplayText { get; set; }
        public bool IsVisible { get; set; }
        public bool IsControlledByDateRange { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual CriticalAlertInfo Alert { get; set; }
    }
}
