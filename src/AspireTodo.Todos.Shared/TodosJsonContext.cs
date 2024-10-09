using System.Text.Json.Serialization;

namespace AspireTodo.Todos.Shared;

[JsonSerializable(typeof(TodoDto))]
public partial class TodosJsonContext: JsonSerializerContext;