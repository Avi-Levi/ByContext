﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NConfig.Model;
using NConfig;
using NConfig.Abstractions;

namespace NConfig.Filter.Rules
{
    /// <summary>
    /// selects items that has an "ALL" or a specific reference to the given <paramref name="currentRuntimeContextItem"/>.
    /// </summary>
    public class WithSpecificOrALLRerefenceToSubjectRule : IFilterRule
    {
        public IEnumerable<IHaveFilterReference> Apply
            (IEnumerable<IHaveFilterReference> items, IDictionary<string, string> runtimeContext, KeyValuePair<string, string> currentRuntimeContextItem)
        {
            var query = from item in items
                        where
                        item.References.HasSpecificReference(currentRuntimeContextItem)
                        ||
                        item.References.HasAllReference(currentRuntimeContextItem.Key)
                        select item;
            return query;
        }
    }
}
