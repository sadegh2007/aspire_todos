using Microsoft.AspNetCore.Http.HttpResults;
using Yarp.ReverseProxy.Configuration;

namespace AspireTodo.Gateway;

public class ProxyBuilder
{
    private readonly List<RouteConfig> _routes = new();
    private readonly List<ClusterConfig> _cluster = new();

    private ProxyBuilder() { }

    public static ProxyBuilder Create()
    {
        return new ProxyBuilder();
    }

    public ProxyBuilder AddRoute(string route, string targetUrl)
    {
        _routes.Add(new RouteConfig()
        {
            RouteId    = route,
            ClusterId = "cluster/" + route,
            CorsPolicy = "myPolicy",
            Match = new RouteMatch
            {
                Path = "/" + route + "/{**catch-all}",
            },
            Transforms = new[]
            {
                new Dictionary<string, string> { { "PathRemovePrefix", "/" + route } }
            }
        });
        
        _cluster.Add(new ClusterConfig()
        {
            ClusterId = "cluster/" + route,
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "local", new DestinationConfig() { Address = targetUrl } }
            }
        });
        
        return this;
    }
    
    public ProxyBuilder AddSignalRRoute(string route, string targetUrl)
    {
        _routes.Add(new RouteConfig()
        {
            RouteId    = route,
            ClusterId = "cluster/" + route,
            CorsPolicy = "myPolicy",
            Match = new RouteMatch
            {
                Path = "/" + route + "/{**catch-all}",
            },
            Transforms = new[]
            {
                new Dictionary<string, string>
                {
                    { "PathRemovePrefix", "/" + route },
                },
                new Dictionary<string, string>()
                {
                    { "RequestHeadersCopy", "true" },
                },
                new Dictionary<string, string>()
                {
                    { "RequestHeaderOriginalHost", "true" },
                },
                new Dictionary<string, string>()
                {
                    { "RequestHeader", "Upgrade" },
                    { "Set", "WebSocket" },
                },
                new Dictionary<string, string>()
                {
                    { "RequestHeader", "Connection" },
                    { "Set", "Upgrade" },
                },
                new Dictionary<string, string>
                {
                    { "RequestHeaderRemove", "Cookie" },
                },
                new Dictionary<string, string>
                {
                    { "X-Forwarded", "Set" },
                    { "For", "Append" },
                    { "Proto", "Append" },
                    { "Prefix", "Append" },
                    { "HeaderPrefix", "X-Forwarded-" },
                },
            }
        });

        _cluster.Add(new ClusterConfig()
        {
            ClusterId = "cluster/" + route,
            SessionAffinity = new SessionAffinityConfig
            {
                Enabled  = true,
                Policy = "HashCookie",
                FailurePolicy = "Redistribute",
                AffinityKeyName = "ChatHubAffinityKey",
                Cookie = new SessionAffinityCookieConfig
                {
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.Always
                }
            },
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "local", new DestinationConfig() { Address = targetUrl } }
            }
        });

        return this;
    }

    public (IReadOnlyList<RouteConfig> Routes, IReadOnlyList<ClusterConfig> Clusters) Build()
    {
        return (_routes, _cluster);
    }
}