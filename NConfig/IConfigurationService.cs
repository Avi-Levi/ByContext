﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NConfig
{
    public interface IConfigurationService
    {
        TSection GetSection<TSection>() where TSection : class;
        IConfigurationService WithReference(string subjectName, string subjectValue);
    }
}
