﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NConfig.Model;
using NConfig.Rules;

namespace NConfig.Policy
{
    public class FilterPolicy : IFilterPolicy
    {
        public FilterPolicy(IEnumerable<IFilterRule> rules)
        {
            this._rules = rules;
            this.Logger = new OutputLogger();
        }
        private readonly IEnumerable<IFilterRule> _rules;

        public ILoggerFacade Logger { get; set; }
        public IEnumerable<ParameterValue> Apply(IEnumerable<ParameterValue> items, IDictionary<string, string> runtimeContext)
        {
            this.Logger.LogFormat("start filtering for context: {0} and items: {1}", runtimeContext.FormatString(), items.FormatString());

            IEnumerable<ParameterValue> filteredItems = items;
            foreach (IFilterRule rule in this._rules)
            {
                this.Logger.LogFormat("start using rule: {0}", rule.GetType().FullName);

                foreach (KeyValuePair<string, string> runtimeContextItem in runtimeContext)
                {
                    this.Logger.LogFormat("start filtering by runtimeContextItem: {0}", runtimeContextItem.FormatString());

                    this.Logger.LogFormat("items before: {0}", filteredItems.FormatString());

                    filteredItems = rule.Apply(filteredItems, runtimeContext, runtimeContextItem);

                    this.Logger.LogFormat("items after: {0}", filteredItems.FormatString());
                }
            }

            return filteredItems;
        }
    }
}
