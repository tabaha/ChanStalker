using ChanStalker.FourChan;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace ChanStalker
{
    class Program
    {
        

        static String GetJsonFromUrl(String url)
        {
            WebRequest reqThreads = WebRequest.Create(url);
            reqThreads.Proxy = null;

            String result;
            using (WebResponse response = reqThreads.GetResponse())
            {
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                {
                    result = responseStream.ReadToEnd();
                }
            }
            return result;
        }

        static void Main(string[] args)
        {

            String basefilename = ((long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            Console.WriteLine("filename: " + basefilename);

            String result = GetJsonFromUrl(@"http://a.4cdn.org/a/threads.json");


            List<Page> pages = JsonConvert.DeserializeObject<List<Page>>(result);


            System.Threading.Thread.Sleep(1500);

            ThreadAnalyzer ta = new ThreadAnalyzer(basefilename);
            foreach (Page page in pages)
            {
                foreach (Thread t in /*pages[0].threads*/ page.threads)
                {
                    String res = GetJsonFromUrl(@"http://a.4cdn.org/a/thread/" + t.no + ".json");

                    Thread newT = JsonConvert.DeserializeObject<Thread>(res);
                    newT.no = t.no;

                    (new System.Threading.Thread(ta.ThreadedAnalyze)).Start(newT);

                    System.Threading.Thread.Sleep(1500);

                }
            }
            Console.WriteLine("Done. Press Enter to close");
            Console.ReadLine();
        }

       
    }
}
