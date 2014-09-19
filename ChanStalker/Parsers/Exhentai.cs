using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChanStalker.FourChan;
using System.Text.RegularExpressions;

namespace ChanStalker.Parsers {
    class ExHentai : IParser{

        
        private Regex ExHentaiMatcher;
        public ExHentai() {
            ExHentaiMatcher = new Regex(@"(http://)?exhentai.org/((g/[0-9]*/[A-Za-z0-9]*/)|s/[A-Za-z0-9]*/[0-9]*-[0-9]*)");
        }

        public List<String> Parse(Post post) {
            List<String> results = new List<string>();
            if (post.com == null) return results;
            MatchCollection matches = ExHentaiMatcher.Matches(post.com);
            foreach (Match match in matches) {
                Console.WriteLine(match.Value);
                results.Add(match.Value);
            }
            return results;
        }

    }
}
