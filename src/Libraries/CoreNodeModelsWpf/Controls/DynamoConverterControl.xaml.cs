using CoreNodeModels;
using Dynamo.Controls;
using System.Windows.Controls;

namespace CoreNodeModelsWpf.Controls
{
    /// <summary>
    /// Interaction logic for ConverterControl.xaml
    /// </summary>
    public partial class DynamoConverterControl : UserControl
    {
        public DynamoConverterControl(DynamoConvert Model, NodeView nodeView)
        {
            InitializeComponent();          
        }      
    }
}
