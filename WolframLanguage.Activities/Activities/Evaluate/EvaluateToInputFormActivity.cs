using System;
using System.Activities;
using System.Threading;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Evaluate
{
    [LocalizedDisplayName(nameof(Resources.EvaluateToInputFormActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.EvaluateToInputFormActivityDescription))]
    public class EvaluateToInputFormActivity : NativeActivity<string>
    {
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityExpressionStringDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityExpressionStringDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [OverloadGroup("ExpressionString")]
        [RequiredArgument]
        public InArgument<string> Expression { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityExpressionDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityExpressionDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [OverloadGroup("ExpressionExpr")]
        [RequiredArgument]
        public InArgument<Expr> Expr { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            var expression = Expression.Get(context) ?? (dynamic) Expr.Get(context);
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            Result.Set(context, client.EvaluateToInputForm(expression));
        }
    }
}
