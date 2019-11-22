using Gsafety.PTMS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class CommandFactory
    {
        public static Dictionary<CommandTypeEnum, CommandBase> commands = null;
        static CommandFactory()
        {
            commands = new Dictionary<CommandTypeEnum, CommandBase>();
            commands.Add(CommandTypeEnum.CommandParam, new CommandParamCommand());
            commands.Add(CommandTypeEnum.ElectricFence, new TrafficeFenceCommand());
            commands.Add(CommandTypeEnum.Route, new TrafficeRouteCommand());
            commands.Add(CommandTypeEnum.MDVRMessage, new MDVRTextCommand());
            
        }

        public static CommandBase GetCommand(CommandTypeEnum type)
        {
            return commands[type];
        }
    }
}
