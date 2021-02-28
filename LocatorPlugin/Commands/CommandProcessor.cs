﻿using System.Collections.Generic;
using System.Linq;
using Purps.Valheim.Locator.Patches;

namespace Purps.Valheim.Utils {
    public class CommandProcessor {
        public List<ICommand> Commands { get; } = new List<ICommand>();

        public void addCommand(ICommand command) {
            Commands.Add(command);
        }

        public void printCommands(string[] parameters) {
            Commands.FindAll(c => c.ShouldPrint)
                .ForEach(c => ConsoleUtils.WriteToConsole($"{c.Name} => {c.Description}"));
        }

        public void executeCommand(string commandStr) {
            var command = Commands.FirstOrDefault(e => e.Name.Equals(commandStr.Split(' ').First()));
            command?.Execute(commandStr);
        }
    }
}