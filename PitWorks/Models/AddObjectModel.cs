using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PitWorks.Models
{
    public class AddObjectModel
    {
        public IEnumerable<ЛПУ> lpuList;
        public IEnumerable<Участок_газопровода> tubeList;
    }
}