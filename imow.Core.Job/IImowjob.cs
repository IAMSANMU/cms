using Quartz;

namespace imow.Core.Job
{
    public interface IImowjob:IJob
    {
        ITrigger GeTrigger();
    }
}