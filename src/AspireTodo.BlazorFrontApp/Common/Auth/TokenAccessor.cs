using Blazored.LocalStorage;

namespace AspireTodo.BlazorFrontApp.Common.Auth;

public interface ITokenAccessor
{
    public Task<string?> GetToken();
}

public class TokenAccessor(ILocalStorageService localStorageService): ITokenAccessor
{
    public async Task<string?> GetToken()
    {
        return await localStorageService.GetItemAsync<string>("token");
    }
}