using System.Windows;
using System.Windows.Controls.WpfPropertyGrid;
using Microsoft.Win32;

namespace Pat.Api.Editors
{
    public class FilePathPicker : PropertyEditor
    {
        public FilePathPicker()
        {
            this.InlineTemplate = EditorKeys.FilePathPickerEditorKey;
        }

        public override void ShowDialog(PropertyItemValue propertyValue, IInputElement commandSource)
        {
            if (propertyValue == null) 
                return;
            if (propertyValue.ParentProperty.IsReadOnly) 
                return;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Multiselect = false
            };

            if (ofd.ShowDialog() == true)
            {
                propertyValue.StringValue = ofd.FileName;
            }
        }
    }
}