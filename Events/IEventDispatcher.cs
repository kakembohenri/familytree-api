namespace familytree_api.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent @event);
    }
}
