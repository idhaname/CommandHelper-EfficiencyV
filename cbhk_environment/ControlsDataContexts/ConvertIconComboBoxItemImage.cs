﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace cbhk_environment.ControlsDataContexts
{
    public class ConvertIconComboBoxItemImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is IconComboBoxItem)
            {
                IconComboBoxItem iconComboBoxItem = (IconComboBoxItem)value;
                return iconComboBoxItem.ComboBoxItemIcon;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
