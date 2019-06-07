using System;
using System.Activities;
using System.Threading;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Send_Data
{
    [LocalizedDisplayName(nameof(Resources.PutArgCountActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.PutArgCountActivityDescription))]
    public class PutArgCountActivity : NativeActivity
    {
        [LocalizedDisplayName(nameof(Resources.PutArgCountActivityNumDisplayName))]
        [LocalizedDescription(nameof(Resources.PutArgCountActivityNumDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<int> ArgCount { get; set; }
        
        #region Overrides of NativeActivity

        protected override void Execute(NativeActivityContext context)
        {
            if (ArgCount is null) throw new ArgumentException();
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            client.PutArgCount(ArgCount.Get(context));
        }

        #endregion
    }
}
