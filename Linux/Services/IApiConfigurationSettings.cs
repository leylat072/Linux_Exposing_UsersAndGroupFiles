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
    }
    public interface IApiConfigurationSettings
    {
         string Passwd { get; set; }
         string GroupFiles { get; set; }
    }
}
