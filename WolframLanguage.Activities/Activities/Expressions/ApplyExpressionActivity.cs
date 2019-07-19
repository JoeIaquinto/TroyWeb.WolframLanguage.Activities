using System;
using System.Activities;
using System.CodeDom.Compiler;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Expressions
{
    [LocalizedDisplayName(nameof(Resources.ApplyExpressionActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.ApplyExpressionActivityDescription))]
    public class ApplyExpressionActivity : NativeActivity<Expr>
    {
        [LocalizedDisplayName(nameof(Resources.ApplyExpressionActivityHeadDisplayName))]
        [LocalizedDescription(nameof(Resources.ApplyExpressionActivityHeadDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<object> Head { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.ApplyExpressionActivityArgumentsDisplayName))]
        [LocalizedDescription(nameof(Resources.ApplyExpressionActivityArgumentsDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        public InArgument<object[]> Arguments { get; set; }

        #region Overrides of NativeActivity<Expr>

        protected override void Execute(NativeActivityContext context)
        {
            var head = Head.Get(context);
            if (!(Arguments is null) && Arguments.Get(context).Length > 0)
            {
                Result.Set(context, new Expr(head, Arguments.Get(context)));
                return;
            }
            
            Result.Set(context, new Expr(head));
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (Head is null)
            {
                metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Head)));
            }
            
            base.CacheMetadata(metadata);
        }

        #endregion
    }
}
