using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Evaluate
{
    [LocalizedDisplayName(nameof(Resources.EvaluateActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.EvaluateActivityDescription))]
    public class EvaluateExprActivity : AsyncTaskCodeActivity<Expr>
    {
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityExpressionDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityExpressionDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [OverloadGroup("ExpressionExpr")]
        [RequiredArgument]
        public InArgument<Expr> Expr { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityExpressionStringDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityExpressionStringDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [OverloadGroup("ExpressionStr")]
        [RequiredArgument]
        public InArgument<string> Expression { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityTimeoutDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityTimeoutDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [DefaultValue(300)]
        public InArgument<int> Timeout { get; set; }

        [LocalizedDisplayName(nameof(Resources.EvaluateActivityResultDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityResultDescription))]
        [LocalizedCategory(nameof(Resources.Output))]
        public OutArgument<Expr> Result { get; set; }

        /// <inheritdoc />
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Expr == null && Expression == null) metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Expression)));
        }

        protected override Task<Expr> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken, Application client)
        {
            string expression = null;
            Expr expr = null;
            if (Expr.Get(context) is null) expression = Expression.Get(context);
            if (Expression.Get(context) is null) expr = Expr.Get(context);
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }

            if (expr is null) //Using string expression
            {
                if (!(Timeout is null) && Timeout.Get(context) > 0) //User indicated we should timeout if taking longer than Timeout secs
                {
                    expression = Application.ApplyTimeConstraint(expression, Timeout.Get(context));
                }

                return Task.Run(() => client.Evaluate(expression), cancellationToken);
            }

            if (Timeout is null || Timeout.Get(context) <= 0)
                return Task.Run(() => client.Evaluate(expr), cancellationToken);
            expr = Application.ApplyTimeConstraint(expr, Timeout.Get(context));

            return Task.Run(() => client.Evaluate(expr), cancellationToken);
        }

        protected override void OutputResult(AsyncCodeActivityContext context, Expr result)
        {
            Result.Set(context, result);
        }
    }
}
