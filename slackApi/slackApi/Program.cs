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

    class Program
    {
        public static void driveForward(string[] args)
        {
            
        }

        static void Main(string[] args)
        {

            SlackIO slack = new SlackIO();

            slack.cmdProc.addCommand(new ChatCommand(
            
                name: "drive",
                doThis: new Action<string[]>(driveForward),
                helpShort: "",
                helpLong: ""
            ));

            slack.defaultChannelId = slack.getChannelId("test");
            slack.postMessage("System Online");

            slack.watchChannel();
        }



    }
}
