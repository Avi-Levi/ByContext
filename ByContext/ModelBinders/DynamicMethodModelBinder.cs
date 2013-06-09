﻿// Copyright 2011 Avi Levi

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//  http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;

namespace ByContext.ModelBinders
{
    public class DynamicMethodModelBinder : IModelBinder
    {
        private readonly Dictionary<string, Action<object, object>> _injectors;

        public DynamicMethodModelBinder(Dictionary<string, Action<object, object>> injectors)
        {
            _injectors = injectors;
        }

        public void Bind(object instance, IDictionary<string, object> parametersInfo)
        {
            foreach (var pi in parametersInfo)
            {
                this._injectors[pi.Key](instance, pi.Value);
            }
        }
    }
}