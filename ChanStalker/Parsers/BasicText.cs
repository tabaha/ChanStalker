using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChanStalker.FourChan;
using System.IO;

namespace ChanStalker.Parsers {
    class BasicText : IParser {

        private List<String> Rules;
        private static String configFile = "config.ini";

        public BasicText() {
            LoadRules();
        }

        public List<String> Parse(Post post) {
            if (Rules.Count == 0) return new List<string>();
            List<String> results = new List<string>();
            foreach (String s in Rules) {
                if (post.filename != null && post.filename.ToLower().Contains(s)) {
                    results.Add(s);
                } else if (post.com != null && post.com.ToLower().Contains(s)) {
                    results.Add(s);
                } else if (post.name != null && post.name.ToLower().Contains(s)) {
                    results.Add(s);
                } else if (post.trip != null && post.trip.ToLower().Contains(s)) {
                    results.Add(s);
                }
            }
            return results;
        }

        private void LoadRules() {
            List<String> rules = new List<string>();
            if (File.Exists(configFile)) {
                using (StreamReader reader = new StreamReader(configFile)) {
                    String input;
                    while ((input = reader.ReadLine()) != null) {
                        rules.Add(input);
                    }
                }
                Rules = rules;
            } else Rules = new List<string>();
        }
    }
}
