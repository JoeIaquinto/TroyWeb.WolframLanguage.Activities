using System;
using System.Activities;
using System.Threading;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Send_Data
{
    [LocalizedDisplayName(nameof(Resources.PutFuncActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.PutFuncActivityDescription))]
    public class PutFunctionActivity : NativeActivity
    {
        [LocalizedDisplayName(nameof(Resources.PutFuncActivityFuncDisplayName))]
        [LocalizedDescription(nameof(Resources.PutFuncActivityFuncDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<string> Function { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.PutFuncActivityArgCountDisplayName))]
        [LocalizedDescription(nameof(Resources.PutFuncActivityArgCountDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<int> ArgCount { get; set; }
        
        #region Overrides of NativeActivity

        protected override void Execute(NativeActivityContext context)
        {
            if (Function is null || ArgCount is null) throw new ArgumentException();
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            client.PutFunction(Function.Get(context), ArgCount.Get(context));
        }

        #endregion
    }
}
