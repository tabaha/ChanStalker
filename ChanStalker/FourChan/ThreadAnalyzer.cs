using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ChanStalker.Parsers;

namespace ChanStalker.FourChan
{
    class ThreadAnalyzer
    {
        private String ResultFile;
        private List<IParser> Parsers;

        public ThreadAnalyzer(String resultFile)
        {
            ResultFile = resultFile;
            Parsers = new List<IParser>();
            LoadParsers();
        }

        private void LoadParsers() {
            //Parsers.Add(new BasicText());
            Parsers.Add(new ExHentai());
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
                if(p.com!=null) p.com = p.com.Replace("<wbr>", "").Replace("</wbr>", "");
                List<String> results;
                if ((results = ContainsIt(p)).Count != 0)
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


        private List<String> ContainsIt(Post post)
        {
            List<String> results = new List<string>();

            foreach (IParser parser in Parsers) {
                results.AddRange(parser.Parse(post));
            }

            return results;
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
