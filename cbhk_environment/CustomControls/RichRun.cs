﻿using cbhk_environment.ControlsDataContexts;
using cbhk_environment.Generators.WrittenBookGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;

namespace cbhk_environment.CustomControls
{
    public class RichRun:Run
    {
        /// <summary>
        /// 迭代内容序列
        /// </summary>
        public List<char> Obfuscates;
        //开启混淆
        public bool IsObfuscated = false;
        /// <summary>
        /// 迭代结果
        /// </summary>
        StringBuilder ObfuscatesResult = new StringBuilder() { };
        /// <summary>
        /// 迭代器
        /// </summary>
        Random random = new Random();

        #region UID
        private string uid = "";
        public string UID
        {
            get { return uid; }
            set
            {
                uid = value;
            }
        }
        #endregion

        /// <summary>
        /// 当前混淆最长长度
        /// </summary>
        double MaxContentLength = 0;

        public System.Windows.Forms.Timer ObfuscateTimer = new System.Windows.Forms.Timer()
        {
            Interval = 10,
            Enabled = false
        };

        #region 保存事件数据
        private bool hasClickEvent = false;

        public bool HasClickEvent
        {
            get { return hasClickEvent; }
            set
            {
                hasClickEvent = value;
            }
        }

        private bool hasHoverEvent = false;

        public bool HasHoverEvent
        {
            get { return hasHoverEvent; }
            set
            {
                hasHoverEvent = value;
            }
        }

        private bool hasInsertion = false;

        public bool HasInsertion
        {
            get { return hasInsertion; }
            set
            {
                hasInsertion = value;
            }
        }

        private TextSource clickEventActionItem = new TextSource() { ItemText = "运行命令" };
        public TextSource ClickEventActionItem
        {
            get { return clickEventActionItem; }
            set
            {
                clickEventActionItem = value;
            }
        }
        private string clickEventValue = "";
        public string ClickEventValue
        {
            get { return clickEventValue; }
            set
            {
                clickEventValue = value;
            }
        }
        private TextSource hoverEventActionItem = new TextSource() { ItemText = "显示文本" };
        public TextSource HoverEventActionItem
        {
            get { return hoverEventActionItem; }
            set
            {
                hoverEventActionItem = value;
            }
        }
        private string hoverEventValue = "";
        public string HoverEventValue
        {
            get { return hoverEventValue; }
            set
            {
                hoverEventValue = value;
            }
        }

        private string insertionValue = "";
        public string InsertionValue
        {
            get { return insertionValue; }
            set
            {
                insertionValue = value;
            }
        }

        #region 最终事件数据
        public string EventData
        {
            get
            {
                string result = "";
                string ClickEventString = HasClickEvent? ",\"clickEvent\":{\"action\":\""+(written_book_datacontext.EventDataBase.Where(item => item.Value == ClickEventActionItem.ItemText.Trim()).First().Key)+"\"value\":\""+ClickEventValue+"\"}":"";

                string HoverEventString = HasHoverEvent ? ",\"hoverEvent\":{\"action\":\"" + (written_book_datacontext.EventDataBase.Where(item => item.Value == HoverEventActionItem.ItemText.Trim()).First().Key) + "\"value\":\"" + HoverEventValue + "\"}" : "";
                string InsertionString = HasInsertion ? ",\"insertion\":{\"action\":\"" + (written_book_datacontext.EventDataBase.Where(item => item.Value == HoverEventActionItem.ItemText.Trim()).First().Key) + "\"value\":\"" + HoverEventValue + "\"}" : "";
                result = ClickEventString + HoverEventString + InsertionString;
                return result;
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 混淆文字控件
        /// </summary>
        public RichRun()
        {
            if (base.Text.Trim() != "")
                Text = base.Text;
            Obfuscates = written_book_datacontext.obfuscates;
            ObfuscateTimer.Tick += ObfuscateTick;
            MouseEnter += ObfuscateTextMouseEnter;
            MouseLeave += ObfuscateTextMouseLeave;
            MouseLeftButtonDown += ObfuscateTextMouseLeftButtonDown;
            MouseLeftButtonUp += ObfuscateTextMouseLeftButtonUp;
        }

        /// <summary>
        /// 鼠标抬起时启用混淆效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObfuscateTextMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(IsObfuscated)
            ObfuscateTimer.Enabled = true;
        }

        /// <summary>
        /// 鼠标按下时关闭混淆效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObfuscateTextMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsObfuscated)
            {
                ObfuscateTimer.Enabled = false;
                Text = UID;
            }
        }

        /// <summary>
        /// 鼠标移出时启用混淆效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObfuscateTextMouseLeave(object sender, MouseEventArgs e)
        {
            if (IsObfuscated)
                ObfuscateTimer.Enabled = true;
        }

        /// <summary>
        /// 鼠标移入时关闭混淆效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObfuscateTextMouseEnter(object sender, MouseEventArgs e)
        {
            if (IsObfuscated)
            {
                ObfuscateTimer.Enabled = false;
                Text = UID;
            }
        }

        /// <summary>
        /// 混淆效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ObfuscateTick(object sender, EventArgs e)
        {
            MaxContentLength = GeneralTools.GetTextWidth.Get(new Run(UID));
            ObfuscatesResult.Clear();
            for (int i = 0; i < UID.Length; i++)
                ObfuscatesResult.Append(Obfuscates[random.Next(0, Obfuscates.Count - 1)]);
            Text = ObfuscatesResult.ToString();
        }
    }
}
