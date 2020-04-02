using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class EditViewModel<T>
    {
        public T Data { get; set; }

        public UserListViewModel User { get; set; }
    }
}
