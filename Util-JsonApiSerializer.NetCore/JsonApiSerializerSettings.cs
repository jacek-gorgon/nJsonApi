using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util_JsonApiSerializer.NetCore
{
    public class JsonApiSerializerSettings : IJsonApiSerializerSettings
    {
        public string RoutePrefix { get; set; } = "api";
    }
}
