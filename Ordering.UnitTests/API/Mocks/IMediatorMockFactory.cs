namespace Ordering.UnitTests.API.Mocks
{
    public class IMediatorMockFactory
    {
        public static Mock<IMediator> CreateForCommand<T, R>(IRequestHandler<T, R> handler)
            where T: class, IRequest<R>
        {
            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(It.IsAny<T>(), default)).Returns(async (IRequest<R> request, CancellationToken token) => await handler.Handle((request as T)!, token));

            return mediator;
        }

        public static Mock<IMediator> CreateForIdentifiedCommand<T, R>(IdentifiedCommandHandler<T, R> handler)
            where T: class, IRequest<R>
        {
            var mediator = new Mock<IMediator>();

            mediator.Setup(m => m.Send(It.IsAny<IdentifiedCommand<T, R>>(), default)).Returns(async (IRequest<R> request, CancellationToken token) => await handler.Handle((request as IdentifiedCommand<T, R>)!, token));

            return mediator;
        }
    }
}