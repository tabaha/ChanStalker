using ChanStalker.FourChan;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Collections;

namespace ChanStalker
{
    class Program
    {
        static String configFile = "config.ini";

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
            List<String> hue = LoadSettings();
            if (hue == null) return;

            String basefilename = ((long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            Console.WriteLine("filename: " + basefilename);

            String result = GetJsonFromUrl(@"http://a.4cdn.org/a/threads.json");


            List<Page> pages = JsonConvert.DeserializeObject<List<Page>>(result);


            System.Threading.Thread.Sleep(2000);

            ThreadAnalyzer ta = new ThreadAnalyzer(hue, basefilename);

            foreach (Thread t in pages[0].threads)
            {
                String res = GetJsonFromUrl(@"http://a.4cdn.org/a/thread/" + t.no + ".json");

                Thread newT = JsonConvert.DeserializeObject<Thread>(res);
                newT.no = t.no;

                (new System.Threading.Thread(ta.ThreadedAnalyze)).Start(newT);

                System.Threading.Thread.Sleep(2000);

            }

            Console.ReadLine();
        }

        static List<String> LoadSettings()
        {
            if (File.Exists(configFile))
            {
                List<String> result = new List<string>();

                using (StreamReader reader = new StreamReader(configFile))
                {
                    String input;
                    while ((input = reader.ReadLine()) != null)
                    {
                        result.Add(input);
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
