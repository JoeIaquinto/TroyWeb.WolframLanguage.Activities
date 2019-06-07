using System;
using System.Activities;
using System.Threading;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Get_Data
{
    [LocalizedDisplayName(nameof(Resources.GetActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.GetActivityDescription))]
    public class GetActivity<T> : NativeActivity<T>
    {
        #region Overrides of NativeActivity

        protected override void Execute(NativeActivityContext context)
        {
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }

            Result.Set(context, client.GetValue<T>());
        }

        #endregion
    }
}
