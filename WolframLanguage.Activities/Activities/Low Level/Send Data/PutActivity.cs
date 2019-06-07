using System;
using System.Activities;
using System.Threading;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Send_Data
{
    [LocalizedDisplayName(nameof(Resources.PutActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.PutActivityDescription))]
    public class PutActivity : NativeActivity
    {
        [LocalizedDisplayName(nameof(Resources.PutActivityInDisplayName))]
        [LocalizedDescription(nameof(Resources.PutActivityInDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<dynamic> InArg { get; set; }
        
        #region Overrides of NativeActivity

        protected override void Execute(NativeActivityContext context)
        {
            if (InArg is null) throw new ArgumentException();
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            client.Put(InArg.Get(context));
        }

        #endregion
    }
}
