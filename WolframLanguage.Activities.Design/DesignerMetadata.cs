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

            var categoryAttribute =  new CategoryAttribute($"{Resources.Category}");


            builder.AddCustomAttributes(typeof(WolframLanguageScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(WolframLanguageScope), new DesignerAttribute(typeof(ParentScopeDesigner)));
            builder.AddCustomAttributes(typeof(WolframLanguageScope), new HelpKeywordAttribute("https://go.uipath.com"));

            builder.AddCustomAttributes(typeof(EvaluateActivity<string>), categoryAttribute);
            builder.AddCustomAttributes(typeof(EvaluateActivity<string>), new DesignerAttribute(typeof(ChildActivityDesigner)));
            builder.AddCustomAttributes(typeof(EvaluateActivity<string>), new HelpKeywordAttribute("https://go.uipath.com"));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
