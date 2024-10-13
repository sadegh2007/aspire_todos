using AspireTodo.Core.MassTransit;
using AspireTodo.Core.Shared;
using AspireTodo.Todos.Shared;

namespace AspireTodo.Todos.StateMachines.States;

public class TodoState: IMessage
{ 
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public TodoDto Todo { get; set; }
    public UserId UserId { get; set; }
    public int UserTodosCount { get; set; }
    public string? Message { get; set; }
}