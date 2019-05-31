using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using WolframLanguage.Activities.Design.Properties;

namespace WolframLanguage.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");
            
            builder.AddCustomAttributes(typeof(WolframLanguageScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(WolframLanguageScope), new DesignerAttribute(typeof(ParentScopeDesigner)));
            builder.AddCustomAttributes(typeof(WolframLanguageScope), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(EvaluateActivity<>), categoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateActivity<>), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateActivity<>), new HelpKeywordAttribute("https://troyweb.com"));

            builder.AddCustomAttributes(typeof(EvaluateToImageActivity), categoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateToImageActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateToImageActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(EvaluateToInputFormActivity), categoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateToInputFormActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateToInputFormActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            builder.AddCustomAttributes(typeof(EvaluateToOutputFormActivity), categoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateToOutputFormActivity), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateToOutputFormActivity), new HelpKeywordAttribute("https://troyweb.com"));
            
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
