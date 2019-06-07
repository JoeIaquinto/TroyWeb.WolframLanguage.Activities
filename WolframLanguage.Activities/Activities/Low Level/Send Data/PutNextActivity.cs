using System;
using System.Activities;
using System.Threading;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Send_Data
{
    [LocalizedDisplayName(nameof(Resources.PutNextActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.PutNextActivityDescription))]
    public class PutNextActivity : NativeActivity
    {
        [LocalizedDisplayName(nameof(Resources.PutNextActivityTypeDisplayName))]
        [LocalizedDescription(nameof(Resources.PutNextActivityTypeDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<ExpressionType> ExpressionTypeIn { get; set; }
        
        #region Overrides of NativeActivity

        protected override void Execute(NativeActivityContext context)
        {
            if (ExpressionTypeIn is null) throw new ArgumentException();
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            client.PutNext(ExpressionTypeIn.Get(context));
        }

        #endregion
    }
}
