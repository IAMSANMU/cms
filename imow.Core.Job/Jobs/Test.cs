using imow.Services;
using Imow.Framework.Engine;
using Quartz;

namespace imow.Core.Job.Jobs
{
    [DisallowConcurrentExecution]
    public class Test : IImowjob
    {
        public void Execute(IJobExecutionContext context)
        {
        }

        public ITrigger GeTrigger()
        {
            throw new System.NotImplementedException();
        }
    }
}