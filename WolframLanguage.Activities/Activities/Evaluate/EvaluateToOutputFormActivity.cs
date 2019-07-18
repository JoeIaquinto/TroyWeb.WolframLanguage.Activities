using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Evaluate
{
    [LocalizedDisplayName(nameof(Resources.EvaluateToOutputFormActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.EvaluateToOutputFormActivityDescription))]
    public class EvaluateToOutputFormActivity : NativeActivity<string>
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
        
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityTimeoutDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityTimeoutDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [DefaultValue(300)]
        public InArgument<int> Timeout { get; set; }

        #region Overrides of NativeActivity<string>

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (Expr == null && Expression == null) metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Expression)));
        }

        #endregion

        protected override void Execute(NativeActivityContext context)
        {
            var expression = Expression.Get(context) ?? (dynamic) Expr.Get(context);
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            expression = Timeout is null || Timeout.Get(context) <= 0 ? expression : Application.ApplyTimeConstraint(expression, Timeout.Get(context));

            Result.Set(context, client.EvaluateToOutputForm(expression));
        }
    }
}
