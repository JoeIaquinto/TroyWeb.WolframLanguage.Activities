using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities
{
    [LocalizedDisplayName(nameof(Resources.EvaluateActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.EvaluateActivityDescription))]
    public class EvaluateActivity<dynamic> : AsyncTaskCodeActivity<dynamic>
    {
        [LocalizedDisplayName(nameof(Resources.EvaluateActivityExpressionDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityExpressionDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<string> Expression { get; set; }

        [LocalizedDisplayName(nameof(Resources.EvaluateActivityResultDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateActivityResultDescription))]
        [LocalizedCategory(nameof(Resources.Output))]
        public OutArgument<dynamic> Result { get; set; }

        /// <inheritdoc />
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Expression == null) metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Expression)));
        }

        protected override Task<dynamic> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken, Application client)
        {
            var expression = Expression.Get(context);
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }
            
            return Task.Run(() => client.Evaluate<dynamic>(expression), cancellationToken);
        }

        protected override void OutputResult(AsyncCodeActivityContext context, dynamic result)
        {
            Result.Set(context, result);
        }
    }
}
