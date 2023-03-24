namespace Ordering.UnitTests.API.Mocks
{
    public class LoggerMockFactory
    {
        public static Mock<ILogger<T>> CreateLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }
    }
}