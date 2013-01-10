﻿using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NConfig.Model;
using NUnit.Framework;
using Parameter = NConfig.Model.Parameter;

namespace NConfig.Tests
{
    [TestFixture]
    public class LoadConfigurationTest
    {
        [Test]
        public void TestObjectModelConfiguration()
        {
            IWindsorContainer container = new WindsorContainer()
                .Register(Component.For<IService>().Instance(new ServiceImpl(2)))
                .Register(Component.For<IService>().Instance(new ServiceImpl(1)).Named("1"))
                .Register(Component.For<IService>().Instance(new ServiceImpl(2)).Named("2"));
            

            Section section = Section.Create().FromType<ComplexTestSection>()
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, int>(x => x.Num)
                    .AddValue(ParameterValue.Create("1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, string>(x => x.Name)
                    .AddValue(ParameterValue.Create("name 1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("name 2")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, IList<int>>(x => x.Numbers)
                    .AddValue(ParameterValue.Create("1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient"))
                    .AddValue(ParameterValue.Create("1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, IEnumerable<int>>(x => x.EnumerableNumbers)
                    .AddValue(ParameterValue.Create("1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient"))
                    .AddValue(ParameterValue.Create("1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, TestEnum>(x => x.EnumValue)
                    .AddValue(ParameterValue.Create("Value1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer")))
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, IDictionary<int, string>>(x => x.Dictionary)
                    .AddValue(ParameterValue.Create("1:one")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2:two")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient"))
                    .AddValue(ParameterValue.Create("3:three")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("4:four")
                        .WithTextMatchReference("environment", "production")
                        .WithTextMatchReference("appType", "onlineClient")))
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, IService>(x => x.SVC).WithTranslator("Windsor")
                    .AddValue(ParameterValue.Create(string.Empty)
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer")))
                .AddParameter(Parameter.Create().FromExpression<ComplexTestSection, IEnumerable<IService>>(x => x.SVCs).WithTranslator("Windsor")
                    .AddValue(ParameterValue.Create("1")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
                    .AddValue(ParameterValue.Create("2")
                        .WithTextMatchReference("environment", "development")
                        .WithTextMatchReference("appType", "onlineServer"))
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

            var testSection = svc.GetSection<ComplexTestSection>();
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


            var section = svc.GetSection<ComplexTestSection>();
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
