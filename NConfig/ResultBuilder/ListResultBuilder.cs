﻿using System.Collections.Generic;
using System.Linq;

namespace NConfig.ResultBuilder
{
    public class ListResultBuilder<TItem> : BaseCollectionResultBuilder<IList<TItem>, TItem>
    {
        protected override IList<TItem> Convert(IEnumerable<TItem> input)
        {
            return input.ToList();
        }
    }
}