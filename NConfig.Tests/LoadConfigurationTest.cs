﻿using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using NConfig.Model;
using NConfig.Tests.Helpers;
using NUnit.Framework;

namespace NConfig.Tests
{
    [TestFixture]
    public class LoadConfigurationTest
    {
        [Test]
        public void TestObjectModelConfiguration()
        {
            IWindsorContainer container = new WindsorContainer()
                .Register(Castle.MicroKernel.Registration.Component.For<IService>().Instance(new ServiceImpl(2)))
                .Register(Castle.MicroKernel.Registration.Component.For<IService>().Instance(new ServiceImpl(1)).Named("1"))
                .Register(Castle.MicroKernel.Registration.Component.For<IService>().Instance(new ServiceImpl(2)).Named("2"));

            Section section = Section.Create().FromType<TestSection>()
                .AddParameter(Parameter.Create().FromExpression<TestSection, int>(x => x.Num)
                    .AddValue(ParameterValue.Create("1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<TestSection, string>(x => x.Name)
                    .AddValue(ParameterValue.Create("name 1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("name 2")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<TestSection, IList<int>>(x => x.Numbers)
                    .AddValue(ParameterValue.Create("1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient"))
                    .AddValue(ParameterValue.Create("1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<TestSection, IEnumerable<int>>(x => x.EnumerableNumbers)
                    .AddValue(ParameterValue.Create("1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient"))
                    .AddValue(ParameterValue.Create("1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<TestSection, TestEnum>(x => x.EnumValue)
                    .AddValue(ParameterValue.Create("Value1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer")))
                .AddParameter(Parameter.Create().FromExpression<TestSection, IDictionary<int, string>>(x => x.Dictionary)
                    .AddValue(ParameterValue.Create("1:one")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2:two")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient"))
                    .AddValue(ParameterValue.Create("3:three")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("4:four")
                        .WithReference("environment", "production")
                        .WithReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<TestSection, IService>(x => x.SVC).WithTranslator("Windsor")
                    .AddValue(ParameterValue.Create(string.Empty)
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer")))
                .AddParameter(Parameter.Create().FromExpression<TestSection, IEnumerable<IService>>(x => x.SVCs).WithTranslator("Windsor")
                    .AddValue(ParameterValue.Create("1")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithReference("environment", "development")
                        .WithReference("appType", "onlineServer"))
                        );

            IConfigurationService svc = Configure.With(cfg=>
                cfg.RuntimeContext((context) =>
                {
                    context.Add("environment", "development");
                    context.Add("appType", "onlineServer");
                })
                .AddWindsorTranslatorProvider(container)
                .AddSection(section)
            );

            var testSection = svc.GetSection<TestSection>();
            Assert.IsNotNull(testSection);
            Assert.IsNotNull(testSection.Name);
            Assert.IsNotNull(testSection.Num);
            Assert.AreEqual(TestEnum.Value1, testSection.EnumValue);
            Assert.IsNotNull(testSection.Numbers);
            Assert.IsNotNull(testSection.Dictionary);
            Assert.IsNotNull(testSection.EnumerableNumbers);
            Assert.IsTrue(testSection.EnumerableNumbers.Any());
            Assert.IsTrue(testSection.SVCs.Any());
            Assert.AreEqual(2, testSection.SVCs.Count());
            Assert.AreEqual(2, testSection.SVC.Get());
            Assert.AreEqual(1, testSection.SVCs.First().Get());
        }

        [Test]
        public void TestXMLConfiguration_load_from_file()
        {
            IWindsorContainer container = new WindsorContainer()
                .Register(Castle.MicroKernel.Registration.Component.For<IService>().Instance(new ServiceImpl(2)))
                .Register(Castle.MicroKernel.Registration.Component.For<IService>().Instance(new ServiceImpl(1)).Named("1"))
                .Register(Castle.MicroKernel.Registration.Component.For<IService>().Instance(new ServiceImpl(2)).Named("2"));

            IConfigurationService svc =
            Configure.With(cfg=>
                cfg.RuntimeContext((context) =>
                {
                    context.Add("environment", "development");
                    context.Add("appType", "onlineServer");
                })
                .AddWindsorTranslatorProvider(container)
                .AddFromXmlFile("TestConfiguration.xml")
                );


            var section = svc.GetSection<TestSection>();
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.Name);
            Assert.IsNotNull(section.Num);
            Assert.AreEqual(TestEnum.Value1, section.EnumValue);
            Assert.IsNotNull(section.Numbers);
            Assert.IsNotNull(section.Dictionary);
            Assert.IsNotNull(section.EnumerableNumbers);
            Assert.IsNotNull(section.SVC);
            Assert.IsNotNull(section.SVCs);
            Assert.IsTrue(section.SVCs.Any());
            Assert.AreEqual(2, section.SVCs.Count());
            Assert.IsTrue(section.EnumerableNumbers.Any());
            Assert.AreEqual(2, section.SVC.Get());
            Assert.AreEqual(1, section.SVCs.First().Get());
        }
    }
}
