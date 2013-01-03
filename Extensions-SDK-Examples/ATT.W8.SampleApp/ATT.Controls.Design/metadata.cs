extern alias ControlLibrary;
using System.ComponentModel;
using Win8ControlLibrary = ControlLibrary::ATT.Controls;

using Microsoft.Windows.Design.Metadata;

[assembly: ProvideMetadata(typeof(ATT.Controls.Design.Metadata))]
namespace ATT.Controls.Design
{
    public class Metadata : IProvideAttributeTable
    {
        public AttributeTable AttributeTable
        {
            get
            {
                var builder = new AttributeTableBuilder();
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.AttControl), "ApiKey", new CategoryAttribute("AT&T"));
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.AttControl), "SecretKey", new CategoryAttribute("AT&T"));
                builder.AddCustomAttributes(typeof(Win8ControlLibrary.AttControl), "Endpoint", new CategoryAttribute("AT&T"));
                return builder.CreateTable();
            }
        }
    }
}
