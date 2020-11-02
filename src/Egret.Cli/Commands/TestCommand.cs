﻿using Egret.Cli.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egret.Cli.Commands
{


    public class TestCommandOptions
    {
        public FileInfo Configuration { get; set; }
    }


    public class TestCommand : IEgretCommand
    {


        public TestCommandOptions Options { get; }

        public TestCommand(ILogger<TestCommand> logger, ITerminal terminal, TestCommandOptions options)
        {
            Options = options;
            Logger = logger;
            Terminal = terminal;
        }

        public ILogger<TestCommand> Logger { get; }

        public ITerminal Terminal { get; }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            Logger.LogInformation("Test Command execute");
            Terminal.WriteLine("Starting tests".Bold());

            Logger.LogDebug("Config: {config}", Options.Configuration);

            YamlDotNet.

            return await Task.FromResult(0);
        }


    }
}
