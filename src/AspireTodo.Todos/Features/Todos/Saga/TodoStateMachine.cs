using AspireTodo.Core.MassTransit;
using AspireTodo.Core.Shared;
using AspireTodo.Todos.Events;
using AspireTodo.UserManagement.Events;
using MassTransit;

namespace AspireTodo.Todos.Features.Todos.Saga;

public class TodoState: IMessage
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public TodoId TodoId { get; set; }
    public UserId UserId { get; set; }
    public int UserTodosCount { get; set; }
}

public class TodoStateMachine: MassTransitStateMachine<TodoState>
{
    public TodoStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => TodoCreated,
            e => e.CorrelateBy<TodoId>(x => x.TodoId, y => y.Message.TodoId)
                .SelectId(_ => Guid.NewGuid())
        );

        Event(() => UserUpdatedTodosCount,
            e => e.CorrelateBy<TodoId>(x => x.TodoId, y => y.Message.TodoId)
                .SelectId(_ => Guid.NewGuid())
        );

        Event(() => FailedUserUpdateTodosCount,
            e => e.CorrelateBy<TodoId>(x => x.TodoId, y => y.Message.TodoId)
                .SelectId(_ => Guid.NewGuid())
        );

        Initially(
            When(TodoCreated)
                .Then(context =>
                {
                    context.Saga.TodoId = context.Message.TodoId;
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.UserTodosCount = context.Message.UserTodosCount;
                })
                .TransitionTo(Created)
                .Publish(context => new TodoCreated(context.Message.TodoId, context.Message.UserId, context.Message.UserTodosCount))
        );

        During(Created,
            When(UserUpdatedTodosCount)
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.UserTodosCount = context.Message.TodosCount;
                    context.Saga.TodoId = context.Message.TodoId;
                })
                .TransitionTo(UpdatedTodosCount)
                .Publish(context => new UserUpdatedTodosCount(context.Message.UserId, context.Message.TodoId, context.Message.TodosCount))
                .Finalize()
        );

        During(Created,
            When(FailedUserUpdateTodosCount)
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                })
                .TransitionTo(FailedToUpdateUserTodosCount)
                .Publish(context => new FailedUserUpdateTodosCount(context.Message.UserId, context.Message.TodoId))
                .Finalize()
            );

        OnUnhandledEvent(x => x.Ignore());

        SetCompletedWhenFinalized();
    }

    public State Created { get; set; }
    public State UpdatedTodosCount { get; set; }
    public State FailedToUpdateUserTodosCount { get; set; }

    public Event<TodoCreated> TodoCreated { get; private set; }
    public Event<UserUpdatedTodosCount> UserUpdatedTodosCount { get; private set; }
    public Event<FailedUserUpdateTodosCount> FailedUserUpdateTodosCount { get; private set; }
}