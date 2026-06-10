using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerWithJwt(this SwaggerGenOptions options)
    {
        // JWT Bearer authentication is configured in Program.cs
        // This extension can be used for additional Swagger customization if needed
    }
}




