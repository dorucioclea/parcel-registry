namespace ParcelRegistry.Api.Extract.Extracts.Responses
{
    using System;
    using Swashbuckle.AspNetCore.Filters;

    public class ParcelRegistryResponseExample : IExamplesProvider<object>
    {
        public object GetExamples()
            => new { mimeType = "application/zip", fileName = $"{ExtractController.ZipName}-{DateTime.Now:yyyy-MM-dd}.zip" };
    }
}
