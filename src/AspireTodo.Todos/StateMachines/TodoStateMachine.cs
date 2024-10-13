using AspireTodo.Core.Shared;
using AspireTodo.Todos.Events;
using AspireTodo.Todos.Shared;
using AspireTodo.Todos.StateMachines.States;
using AspireTodo.UserManagement.Events;
using MassTransit;

namespace AspireTodo.Todos.StateMachines;

public class TodoStateMachine: MassTransitStateMachine<TodoState>
{
    public TodoStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => TodoCreating,
            e => e.CorrelateBy<UserId>(x => x.UserId, y => y.Message.UserId)
                .SelectId(_ => Guid.NewGuid())
        );

        Event(() => TodoCreated,
            e => e.CorrelateBy<TodoId>(x => x.Todo.Id, y => y.Message.Todo.Id)
                .SelectId(_ => Guid.NewGuid())
        );

        Event(() => UserUpdatedTodosCount,
            e => e.CorrelateBy<TodoId>(x => x.Todo.Id, y => y.Message.Todo.Id)
                .SelectId(_ => Guid.NewGuid())
        );

        Event(() => FailedUserUpdateTodosCount,
            e => e.CorrelateBy<TodoId>(x => x.Todo.Id, y => y.Message.Todo.Id)
                .SelectId(_ => Guid.NewGuid())
        );

        Initially(
            When(TodoCreating)
                .Then(context =>
                {
                    context.Saga.Todo = new TodoDto
                    {
                        Id = TodoId.FromInt32(0),
                        IsCompleted = false,
                        Title = context.Message.Title,
                        Summery = context.Message.Summery,
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    context.Saga.Message = context.Message.AuthToken;
                    context.Saga.UserId = context.Message.UserId;
                })
                .TransitionTo(Creating)
                .Publish(context => new TodoCreating(context.Message.Title, context.Message.Summery, context.Message.UserId, context.Message.AuthToken))
        );

        During(Creating,
            When(TodoCreated)
                .Then(context =>
                {
                    context.Saga.Todo = context.Message.Todo;
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.UserTodosCount = context.Message.UserTodosCount;
                })
                .TransitionTo(Created)
                .Publish(context => new TodoCreated(context.Message.Todo, context.Message.UserId, context.Message.UserTodosCount))
        );

        During(Created,
            When(UserUpdatedTodosCount)
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.Todo = context.Message.Todo;
                    context.Saga.UserTodosCount = context.Message.TodosCount;
                })
                .TransitionTo(UpdatedTodosCount)
                .Publish(context => new UserUpdatedTodosCount(context.Message.UserId, context.Message.Todo, context.Message.TodosCount))
                .Finalize()
        );

        During(Created,
            When(FailedUserUpdateTodosCount)
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.Todo = context.Message.Todo;
                    context.Saga.Message = context.Message.Message;
                })
                .TransitionTo(FailedToUpdateUserTodosCount)
                .Publish(context => new FailedUserUpdateTodosCount(context.Message.UserId, context.Message.Todo, context.Message.Message))
                .Finalize()
            );

        OnUnhandledEvent(x => x.Ignore());

        // SetCompletedWhenFinalized();
    }

    public State Creating { get; set; }
    public State Created { get; set; }
    public State UpdatedTodosCount { get; set; }
    public State FailedToUpdateUserTodosCount { get; set; }
    public Event<TodoCreating> TodoCreating { get; private set; }
    public Event<TodoCreated> TodoCreated { get; private set; }
    public Event<UserUpdatedTodosCount> UserUpdatedTodosCount { get; private set; }
    public Event<FailedUserUpdateTodosCount> FailedUserUpdateTodosCount { get; private set; }
}