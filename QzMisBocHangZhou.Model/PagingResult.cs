using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class PagingResult<T>
    {
        public int Count { get; set; }

        public List<T> Result { get; set; }
    }
}
