using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PitWorks.Models
{
    public class ReportObjectModel
    {
        public Объект pitObject;
        public string sectionTube; //наименование газопровода
        public string lpuName;
        public List<ReportDefectModel> defects = new List<ReportDefectModel>();

    }
}
