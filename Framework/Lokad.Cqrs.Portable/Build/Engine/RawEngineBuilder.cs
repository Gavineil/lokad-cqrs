using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lokad.Cqrs.Core.Dispatch;
using Lokad.Cqrs.Core.Inbox;

namespace Lokad.Cqrs.Build.Engine
{
    public sealed class RawEngineBuilder
    {

        public readonly List<IEngineProcess> Processes = new List<IEngineProcess>();


        public void AddProcess(IEngineProcess process)
        {
            Processes.Add(process);
        }

        public void AddProcess(Func<CancellationToken, Task> factoryToStartTask)
        {
            Processes.Add(new TaskProcess(factoryToStartTask));
        }

        public void Dispatch(IPartitionInbox inbox, Action<byte[]> lambda)
        {
            Processes.Add(new DispatcherProcess(lambda, inbox));
        }

        public CqrsEngineHost Build()
        {
            var host = new CqrsEngineHost(Processes.AsReadOnly());
            host.Initialize();
            return host;
        }
    }
}