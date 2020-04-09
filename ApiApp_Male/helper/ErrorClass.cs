using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.helper
{
    public class ErrorClass : IErrorClass
    {
        private readonly IConfiguration config;

        public ErrorClass(IConfiguration Config)
        {
            config = Config;
        }
        public String ErrorCode { get; set; }
        public String ErrorProp { get; set; }
        public String ErrorMessage { get; set; }

        public void LoadError(String ErrorCode)
        {
            this.ErrorCode = ErrorCode;
            var eSection = config.GetSection("Errors");
            eSection.Bind(ErrorCode, this);
        }
    }
}
