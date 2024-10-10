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

    public (IReadOnlyList<RouteConfig> Routes, IReadOnlyList<ClusterConfig> Clusters) Build()
    {
        return (_routes, _cluster);
    }
}