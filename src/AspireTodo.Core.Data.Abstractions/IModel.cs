namespace AspireTodo.Core.Data.Abstractions;

public interface IModel<TKey>
{
    public TKey Id { get; set; }
}

public interface IModel : IModel<int>;