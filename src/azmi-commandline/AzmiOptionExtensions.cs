﻿using azmi_main;

using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace azmi_commandline
{
    public static class AzmiOptionExtensions
    {
        public static String[] OptionNames(this AzmiOption option)
        {
            if (option.alias != null)
            {
                return new String[] { $"--{option.name}", $"-{option.alias}" };
            } else
            {
                return new String[] { $"--{option.name}" };
            }
        }

        public static Argument OptionArgument(this AzmiOption option)
        {
            switch (option.type)
            {
                case ArgType.str: return new Argument<string>("string");
                case ArgType.flag: return new Argument<bool>("bool");
                case ArgType.url: return new Argument<string>("url");                
                
                default: throw new Exception($"Unsupported option type: {option.type}");
            }
        }

        public static string OptionDescription(this AzmiOption option)
        {
            return 
                (option.required 
                    ? "Required. " 
                    : "Optional. "
                ) 
                + option.description;
        }

        public static Option ToOption(this AzmiOption option)
        {
            return new Option(option.OptionNames())
            {
                Argument = option.OptionArgument(),
                Description = option.OptionDescription(),
                Required = option.required
            };
        }

        public static Command ToCommand<T,TOptions> () where T: IAzmiCommand, new()
        {

            T cmd = new T();
            var commandLineSubCommand = new Command(cmd.Definition().name, cmd.Definition().description);
             
            foreach (var op in cmd.Definition().arguments)
            {
                commandLineSubCommand.AddOption(op.ToOption());
                // TODO: Implement sorting: 1st required, then strings, then alphabet
            }
            commandLineSubCommand.Handler = CommandHandler.Create<TOptions>(
                op => Console.WriteLine(cmd.Execute(op)));

            return commandLineSubCommand;
        }

    }
}
