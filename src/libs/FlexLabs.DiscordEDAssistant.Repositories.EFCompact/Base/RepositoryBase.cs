using System;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base
{
    public abstract class RepositoryBase : IDisposable
    {
        protected EDAssistantDataContext DataContext { get; private set; }
        public RepositoryBase(EDAssistantDataContext context)
        {
            DataContext = context;
        }

        public void Dispose()
        {
            DataContext?.Dispose();
            DataContext = null;
        }

        public DateTime GetDate() => DateTime.UtcNow;

        protected Int64 ConvertID(UInt64 id)
        {
            if (id <= Int64.MaxValue)
                return Convert.ToInt64(id);

            return -Convert.ToInt64(id - Int64.MaxValue);
        }

        protected UInt64 ConvertID(Int64 id)
        {
            if (id >= 0)
                return Convert.ToUInt64(id);

            return Convert.ToUInt64(-id) + Int64.MaxValue;
        }
    }
}
