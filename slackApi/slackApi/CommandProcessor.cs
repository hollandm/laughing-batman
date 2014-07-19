using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slackApi
{
    class CommandProcessor
    {
        private Action<string> printMethod;
        Hashtable commands;

        public CommandProcessor(Action<string> printMethod)
        {
            this.printMethod = printMethod;

            commands = new Hashtable();

            ChatCommand helpCmd = new ChatCommand("help",
                new Action<string[]>(help),
                "Shows this message. Optional help <cmd> to get help for that command",
                "");
            addCommand(helpCmd);


        }

        public void addCommand(ChatCommand newCmd)
        {
            commands.Add(newCmd.name, newCmd);
        }

        public void processCommand(string cmd)
        {
            Console.WriteLine(cmd);

            string[] args = cmd.Split(' ');

            if (args.Count() == 0) return;
            if (args[0] == null) return;

            ChatCommand cc = (ChatCommand)commands[args[0]];
            if (cc == null) return;

            cc.doThis(args);

        }

        public void help(string[] args)
        {

            if (args.Count() == 1)
            {
                foreach (ChatCommand cmd in commands.Values)
                {
                    string text = cmd.name + ": " + cmd.helpShort;
                    printMethod(text);
                }
            }
            else
            {
                string text = ((ChatCommand)commands[args[1]]).helpLong;
                printMethod(text);
            }
            

        }

    }

    class ChatCommand 
    {
        public string name;
        
        public Action<string[]> doThis;

        public string helpShort;

        public string helpLong;


        public ChatCommand(string name, Action<string[]> doThis, string helpShort, string helpLong)
        {
            this.name = name;
            this.doThis = doThis;
            this.helpShort = helpShort;
            this.helpLong = helpLong;
        }
    }
}
