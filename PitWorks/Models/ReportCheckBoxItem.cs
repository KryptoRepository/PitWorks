using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PitWorks.Models
{
    public class ReportCheckBoxItem
    {
        public string title;
        public string key;
        public List<ReportCheckBoxItem> children = null;

        public ReportCheckBoxItem()
        {

        }

        public ReportCheckBoxItem(string key, string title)
        {
            this.key = key;
            this.title = title;
        }
    }
}