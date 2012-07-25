using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace TetrisHTW.Util
{
    public class SQLClient
    {
        private string proxy;
        string postData;
        public delegate void SuccessCallback(System.Collections.Generic.List<string> playerNames, System.Collections.Generic.List<int> levels, System.Collections.Generic.List<int> scores, System.Collections.Generic.List<string> times, System.Collections.Generic.List<int> mods);
        public delegate void ErrorCallback(string msg);

        SuccessCallback cb;
        ErrorCallback ecb;

        public SQLClient(string proxy)
        {
            this.proxy = proxy;
        }

        public void writeScore(string playerName, int score, int level, long time, int mod)
        {
            time /= 10000;
            XDocument doc = new XDocument(
                new XElement("scoreentry",
                    new XElement("playername", playerName),
                    new XElement("score", score),
                    new XElement("level", level),
                    new XElement("time", time),
                    new XElement("mod", mod)
                )
            );
            postData = doc.ToString();
            try
            {
                WebRequest request = WebRequest.Create(proxy);
                request.Method = "POST";

                request.ContentType = "text/xml";
                request.BeginGetRequestStream(new AsyncCallback(RequestReady), request);
            }
            catch (Exception e)
            { Debug.WriteLine(e.Message); }
            
        }

        void RequestReady(IAsyncResult asyncResult)
        {
            HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
            using (Stream stream = request.EndGetRequestStream(asyncResult))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                stream.Write(bytes, 0, bytes.Length);
                
            }
            request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
        }

        void ResponseReady(IAsyncResult asyncResult)
        {
            HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
            using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult))
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        Debug.WriteLine(reader.ReadToEnd());
                    }
                }
            }
        }

        internal void requestScores(SuccessCallback cb, ErrorCallback ecb,  int lines)
        {
            this.cb = cb;
            this.ecb = ecb;
            try
            {
                WebClient get = new WebClient();


                get.DownloadStringCompleted += client_DownloadStringCompleted;

                get.DownloadStringAsync(new Uri(proxy + "?lines=" + lines));
            }
            catch (Exception e)
            { Debug.WriteLine(e.Message); }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            if (e.Error == null)
            {
                List<string> playerNames = new List<string>();
                List<int> levels = new List<int>();
                List<int> scores = new List<int>();
                List<string> times = new List<string>();
                List<int> mods = new List<int>();

                XElement root = XElement.Parse(e.Result);
                
                foreach (XElement scoreentry in root.Nodes())
                {
                    string playerName = (string)scoreentry.Element("playername");
                    int level = int.Parse((string)scoreentry.Element("level"));
                    int score = int.Parse((string)scoreentry.Element("score"));
                    string time = (string)scoreentry.Element("time");
                    int mod = int.Parse((string)scoreentry.Element("mod"));

                    playerNames.Add(playerName);
                    levels.Add(level);
                    scores.Add(score);
                    times.Add(time);
                    mods.Add(mod);
                }
                cb(playerNames, levels, scores, times, mods);
            }
            else
                ecb(e.Error.Message);
        }

    }
}
