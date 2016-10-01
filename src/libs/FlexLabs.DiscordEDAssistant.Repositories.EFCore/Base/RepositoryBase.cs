using Microsoft.EntityFrameworkCore;
using System;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base
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

        public void SetLongTimeout() => DataContext.Database.SetCommandTimeout(300000);

        protected long ConvertID(ulong id)
        {
            if (id <= long.MaxValue)
                return Convert.ToInt64(id);

            return -Convert.ToInt64(id - long.MaxValue);
        }

        protected ulong ConvertID(long id)
        {
            if (id >= 0)
                return Convert.ToUInt64(id);

            return Convert.ToUInt64(-id) + long.MaxValue;
        }
    }
}
