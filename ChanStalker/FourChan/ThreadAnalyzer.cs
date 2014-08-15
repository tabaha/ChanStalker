using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChanStalker.FourChan
{
    class ThreadAnalyzer
    {
        private List<String> Rules;
        private String ResultFile;

        public ThreadAnalyzer(List<String> rules, String resultFile)
        {
            Rules = rules;
            ResultFile = resultFile;
        }

        public void ThreadedAnalyze(Object o)
        {
            Analyze((Thread)o);
        }

        public void Analyze(Thread thread)
        {
            Console.WriteLine(thread.posts.Count);

            int count = 0;
            Console.WriteLine("Thread: " + thread.no);
            foreach (Post p in thread.posts)
            {
                List<String> results;
                if ((results = ContainsIt(p.name, p.trip, p.filename, p.com)).Count != 0)
                {
                    count++;
                    Console.Write(@"-> http://boards.4chan.org/a/thread/" + thread.no + "#p" + p.no);
                    foreach (String s in results)
                    {
                        Console.Write(" " + s);
                    }
                    Console.WriteLine();
                    WritePlainTextResults(@"http://boards.4chan.org/a/thread/" + thread.no + "#p" + p.no, results);
                }
            }
            Console.WriteLine("count: " + count);
        }


        private List<String> ContainsIt(String name, String trip, String filename, String comment)
        {
            List<String> result = new List<string>();
            foreach (String s in Rules)
            {
                if (filename != null && filename.ToLower().Contains(s))
                {
                    result.Add(s);
                }
                else if (comment != null && comment.ToLower().Contains(s))
                {
                    result.Add(s);
                }
                else if (name != null && name.ToLower().Contains(s))
                {
                    result.Add(s);
                }
                else if (trip != null && trip.ToLower().Contains(s))
                {
                    result.Add(s);
                }
            }
            return result;
        }

        private void WritePlainTextResults(String address, List<String> words)
        {
            using (StreamWriter writer = new StreamWriter(ResultFile + ".txt", true))
            {
                writer.Write(address + "\t\t");
                foreach (String s in words)
                {
                    writer.Write(s + " ");
                }
                writer.WriteLine();
            }
        }
    }
}
