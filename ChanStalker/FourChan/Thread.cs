using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChanStalker.FourChan
{
    class Thread
    {
        public int no { get; set; }
        public int last_modified { get; set; }
        public List<Post> posts { get; set; }
    }
}
