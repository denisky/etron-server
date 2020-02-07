using System;
using System.Diagnostics;
using System.Reflection;

namespace EtronServer.Core.Helpers
{
    public static class AssemblyHelper
    {
        public static string GetAssemblyVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }   
}
