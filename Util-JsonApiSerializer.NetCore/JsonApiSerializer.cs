
    using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using Util_JsonApiSerializer.NetCore;
using UtilJsonApiSerializer.Serialization;
using UtilJsonApiSerializer.Serialization.Documents;
namespace UtilJsonApiSerializer
{
    public class JsonApiSerializer : IJsonApiSerializer
    {
        private readonly string _routePrefix;

        public ConfigurationBuilder SerializerConfiguration { get; set; }
// In Net Core we do not have access to the Http Context directly, so we need to inject it via httpContextAccessor

        private IHttpContextAccessor _accessor;
        private IJsonApiSerializerSettings _settings;
        public JsonApiSerializer(
            IHttpContextAccessor accessor, 
            IJsonApiSerializerSettings settings
            )
        {
            SerializerConfiguration = new ConfigurationBuilder();
            _routePrefix = settings == null ? "" : settings.RoutePrefix;
            _accessor = accessor;
        }

        public JsonApiSerializer(IHttpContextAccessor accessor, string routePrefix)
        {
            SerializerConfiguration = new ConfigurationBuilder();
            _routePrefix = string.IsNullOrEmpty(routePrefix) ? "" : routePrefix;
            _accessor = accessor;
        }




        public object SerializeObject(ConfigurationBuilder serializerConfig, object obj)
        {
            var config = serializerConfig.Build();
            RunPreSerializationPipelineModules(config, obj);

            var sut = new JsonApiTransformer() { TransformationHelper = new TransformationHelper(_accessor) };

            CompoundDocument result = sut.Transform(obj, new Context() { Configuration = config, RoutePrefix = _routePrefix });

            return result;
        }

        private void RunPreSerializationPipelineModules(Configuration config, object objectData)
        {
            var objectType = TransformationHelper.GetObjectType(objectData);
            var preSerializerPipelineModule = config.GetPreSerializerPipelineModule(objectType);
            if (preSerializerPipelineModule != null)
            {
                if (objectData is IEnumerable<object> enumerableData)
                {
                    foreach (var item in enumerableData)
                    {
                        preSerializerPipelineModule.Run(item);
                    }
                }
                else
                {
                    preSerializerPipelineModule.Run(objectData);
                }
                
            }
        }


    }
}
