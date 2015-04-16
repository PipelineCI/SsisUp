using System;
using System.Linq;
using NUnit.Framework;
using SsisUp.Builders;
using SsisUp.Builders.Parsers;
using SsisUp.Builders.References;
using SsisUp.ScriptProviders;

namespace SsisUp.Tests.Builders
{
    [TestFixture]
    public class SqlScriptProviderTests
    {
        [Test]
        public void Ensure_a_list_of_5_scripts_are_returned_given_a_job_configuration_is_enabled_and_with_1_step_and_1_schedule()
        {
            var jobConfiguration = CreateJobConfiguration();

            var builder = new SqlScriptProvider(new JobConfigurationParser());
            var scripts = builder.Build(jobConfiguration);

            Assert.That(scripts != null);
            Assert.That(scripts.Count(), Is.EqualTo(5));
        }

        [Test]
        public void Ensure_the_list_of_scripts_for_a_job_configuration_are_in_the_correct_order()
        {
            var jobConfiguration = CreateJobConfiguration();

            var builder = new SqlScriptProvider(new JobConfigurationParser());
            var scripts = builder.Build(jobConfiguration).ToList();

            Assert.That(scripts[0].Name, Is.EqualTo("1 - Drop Job"));
            Assert.That(scripts[1].Name, Is.EqualTo("2 - Create Job"));
            Assert.That(scripts[2].Name, Is.EqualTo("3 - Create Schedule"));
            Assert.That(scripts[3].Name, Is.EqualTo("4 - Create Step"));
            Assert.That(scripts[4].Name, Is.EqualTo("5 - Enable Job"));
        }

        private static JobConfiguration CreateJobConfiguration()
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
            return jobConfiguration;
        }
    }
}