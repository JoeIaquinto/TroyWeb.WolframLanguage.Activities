using System;
using System.Activities;
using System.Threading;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Low_Level.Get_Data
{
    [LocalizedDisplayName(nameof(Resources.GetExpressionActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.GetExpressionActivityDescription))]
    public class GetExpressionActivity : NativeActivity<Expr>
    {
        #region Overrides of NativeActivity<Expr>

        protected override void Execute(NativeActivityContext context)
        {
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            Result.Set(context, client.GetExpr());
        }

        #endregion
    }
}
