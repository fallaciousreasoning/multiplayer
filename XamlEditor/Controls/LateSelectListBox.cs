using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace XamlEditor.Controls
{
    public class LateSelectListBox : ListBox
    {
        public static readonly DependencyProperty ActiveItemProperty =
            DependencyProperty.Register("ActiveItem", typeof(object),
                typeof(LateSelectListBox), new FrameworkPropertyMetadata(null));

        public object ActiveItem
        {
            get { return GetValue(ActiveItemProperty); }
            set { SetValue(ActiveItemProperty, value); }
        }

        private object toSelect;


        public LateSelectListBox()
        {
            SelectionChanged += (sender, args) =>
            {
                if (SelectedItem != ActiveItem)
                {
                    toSelect = SelectedItem;
                    SelectedItem = ActiveItem;
                }
            };
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (e.ChangedButton != MouseButton.Left) return;

            ActiveItem = toSelect;
            SelectedItem = ActiveItem;
        }
    }
}
