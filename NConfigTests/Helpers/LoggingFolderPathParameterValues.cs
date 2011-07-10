﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NConfig.Model;
using System.Reflection;

namespace NConfig.Tests.Helpers
{
    public static class LoggingFolderPathParameterValues
    {
        /// <summary>
        /// inside client installation folder.
        /// at development time, with self hosting.
        /// </summary>
        public static ParameterValue ReletiveOneLevelDownPath = new ParameterValue(@".\logs");
        /// <summary>
        /// must be absolute, because installation path is not known.
        /// </summary>
        public static ParameterValue ApplicationServerPath = new ParameterValue(@"c:\logs\");

        public static ParameterValue ProdOnlineClient = new ParameterValue(@"c:\UserData\ApplicationName\logs\");

        /// <summary>
        /// integration server logs to a different path?
        /// </summary>
        public static ParameterValue IntegrationServerPath = new ParameterValue(@"\someNetworDrive\integrationLogs\");

        /// <summary>
        /// specific service logs to a specific path?
        /// </summary>
        public static ParameterValue AuditServicePath = new ParameterValue(@"\\someNetworkDrive\enterpriseAuditData\");
    }
}
