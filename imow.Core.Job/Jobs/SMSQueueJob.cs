using imow.Services;
using Imow.Framework.Engine;
using Imow.Framework.SMS;
using Quartz;

namespace imow.Core.Job.Jobs
{
    [DisallowConcurrentExecution]
    public class SmsQueueJob : IImowjob
    {
        public void Execute(IJobExecutionContext context)
        {
            var messageService = ImowEngineContext.Current.Resolve<MessageService>();
            var messages = messageService.GetNeedSendMessage();
            foreach (var smsQueueEntity in messages)
            {
             //   messageService.SendMsgToMobile(smsQueueEntity);
            }
        }
    }
}