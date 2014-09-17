using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChanStalker.FourChan;

namespace ChanStalker.Parsers {
    interface IParser {

        List<String> Parse(Post post);
    }
}
