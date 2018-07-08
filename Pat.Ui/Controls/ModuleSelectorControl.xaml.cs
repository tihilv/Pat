using System.Windows;
using System.Windows.Controls;
using Pat.BusinessLogic;

namespace Pat.Ui.Controls
{
    /// <summary>
    /// Interaction logic for ModuleSelectorControl.xaml
    /// </summary>
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
