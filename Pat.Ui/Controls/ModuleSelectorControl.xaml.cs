using System.Windows;
using System.Windows.Controls;
using Pat.BusinessLogic;

namespace Pat.Ui.Controls
{
    public partial class ModuleSelectorControl : UserControl
    {
        public ModuleSelectorControl()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            propertyGrid.SelectedObject = ((IOptionsProvider) DataContext).SelectedModuleOptions;
        }
    }
}
