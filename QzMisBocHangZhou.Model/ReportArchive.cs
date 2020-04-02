using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class ReportArchive
    {
        public ArchiveStatusType Status { get; set; } = ArchiveStatusType.草稿;

        public int Total { get; set; }

        public string StatusName
        {
            get
            {
                return Status.ToString();
            }
        }
    }
}
