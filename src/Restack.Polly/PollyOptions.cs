using Polly;

namespace Restack.Polly
{
    public class PollyOptions
    {
        public PolicyCollection Policies { get; } = new PolicyCollection();
    }
}