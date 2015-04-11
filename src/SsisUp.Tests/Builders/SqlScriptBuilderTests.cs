using System;
using System.Linq;
using NUnit.Framework;
using SsisUp.Builders;
using SsisUp.Configurations;

namespace SsisUp.Tests.Builders
{
    [TestFixture]
    public class SqlScriptBuilderTests
    {
        [Test]
        public void Ensure_a_list_of_5_scripts_are_returned_given_a_job_configuration_is_enabled_and_with_1_step_and_1_schedule()
        {
            var jobConfiguration =
                JobConfiguration.Create()
                    .WithName("Test Name")
                    .WithDescription("Test Description")
                    .WithSsisOwner("Test SSIS Owner")
                    .WithSsisServer("Test SSIS Server")
                    .WithStep(StepConfiguration.Create()
                        .WithName("Test Step Name")
                        .WithId(1)
                        .WithSubSystem(SsisSubSystem.TransactStructuredQueryLanguage)
                        .ExecuteCommand("Test Command")
                        .OnFailure(JobAction.QuitWithFailure)
                        .OnSuccess(JobAction.QuitWithSuccess))
                    .WithSchedule(ScheduleConfiguration.Create()
                        .WithName("Test Schedule Configuration")
                        .RunOn(FrequencyDay.Monday)
                        .ActivatedFrom(DateTime.Now)
                        .ActivatedUntil(DateTime.Now.AddDays(1))
                        .StartingAt(TimeSpan.FromSeconds(6)))
                    .Enabled();

            var builder = new SqlScriptBuilder(new JobConfigurationParser());
            var scripts = builder.Build(jobConfiguration);

            Assert.That(scripts != null);
            Assert.That(scripts.Count(), Is.EqualTo(5));
        }
    }
}