using System;
using System.Activities;
using System.Threading;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Send_Data
{
    [LocalizedDisplayName(nameof(Resources.PutDataActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.PutDataActivityDescription))]
    public class PutDataActivity : NativeActivity
    {
        [LocalizedDisplayName(nameof(Resources.PutDataActivityDataDisplayName))]
        [LocalizedDescription(nameof(Resources.PutDataActivityDataDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<byte[]> Data { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.PutDataActivitySizeDisplayName))]
        [LocalizedDescription(nameof(Resources.PutDataActivitySizeDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<int> Size { get; set; }
        
        #region Overrides of NativeActivity

        protected override void Execute(NativeActivityContext context)
        {
            if (Data is null || Size is null || Size.Get(context) <= 0) throw new ArgumentException();
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            client.PutSize(Size.Get(context));
            client.PutData(Data.Get(context));
        }

        #endregion
    }
}
