using AspireTodo.Core.Identity;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AspireTodo.Todos.Grpc.Interceptors;

public class AuthInterceptor: Interceptor
{
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        if (context.GetHttpContext().User.GetTypedUserId() == null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthenticated"));
        }

        return base.UnaryServerHandler(request, context, continuation);
    }
}