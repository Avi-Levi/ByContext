﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NConfig;
using Common;
using System.ServiceModel;
using Server.Services;
using Client;
using Server;
using NConfig.Filter;
using NConfig.Filter.Rules;
using NConfig.Testing;
using Server.Configuration;
using Server.WCF;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace SampleTests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]

        [RuntimeContextItem(ConfigConstants.Subjects.Environment.Name, ConfigConstants.Subjects.Environment.Dev)]
        [RuntimeContextItem(ConfigConstants.Subjects.AppType.Name, ConfigConstants.Subjects.AppType.OnlineClient)]
        [RuntimeContextItem(ConfigConstants.Subjects.MachineName.Name, ConfigConstants.Subjects.MachineName.ClientMachine1)]
        [RuntimeContextItem(ConfigConstants.Subjects.ServiceContract.Name, "Common.ILoginService")]

        public void ServiceContractConfig_for_ILoginService()
        {
            var configSvc = Configure.With().AddWindsorTranslatorProvider().
                ContextFromCallingMethod().AddFromXmlFile("Configuration.xml").Build();
            var cfg = configSvc.GetSection<ServiceContractConfig>();
            var binding = configSvc.GetSection<Binding>(cfg.BindingType);

            Assert.AreEqual<string>(cfg.Address.AbsoluteUri, "net.tcp://localhost:20/login");
            Assert.AreEqual<Type>(typeof(NetTcpBinding), cfg.BindingType);
            Assert.IsTrue(typeof(NetTcpBinding).IsInstanceOfType(binding));

            NetTcpBinding tcpBinding = (NetTcpBinding)binding;
            Assert.AreEqual<long>(new NetTcpBinding().MaxReceivedMessageSize, tcpBinding.MaxReceivedMessageSize);
        }

        [TestMethod]
        [RuntimeContextItem(ConfigConstants.Subjects.Environment.Name, ConfigConstants.Subjects.Environment.Dev)]
        [RuntimeContextItem(ConfigConstants.Subjects.AppType.Name, ConfigConstants.Subjects.AppType.OnlineClient)]
        [RuntimeContextItem(ConfigConstants.Subjects.MachineName.Name, ConfigConstants.Subjects.MachineName.ClientMachine1)]
        [RuntimeContextItem(ConfigConstants.Subjects.ServiceContract.Name, "Common.IProductsService")]
        public void ServiceContractConfig_for_IProductsService()
        {
            var configSvc = Configure.With().AddWindsorTranslatorProvider().
                ContextFromCallingMethod().AddFromXmlFile("Configuration.xml").Build();

            var cfg = configSvc.GetSection<ServiceContractConfig>();
            var binding = configSvc.GetSection<Binding>(cfg.BindingType);

            Assert.AreEqual<string>("net.tcp://localhost:21/products", cfg.Address.AbsoluteUri);
            Assert.AreEqual<Type>(typeof(NetTcpBinding), cfg.BindingType);
            Assert.IsTrue(typeof(NetTcpBinding).IsInstanceOfType(binding));

            NetTcpBinding tcpBinding = (NetTcpBinding)binding;
            Assert.AreEqual<long>(3145728, tcpBinding.MaxReceivedMessageSize);
        }

        [TestMethod]
        [RuntimeContextItem(ConfigConstants.Subjects.Environment.Name, ConfigConstants.Subjects.Environment.Dev)]
        [RuntimeContextItem(ConfigConstants.Subjects.AppType.Name, ConfigConstants.Subjects.AppType.OnlineClient)]
        [RuntimeContextItem(ConfigConstants.Subjects.MachineName.Name, ConfigConstants.Subjects.MachineName.ClientMachine1)]
        public void ServicesConfig()
        {
            var cfg = Configure.With().AddWindsorTranslatorProvider().ContextFromCallingMethod().AddFromXmlFile("Configuration.xml").Build()
                .GetSection<ServicesConfig>();

            Assert.IsNotNull(cfg.ServiceTypesToLoad);
            Assert.AreEqual<int>(cfg.ServiceTypesToLoad.Count(), 2);
            Assert.IsTrue(cfg.ServiceTypesToLoad.Contains(typeof(LoginService)));
            Assert.IsTrue(cfg.ServiceTypesToLoad.Contains(typeof(ProductsService)));
        }

        [TestMethod]
        [RuntimeContextItem(ConfigConstants.Subjects.Environment.Name, ConfigConstants.Subjects.Environment.Dev)]
        [RuntimeContextItem(ConfigConstants.Subjects.AppType.Name, ConfigConstants.Subjects.AppType.ApplicationServer)]
        [RuntimeContextItem(ConfigConstants.Subjects.MachineName.Name, ConfigConstants.Subjects.MachineName.ServerMachine)]
        public void SingleServiceConfig()
        {
            IWindsorContainer container = new WindsorContainer()
            .Register(Component.For<IServiceBehavior>().ImplementedBy<DI_InstanceProviderExtension>().Named("DI"));

            container.Register(Component.For<IWindsorContainer>().Instance(container));

            var cfg = Configure.With().AddWindsorTranslatorProvider(container).ContextFromCallingMethod().AddFromXmlFile("Configuration.xml").Build()
                .GetSection<SingleServiceConfig>();

            Assert.IsNotNull(cfg.ServiceBehaviors);
            Assert.AreEqual<int>(1, cfg.ServiceBehaviors.Count());
            Assert.IsTrue(cfg.ServiceBehaviors.Any(x=>typeof(DI_InstanceProviderExtension).IsInstanceOfType(x)));
        }
    }
}
