using System.Windows;

namespace Pat.Api.Ui
{
    public static class EditorKeys
    {
        private static readonly ComponentResourceKey _filePathPickerEditorKey = new ComponentResourceKey(typeof(EditorKeys), "FilePathPickerEditor");

        public static ComponentResourceKey FilePathPickerEditorKey
        {
            get { return _filePathPickerEditorKey; }
        }
    }

}
