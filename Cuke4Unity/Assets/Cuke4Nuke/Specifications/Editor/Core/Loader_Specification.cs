using System;
using System.Collections.Generic;

using Cuke4Nuke.Core;
using Cuke4Nuke.TestStepDefinitions;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class Loader_Specification
    {
        static readonly Type ValidStepDefinitionClass = typeof(StepDefinition_Specification.ValidStepDefinitions);
        static readonly Type ExternalStepDefinitionClass = typeof(ExternalSteps);

        List<StepDefinition> _stepDefinitions;

        [SetUp]
        public void SetUp()
        {
            _stepDefinitions = new List<StepDefinition>();
        }

        [Test]
        public void Should_load_all_step_definitions_in_project()
        {
            var loader = new Loader(new ObjectFactory());

            _stepDefinitions = loader.Load().StepDefinitions;

            AssertAllMethodsLoaded(ValidStepDefinitionClass);
            AssertAllMethodsLoaded(ExternalStepDefinitionClass);
        }

        void AssertAllMethodsLoaded(Type type)
        {
            var expectedMethods = StepDefinition_Specification.GetStepDefinitionMethods(type);

            foreach (var method in expectedMethods)
            {
                var stepDefinition = new StepDefinition(method);
                Assert.That(_stepDefinitions, Has.Member(stepDefinition), "Missing method: " + stepDefinition.Id);
            }
        }
    }
}
