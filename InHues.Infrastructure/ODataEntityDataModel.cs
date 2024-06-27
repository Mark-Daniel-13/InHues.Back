using InHues.Domain.DTO.V1.Identity.Response;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace InHues.Infrastructure
{
    public static class ODataEntityDataModel
    {
        public static IEdmModel GetEntityDataModelV1()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }
    }
}
