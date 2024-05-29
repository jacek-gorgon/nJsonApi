using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util_JsonApiSerializer.NetCore
{
    public interface IJsonApiSerializerSettings
    {
        public string RoutePrefix { get; set; }
    }
}
