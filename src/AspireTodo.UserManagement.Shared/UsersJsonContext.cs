using System.Text.Json.Serialization;

namespace AspireTodo.UserManagement.Shared;

[JsonSerializable(typeof(UserDto))]
public partial class UsersJsonContext: JsonSerializerContext;