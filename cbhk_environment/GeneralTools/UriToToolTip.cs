﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace cbhk_environment.GeneralTools
{
    public class UriToToolTip : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            Uri uri = (value as BitmapImage).UriSource;

            int startIndex = uri.ToString().LastIndexOf('/') + 1;
            int endIndex = uri.ToString().LastIndexOf('.');
            string itemID = uri.ToString().Substring(startIndex, endIndex - startIndex);
            string toolTip = "";
            foreach (var item in MainWindow.ItemDataBase)
            {
                if (item.Key.Substring(0, item.Key.IndexOf(':')) == itemID)
                {
                    toolTip = item.Key.Replace(":", " ");
                    break;
                }
            }
            return toolTip;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
