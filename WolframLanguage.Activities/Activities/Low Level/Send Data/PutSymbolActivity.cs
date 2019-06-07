using System;
using System.Activities;
using System.Threading;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Send_Data
{
    [LocalizedDisplayName(nameof(Resources.PutSymbolActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.PutSymbolActivityDescription))]
    public partial class PutSymbolActivity : NativeActivity
    {
        [LocalizedDisplayName(nameof(Resources.PutSymbolActivitySymbolDisplayName))]
        [LocalizedDescription(nameof(Resources.PutSymbolActivitySymbolDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<string> Symbol { get; set; }
        
        #region Overrides of NativeActivity

        protected override void Execute(NativeActivityContext context)
        {
            if (Symbol is null) throw new ArgumentException();
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            client.PutSymbol(Symbol.Get(context));
        }

        #endregion
    }
}
