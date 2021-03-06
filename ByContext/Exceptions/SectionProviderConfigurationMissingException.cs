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

namespace ByContext.Exceptions
{
    public class SectionProviderConfigurationMissingException : ByContextException
    {
        public Type SectionType { get; private set; }

        public SectionProviderConfigurationMissingException(Type sectionType)
            : base(string.Format("Provider for section: {0} is missing, make sure configuration data exists for the given section", sectionType.FullName))
        {
            SectionType = sectionType;
        }
    }
}