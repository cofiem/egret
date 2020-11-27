using Egret.Cli.Commands;
using Egret.Cli.Extensions;
using Egret.Cli.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Egret.Cli.Formatters
{
    public class MetaFormatter : IResultFormatter
    {
        private readonly IReadOnlyCollection<IResultFormatter> formatters;

        public MetaFormatter(ILogger<TestCommand> logger, TestCommandOptions options, IServiceProvider provider)
        {
            var formatters = new List<IResultFormatter>(3);
            if (logger.PassThrough(options.Console, "Using console result formatter: {yesNo}", LogLevel.Debug))
            {
                formatters.Add(provider.GetRequiredService<ConsoleResultFormatter>());
            }
            if (logger.PassThrough(options.Json, "Using json result formatter: {yesNo}", LogLevel.Debug))
            {
                var json = provider.GetRequiredService<JsonResultFormatter>();
                formatters.Add(json);
                logger.LogDebug("Writing json results to: {path}", json.Output);
            }
            if (logger.PassThrough(options.Html, "Using HTML result formatter: {yesNo}", LogLevel.Debug))
            {
                formatters.Add(provider.GetRequiredService<HtmlResultFormatter>());
            }

            this.formatters = formatters;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var formatter in formatters)
            {
                await formatter.DisposeAsync();
            }
        }

        public async ValueTask WriteResult(int index, TestCaseResult result)
        {
            foreach (var formatter in formatters)
            {
                await formatter.WriteResult(index, result);
            }
        }

        public async ValueTask WriteResultsFooter(int count, int successes, int failures)
        {
            foreach (var formatter in formatters)
            {
                await formatter.WriteResultsFooter(count, successes, failures);
            }
        }

        public async ValueTask WriteResultsHeader()
        {
            foreach (var formatter in formatters)
            {
                await formatter.WriteResultsHeader();
            }
        }
    }
}