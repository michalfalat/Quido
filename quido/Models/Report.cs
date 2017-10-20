using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace quido.Models
{
    public class Report
    {
        public Report()
        {
            Id = Guid.NewGuid();
            //IsDeleted = false;
        }
        public Guid Id { get; set; }
        public DateTime ReportCreated { get; set; }
        public Question ReportedQuestion { get; set; }
        public string ReportText { get; set; }
        public string ReportReason { get; set; }
        public string ContactAddress { get; set; }
    }
}