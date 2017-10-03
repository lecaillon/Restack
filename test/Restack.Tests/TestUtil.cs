using Microsoft.Extensions.DependencyInjection;

namespace Restack.Tests
{
    public static class TestUtil
    {
        public static IServiceCollection ServiceCollection => new ServiceCollection().AddLogging()
                                                                                     .AddOptions()
                                                                                     .AddRestack()
                                                                                     .AddConsul()
                                                                                     .AddPolly();
    }
}
