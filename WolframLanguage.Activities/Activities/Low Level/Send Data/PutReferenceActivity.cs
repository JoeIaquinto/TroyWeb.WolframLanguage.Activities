using System;
using System.Activities;
using System.Threading;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Send_Data
{
    [LocalizedDisplayName(nameof(Resources.PutReferenceActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.PutReferenceActivityDescription))]
    public class PutReferenceActivity : NativeActivity
    {
        [LocalizedDisplayName(nameof(Resources.PutReferenceActivityInDisplayName))]
        [LocalizedDescription(nameof(Resources.PutReferenceActivityInDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<dynamic> InArg { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.PutReferenceActivityTypeDisplayName))]
        [LocalizedDescription(nameof(Resources.PutReferenceActivityTypeDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        public InArgument<Type> InType { get; set; }
        
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

            if (InType is null)
            {
                client.PutReference(InArg.Get(context));
            }
            else
            {
                client.PutReference(InArg.Get(context), InType.Get(context));
            }
        }

        #endregion
    }
}
