using MassTransit;

namespace AspireTodo.Core.MassTransit;

public interface IMessage: SagaStateMachineInstance
{
    public string CurrentState { get; set; }
}