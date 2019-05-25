using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linux.Services
{
    public class ApiConfigurationSettings : IApiConfigurationSettings
    {
        public string Passwd { get; set; }
        public string GroupFiles { get; set; }
        public string ContentRoot { get; set; }
        public string PasswdPath { get; set; }
        public string GroupFilesPath { get; set; }
    }
    public interface IApiConfigurationSettings
    {
         string Passwd { get; set; }
         string GroupFiles { get; set; }
         string ContentRoot { get; set; }
         string PasswdPath { get; set; }
         string GroupFilesPath { get; set; }
    }
}
