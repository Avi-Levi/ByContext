﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NConfig.Model;

namespace NConfig
{
    public static class ParameterValueExtensions
    {
        public static ParameterValue AddReference(this ParameterValue source, string subjectName, string subjectValue)
        {
            source.References.Add(new ContextSubjectReference(subjectName, subjectValue));
            return source;
        }
        public static ParameterValue AddAllReferenceToSubject(this ParameterValue source, string subjectName)
        {
            source.References.Add(new ContextSubjectReference(subjectName, ContextSubjectReference.ALL));
            return source;
        }
    }
}
