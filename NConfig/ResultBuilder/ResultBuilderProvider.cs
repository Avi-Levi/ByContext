﻿using System;
using System.Collections.Generic;

namespace NConfig.ResultBuilder
{
    public class ResultBuilderProvider
    {
        public ResultBuilderProvider()
        {
            this.ResultBuildersRegistry = new Dictionary<Type, Type>() 
            {
                {typeof(IList<>),typeof(ListResultBuilder<>)},
                {typeof(IEnumerable<>),typeof(EnumerableResultBuilder<>)},
                {typeof(ICollection<>),typeof(CollectionResultBuilder<>)},
                {typeof(IDictionary<,>),typeof(DictionaryResultBuilder<,>)},
            };
        }

        private IDictionary<Type, Type> ResultBuildersRegistry { get; set; }

        public IResultBuilder Get(Type resultType)
        {
            Type builderType = null;
            if (resultType.IsGenericType)
            {
                Type openGenericBuilderType = this.ResultBuildersRegistry[resultType.GetGenericTypeDefinition()];
                builderType = openGenericBuilderType.MakeGenericType(resultType.GetGenericArguments());
            }
            else
            {
                builderType = typeof(SingleValueResultBuilder<>).MakeGenericType(resultType);
            }

            IResultBuilder instance = (IResultBuilder)Activator.CreateInstance(builderType);

            return instance;
        }
    }
}
