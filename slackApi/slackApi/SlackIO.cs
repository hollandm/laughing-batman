using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading;


namespace slackApi
{
    class SlackIO
    {

        public string username = "Wall-E Pilot";
        public string iconUrl = "http://www.robotbooks.com/wall-e.jpg";

        public string defaultChannelId = null;

        private string token = getToken();

        public CommandProcessor cmdProc;

        public SlackIO()
        {
            cmdProc = new CommandProcessor(new Action<string>(postMessage));


            string url = "https://slack.com/api/channels.list?" +
                "token=" + token +
                "&pretty=1";


            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            string encoded = null;
            using (StreamReader sr = new StreamReader(res.GetResponseStream()))
            {
                encoded = sr.ReadToEnd();
                sr.Close();
            }

            defaultChannelId = JObject.Parse(encoded).SelectToken("channels").Children().First().Value<string>("id");

        }

        public void watchChannel(string channelId = null)
        {

            channelId = channelId == null ? defaultChannelId : channelId;

            JToken lastMessageRead, lastMessageProcessed;

            lastMessageRead = getChannelLastMsg(channelId);
            lastMessageProcessed = lastMessageRead;


            bool ever = true;
            for (; ever; )
            {

                lastMessageRead = getChannelLastMsg(channelId);

                string readTS = lastMessageRead.Value<string>("ts");
                string processedTS = lastMessageProcessed.Value<string>("ts");

                if (!readTS.Equals(processedTS))
                {
                    IEnumerable<JToken> messages = getChannelCommands(channelId, latest: readTS, oldest: processedTS);

                    foreach (JToken msg in messages)
                    {
                        cmdProc.processCommand(msg.Value<string>("text"));
                    }

                    if (isUserCommand(lastMessageRead))
                    {
                        cmdProc.processCommand(lastMessageRead.Value<string>("text"));
                    }


                    lastMessageProcessed = lastMessageRead;
                }

                Thread.Sleep(100);

            }
        }

        /*
        public void processCmd(JToken cmd)
        {
            string text = cmd.Value<string>("text").ToLower();

            string[] args = text.Split(' ');
            int numArgs = args.Count();

            if (args[0].Equals("help"))
            {
                if (numArgs == 1)
                {
                    //Print available commands
                }
                else
                {
                    string helpWithCmd = args[1];
                    //Print help with this command
                }
            }

            if (text.Contains("drive"))
            {
                Console.WriteLine("driving");
            }

            //cmd.Value<string>("ts") + ": " + 
            //Console.WriteLine(text);
        }
        */
        public JToken getChannelLastMsg(string channelId=null)
        {
            channelId = channelId == null ? defaultChannelId : channelId;

            JToken message = getChannelHistory(channelId, count: "1").First();

            return message;
        }


        public IEnumerable<JToken> getChannelCommands(string channelId=null, string latest = null, string oldest = null, string count = null)
        {
            channelId = channelId == null ? defaultChannelId : channelId;

            IEnumerable<JToken> messages = getChannelHistory(channelId, latest, oldest, count);

            IEnumerable<JToken> commands = (from msg in messages
                                            where isUserCommand(msg)
                                            select msg);

            return commands;

        }

        public static bool isUserCommand(JToken msg)
        {
            return msg.Value<string>("user") != null && msg.Value<string>("subtype") == null;
        }

        public IEnumerable<JToken> getChannelHistory(string channelId=null, string latest=null, string oldest=null, string count=null)
        {
            channelId = channelId == null ? defaultChannelId : channelId;

            
            string url = "https://slack.com/api/channels.history?" +
               "token=" + token +
               "&channel=" + channelId +
               (latest == null ? "" : "&latest=" + latest) +
               (oldest == null ? "" : "&oldest=" + oldest) +
               (count  == null ? "" : "&count=" + count) +
               "&pretty=1";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                return null;
            }

            string encoded = null;
            using (StreamReader sr = new StreamReader(res.GetResponseStream())) {
                encoded = sr.ReadToEnd();
                sr.Close();
            }

            JObject decoded = JObject.Parse(encoded);

            JToken messages = decoded.Value<JToken>("messages");

            return messages;
        }

        public string getChannelId(string name)
        {
            string url = "https://slack.com/api/channels.list?" +
                "token=" + token +
                "&pretty=1";


            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                return null;
            }


            string encoded = null;
            using (StreamReader sr = new StreamReader(res.GetResponseStream()))
            {
                encoded = sr.ReadToEnd();
                sr.Close();
            }

            JObject decoded = JObject.Parse(encoded);


            JEnumerable<JToken> channels = decoded.SelectToken("channels").Children();
            JToken channelInfo = (from channel in channels 
                                  where channel.Value<string>("name").Equals(name) 
                                  select channel).FirstOrDefault();

            return channelInfo.Value<string>("id");

        }

        public void postMessage(string text) 
        {
            postMessage(text, null);
        }

        public void postMessage(string text, string channel = null, string username = null, string iconUrl = null)
        {
            channel = channel == null ? defaultChannelId : channel;

            username = (username != null ? username : this.username);
            iconUrl = (iconUrl != null ? iconUrl : this.iconUrl);

            string url = "https://slack.com/api/chat.postMessage?" + 
                "token=" + token + 
                "&channel=" + channel + 
                "&text=" + text + 
                "&username=" + username + 
                "&icon_url=" + iconUrl + 
                "&pretty=1";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch { }


        }

        public string getUserList()
        {
            string url = "https://slack.com/api/users.list?token=" + token + "&pretty=1";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                return null;
            }


            StreamReader rs = new StreamReader(res.GetResponseStream());

            return rs.ReadToEnd();
        }

        /*
         * Loads a token from a seperate file at the root of the project
         */
        public static string getToken()
        {
            //Load the token from a seperate file
            //(Because I don't want it publicly available on github)
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string token = new StreamReader(ProjectPath + "\\token.private").ReadToEnd();

            return token;
        }

    }
}
