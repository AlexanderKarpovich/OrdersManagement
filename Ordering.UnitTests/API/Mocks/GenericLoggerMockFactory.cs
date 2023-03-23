namespace Ordering.UnitTests.API.Mocks
{
    public class GenericLoggerMockFactory
    {
        public static Mock<ILogger<T>> CreateLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }
    }
}