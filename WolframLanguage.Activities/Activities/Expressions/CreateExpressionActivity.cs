using System;
using System.Activities;
using Wolfram.NETLink;
using WolframLanguage.Activities.Properties;

namespace WolframLanguage.Activities.Activities.Expressions
{
    [LocalizedDisplayName(nameof(Resources.CreateExpressionActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.CreateExpressionActivityDescription))]
    public class CreateExpressionActivity : NativeActivity<Expr>
    {
        [LocalizedDisplayName(nameof(Resources.CreateExpressionActivityNameDisplayName))]
        [LocalizedDescription(nameof(Resources.CreateExpressionActivityNameDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<string> Name { get; set; }
        
        [LocalizedDisplayName(nameof(Resources.CreateExpressionActivityExpressionTypeDisplayName))]
        [LocalizedDescription(nameof(Resources.CreateExpressionActivityExpressionTypeDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<ExpressionType> ExpressionType { get; set; }

        #region Overrides of NativeActivity<Expr>

        protected override void Execute(NativeActivityContext context)
        {
            var expr = new Expr(ExpressionType.Get(context), Name.Get(context));
            Result.Set(context, expr);
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            if (Name is null)
            {
                metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Name)));
            }

            if (ExpressionType is null)
            {
                metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(ExpressionType)));
            }
            
            base.CacheMetadata(metadata);
        }

        #endregion
    }
}
