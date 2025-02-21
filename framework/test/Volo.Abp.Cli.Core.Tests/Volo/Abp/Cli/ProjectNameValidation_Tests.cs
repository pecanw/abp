﻿using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.ProjectModification;
using Xunit;

namespace Volo.Abp.Cli
{
    public class ProjectNameValidation_Tests : AbpCliTestBase
    {
        private readonly NewCommand _newCommand;

        public ProjectNameValidation_Tests()
        {
            _newCommand = GetRequiredService<NewCommand>();
        }

        [Fact]
        public async Task IllegalProjectName_Test()
        {
            var illegalProjectNames = new[]
            {
                "MyCompanyName.MyProjectName",
                "MyProjectName",
                "CON", //Windows doesn't accept these names as file name
                "AUX",
                "PRN",
                "COM1",
                "LPT2"
            };

            foreach (var illegalProjectName in illegalProjectNames)
            {
                var args = new CommandLineArgs("new", illegalProjectName);
                await _newCommand.ExecuteAsync(args).ShouldThrowAsync<CliUsageException>();
            }
        }

        [Fact]
        public async Task ParentDirectoryContain_Test()
        {

            var args = new CommandLineArgs("new", "Test..Test");
            await _newCommand.ExecuteAsync(args).ShouldThrowAsync<CliUsageException>();
        }
    }
}
