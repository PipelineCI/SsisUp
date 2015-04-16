using System;
using System.Configuration;
using SsisUp.Builders;
using SsisUp.Builders.References;

namespace SsisUp.Sample.Ssis.Deploy
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MasterDb"];
            var fileDestination = ConfigurationManager.AppSettings["FileDestination"];

            var jobConfiguration = JobConfiguration.Create()
                .WithName("Ssis Up - Sample Data Load")
                .WithDescription("This is a sample data load")
                .WithSsisOwner(Environment.UserName)
                .WithSsisServer(Environment.MachineName)
                .Enabled()
                .WithSchedule(
                    ScheduleConfiguration.Create()
                        .WithName("Saturday Load")
                        .RunOn(FrequencyDay.Saturday)
                        .StartingAt(new TimeSpan(10, 0, 0))
                        .ActivatedFrom(DateTime.Parse("1 Jan 2010"))
                        .ActivatedUntil(DateTime.Parse("31 Dec 2020")))
                .WithStep(
                    StepConfiguration.Create()
                        .WithId(1)
                        .WithName("Load Movie Data")
                        .WithSubSystem(SsisSubSystem.IntegrationServices)
                        .WithDtsxFile(@".\Packages\SampleJob.dtsx")
                        .WithDtsConfigurationFile(@".\Packages\SampleJob.dtsConfig")
                        .WithDtsxFileDestination(fileDestination)
                        .ExecuteCommand(
                            string.Format(
                                @"/FILE ""{0}\SampleJob.dtsx"" /CONFIGFILE ""{0}\SampleJob.dtsConfig"" /CHECKPOINTING OFF /REPORTING E /X86",
                                fileDestination))
                        .OnSuccess(JobAction.QuitWithSuccess)
                        .OnFailure(JobAction.QuitWithFailure));


            var result = DeploymentConfiguration
                .Create()
                .ToDatabase(connectionString.ConnectionString)
                .WithJobConfiguration(jobConfiguration)
                .Deploy();


            if (!result.Successful)
            {
                return -1;
            }

            return 0;
        }
    }
}
