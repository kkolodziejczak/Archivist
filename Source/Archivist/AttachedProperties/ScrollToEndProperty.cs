using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Archivist
{
    public class ScrollToEndProperty : BaseAttachedProperty<ScrollToEndProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //TODO: Fix issue with scrolling source path, validation style error.
            if (!(sender is TextBox textbox))
                return;
            textbox.TextChanged -= ScrollToEnd;
            textbox.TextChanged += ScrollToEnd;
        }

        private void ScrollToEnd(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox box))
                return;

            box.Focus();
            box.CaretIndex = box.Text.Length;
            box.ScrollToEnd();

        }

    }
}
