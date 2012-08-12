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
        private string postData;
        private Random rnd = new Random();
        public delegate void SuccessCallback(System.Collections.Generic.List<string> playerNames, System.Collections.Generic.List<int> levels, System.Collections.Generic.List<int> scores, System.Collections.Generic.List<string> times, System.Collections.Generic.List<int> modes, System.Collections.Generic.List<int> ranks);
        public delegate void SuccessCallbackEntry(int rank);
        public delegate void ErrorCallback(string msg);


        SuccessCallback cb;
        SuccessCallbackEntry cbe;
        ErrorCallback ecb;

        public SQLClient(string proxy)
        {
            this.proxy = proxy;
        }

        public void writeScore(ErrorCallback ecb, string playerName, int score, int level, long time, int mode)
        {
            time /= 10000;
            this.ecb = ecb;
            XDocument doc = new XDocument(
                new XElement("scoreentry",
                    new XElement("playername", playerName),
                    new XElement("score", score),
                    new XElement("level", level),
                    new XElement("time", time),
                    new XElement("mode", mode)
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
            { ecb(e.Message); }
            
        }

        void RequestReady(IAsyncResult asyncResult)
        {
            try
            {
                HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
                using (Stream stream = request.EndGetRequestStream(asyncResult))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(postData);
                    stream.Write(bytes, 0, bytes.Length);

                }
                request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
            }
            catch (Exception e)
            {
                ecb(e.Message);
            }
        }

        void ResponseReady(IAsyncResult asyncResult)
        {
            try
            {
                HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
                using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult))
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string str = reader.ReadToEnd();
                            if (str.Contains("error"))
                            {
                                ecb(str);
                            }
                        }
                    }
                }
            }   
            catch (Exception e)
            {
                ecb(e.Message);
            }
        }

        internal void requestScoreEntry(SuccessCallbackEntry cbe, ErrorCallback ecb, int score, int mode)
        {
            this.cbe = cbe;
            this.ecb = ecb;
            try
            {
                WebClient get = new WebClient();

                get.DownloadStringCompleted += client_DownloadStringCompletedEntry;

                get.DownloadStringAsync(new Uri(proxy + "?mode=" + mode + "&score=" + score + "&random=" + rnd.Next(int.MaxValue)));
            }
            catch (Exception e)
            { if (App.DEBUG) Debug.WriteLine(e.Message); }
        }

        internal void requestScores(SuccessCallback cb, ErrorCallback ecb, int rows, int mode)
        {
            this.cb = cb;
            this.ecb = ecb;
            try
            {
                WebClient get = new WebClient();

                get.DownloadStringCompleted += client_DownloadStringCompleted;

                get.DownloadStringAsync(new Uri(proxy + "?count=" + rows + "&mode=" + mode + "&random=" + rnd.Next(int.MaxValue)));
            }
            catch (Exception e)
            { if (App.DEBUG) Debug.WriteLine(e.Message); }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<string> playerNames = new List<string>();
                List<int> levels = new List<int>();
                List<int> scores = new List<int>();
                List<string> times = new List<string>();
                List<int> modes = new List<int>();
                List<int> ranks = new List<int>();
                try
                {
                    XElement scoreentries = XElement.Parse(e.Result);

                    foreach (XElement scoreentry in scoreentries.Nodes())
                    {
                        string playerName = (string)scoreentry.Element("playername");
                        int level = int.Parse((string)scoreentry.Element("level"));
                        int score = int.Parse((string)scoreentry.Element("score"));
                        string time = (string)scoreentry.Element("time");
                        int mode = int.Parse((string)scoreentry.Element("mode"));
                        int rank = int.Parse((string)scoreentry.Element("rank"));

                        playerNames.Add(playerName);
                        levels.Add(level);
                        scores.Add(score);
                        times.Add(time);
                        modes.Add(mode);
                        ranks.Add(rank);
                    }
                    cb(playerNames, levels, scores, times, modes, ranks);
                }
                catch (Exception exc)
                {
                    ecb(exc.Message + " Fehler, eventuell ist der Server nicht erreichbar.");
                }
            }
            else
                if (e.Error.Message == null || e.Error.Message.Equals(""))
                {
                    ecb(e.Error.ToString());
                } 
                else
                {
                    ecb(e.Error.Message);
                }
        }

        void client_DownloadStringCompletedEntry(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    XElement scoreentries = XElement.Parse(e.Result);

                    foreach (XElement scoreentry in scoreentries.Nodes())
                    {
                        int rank = int.Parse((string)scoreentry.Element("rank"));

                        cbe(rank);
                        break; //ein entry reicht.
                    }
                }
                catch (Exception exc)
                {
                    ecb(exc.Message + " Fehler, eventuell ist der Server nicht erreichbar.");
                }
            }
            else
                if (e.Error.Message == null || e.Error.Message.Equals(""))
                {
                    ecb(e.Error.ToString());
                }
                else
                {
                    ecb(e.Error.Message);
                }
        }

    }
}
