namespace Ordering.UnitTests.API.Mocks
{
    public class IRequestManagerMockFactory
    {
        public static Mock<IRequestManager> Create<T>()
        {
            var handler = new Mock<IRequestManager>();

            handler.Setup(h => h.ExistAsync(It.IsAny<Guid>())).Returns((Guid id) => Task.FromResult(false));
            handler.Setup(h => h.CreateRequestForCommandAsync<T>(It.IsAny<Guid>())).Returns((Guid id) => Task.CompletedTask);

            return handler;
        }
    }
}