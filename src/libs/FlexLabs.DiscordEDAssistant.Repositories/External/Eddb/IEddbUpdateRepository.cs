﻿using System;
using System.Collections.Generic;
using FlexLabs.DiscordEDAssistant.Models.External.Eddb;

namespace FlexLabs.DiscordEDAssistant.Repositories.External.Eddb
{
    public interface IEddbUpdateRepository : IDisposable
    {
        void ClearAll();
        void UploadAll(IEnumerable<Module> modules);
        void UploadAll(IEnumerable<Models.External.Eddb.System> systems);
        void MergeAll();
        void MergeAllSystems();
    }
}
