using Gsafety.PTMS.Media.Configuration;

namespace Gsafety.PTMS.Media.Audio
{
    public interface IAudioConfigurator : IAudioConfigurationSource
    {
        void Configure(IAudioFrameHeader frameHeader);
    }
}
