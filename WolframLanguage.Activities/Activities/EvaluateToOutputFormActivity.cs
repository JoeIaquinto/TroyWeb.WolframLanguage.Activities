using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities
{
    [LocalizedDisplayName(nameof(Resources.EvaluateToOutputFormActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.EvaluateToOutputFormActivityDescription))]
    public class EvaluateToOutputFormActivity : NativeActivity<string>
    {
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<string> Expression { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            var expression = Expression.Get(context);
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            Result.Set(context, client.EvaluateToOutputForm(expression));
        }
    }
}
