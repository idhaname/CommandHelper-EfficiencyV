﻿using cbhk_environment.CustomControls;
using HandyControl.Tools.Extension;
using System.Windows;
using System.Windows.Controls;

namespace cbhk_environment.Generators.WrittenBookGenerator.Components
{
    /// <summary>
    /// TextEventsForm.xaml 的交互逻辑
    /// </summary>
    public partial class TextEventsForm : UserControl
    {
        #region 允许编辑点击事件
        private bool enableEditClickEvent = false;
        public bool EnableEditClickEvent
        {
            get
            {
                return enableEditClickEvent;
            }
            set
            {
                enableEditClickEvent = value;
                ClickEventPanel.Visibility = EnableEditClickEvent ?Visibility.Visible:Visibility.Collapsed;
            }
        }
        #endregion

        #region 允许编辑悬浮事件
        private bool enableEditHoverEvent = false;
        public bool EnableEditHoverEvent
        {
            get
            {
                return enableEditHoverEvent;
            }
            set
            {
                enableEditHoverEvent = value;
                HoverEventPanel.Visibility = EnableEditHoverEvent ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        #region 允许插入文本
        private bool enableEditInsertion = false;
        public bool EnableEditInsertion
        {
            get
            {
                return enableEditInsertion;
            }
            set
            {
                enableEditInsertion = value;
                InsertionPanel.Visibility = EnableEditInsertion ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        public TextEventsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 载入点击事件的成员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickEventsLoaded(object sender, RoutedEventArgs e)
        {
            TextComboBoxs textComboBoxs = sender as TextComboBoxs;
            textComboBoxs.ItemsSource = written_book_datacontext.clickEventSource;
        }

        /// <summary>
        /// 载入悬浮事件的成员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HoverEventsLoaded(object sender, RoutedEventArgs e)
        {
            TextComboBoxs textComboBoxs = sender as TextComboBoxs;
            textComboBoxs.ItemsSource = written_book_datacontext.hoverEventSource;
        }

        /// <summary>
        /// 点击事件开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableClickEventClick(object sender, RoutedEventArgs e)
        {
            ClickEventPanel.Visibility = (sender as RadiusToggleButtons).IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// 悬浮事件开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableHoverEventClick(object sender, RoutedEventArgs e)
        {
            HoverEventPanel.Visibility = (sender as RadiusToggleButtons).IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// 插入事件开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableInsertionClick(object sender, RoutedEventArgs e)
        {
            InsertionPanel.Visibility = (sender as RadiusToggleButtons).IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
