using System;
using System.Activities;
using System.Drawing;
using System.Threading;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Evaluate
{
    [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.EvaluateToImageActivityDescription))]
    public class EvaluateToImageActivity : NativeActivity<Image>
    {
        [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityExpressionDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateToImageActivityExpressionDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        [OverloadGroup("ExpressionString")]
        public InArgument<string> Expression { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityExprDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateToImageActivityExprDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        [OverloadGroup("ExpressionExpr")]
        public InArgument<Expr> Expr { get; set; }

        [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityWidthDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateToImageActivityWidthDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<int> Width { get; set; }

        [LocalizedDisplayName(nameof(Resources.EvaluateToImageActivityHeightDisplayName))]
        [LocalizedDescription(nameof(Resources.EvaluateToImageActivityHeightDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<int> Height { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (Expr == null && Expression == null) metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Expression)));
        }
        
        protected override void Execute(NativeActivityContext context)
        {
            string expression = null;
            Expr expr = null;
            if (Expression.Get(context) is null)
            {
                expr = Expr.Get(context);
            }
            else
            {
                expression = Expression.Get(context);
            }
            
            var w = Width.Get(context);
            var h = Height.Get(context);
            var client = (Application) context.Properties.Find(@"Application");
            while (client == null || !client.Ready)
            {
                Console.WriteLine(Resources.WolframLanguageScope_Execute_Waiting_for_client_to_be_ready___);
                Thread.Sleep(100);
            }

            Result.Set(context,
                expr is null ? client.EvaluateToImage(expression, w, h) : client.EvaluateToImage(expr, w, h));
        }
    }
}
