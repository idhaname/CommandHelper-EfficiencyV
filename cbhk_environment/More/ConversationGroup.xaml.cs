﻿

using System.Diagnostics;

namespace cbhk_environment.More
{
    /// <summary>
    /// ConversationGroup.xaml 的交互逻辑
    /// </summary>
    public partial class ConversationGroup
    {
        public ConversationGroup()
        {
            InitializeComponent();
        }

        private void ClickToJoinGroup(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://jq.qq.com/?_wv=1027&k=72W2xZYe");
        }
    }
}
