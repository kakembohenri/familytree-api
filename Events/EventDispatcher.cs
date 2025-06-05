namespace familytree_api.Events
{
    public class EventDispatcher(
        IServiceProvider _serviceProvider
        ): IEventDispatcher
    {
        public async Task DispatchAsync<TEvent>(TEvent @event)
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
