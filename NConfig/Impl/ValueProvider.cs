﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NConfig.Abstractions;
using NConfig.Model;
using NConfig.Filter;
using NConfig.Filter.Rules;

namespace NConfig.Impl
{
    public class ValueProvider : IValueProvider
    {
        public ValueProvider(Parameter target, Func<IEnumerable<string>, object> parseMethod,
            IFilterPolicy policy)
        {
            this.Target = target;
            this.Parse = parseMethod;
            this.Policy = policy;
        }

        private Parameter Target { get; set; }
        private Func<IEnumerable<string>, object> Parse { get; set; }
        private IFilterPolicy Policy { get; set; }

        public object Get(IDictionary<string, string> runtimeContext)
        {
            var valuesByPolicy = this.Policy.Filter(runtimeContext, this.Target.Values).OfType<ParameterValue>().Select(v => v.Value);

            return this.Parse(valuesByPolicy);
        }
    }
}
