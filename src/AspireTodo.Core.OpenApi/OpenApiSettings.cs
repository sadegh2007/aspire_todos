using System.ComponentModel.DataAnnotations;

namespace AspireTodo.Core.OpenApi;

public class OpenApiSettings
{
    [Required]
    public EndpointSettings Endpoint { get; set; }

    [Required]
    public DocumentSettings[] Documents { get; set; }

    public AuthSettings? Auth { get; set; }

    public class EndpointSettings
    {
        [Required]
        public string Name { get; set; }

        public string? Url { get; set; }
    }

    public class AuthSettings
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string AppName { get; set; }
    }

    public class DocumentSettings
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Version { get; set; }
    }
}