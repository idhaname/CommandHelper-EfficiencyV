﻿using cbhk_environment.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cbhk_environment.Generators.RecipeGenerator.Components
{
    /// <summary>
    /// Smoker.xaml 的交互逻辑
    /// </summary>
    public partial class Smoker : UserControl
    {
        //获取被切物品的引用
        Image SmokedItem = null;
        //获取多选模式引用
        TextToggleButtons MultipleMode = null;
        //空内容图像
        ImageSource empty_image = null;
        //物品编辑模式
        recipe_datacontext.RecipeModifyTypes modifyTypes = new recipe_datacontext.RecipeModifyTypes();
        //被切物品列表
        private List<Image> SmokedItemList = new List<Image> { };
        //显示物品信息的窗体
        public ItemsDisplayer ItemInfomationWindow = new ItemsDisplayer();
        //是否显示物品信息窗体
        bool DisplayCurrentItem;
        //背景图文件路径
        string background_path = AppDomain.CurrentDomain.BaseDirectory + "resources\\configs\\Recipe\\images\\smoker.png";

        #region 当前槽位被点击子级物品,用于绑定单个物品的标签
        private Image currentSubItem = null;
        public Image CurrentSubItem
        {
            get { return currentSubItem; }
            set { currentSubItem = value; }
        }
        #endregion

        #region 当前选中的物品
        private Image currentItemImage = null;
        public Image CurrentItemImage
        {
            get { return currentItemImage; }
            set { currentItemImage = value; }
        }
        #endregion

        //获取经验的引用
        Slider RecipeExp = null;
        //获取烧制时间的引用
        Slider RecipeCookingTime = null;
        //获取文件名引用
        public TextBox RecipeFileName = null;

        #region 知识之书图标
        System.Drawing.Bitmap KnowLedgeBook = null;
        BitmapImage KnowLedgeBookImage = null;
        #endregion

        #region 生成结果
        private string recipe_result = "";
        public string RecipeResult
        {
            get
            {
                string result;
                string ExperienceData = "\"experience\":";
                string CookingtimeData = "\"cookingtime\":";

                RecipeExp.Value = int.Parse(RecipeExp.Value.ToString().Contains(".") ? RecipeExp.Value.ToString().Split('.')[0].Replace("-", "") : RecipeExp.Value.ToString());
                RecipeCookingTime.Value = int.Parse(RecipeCookingTime.Value.ToString().Contains(".") ? RecipeCookingTime.Value.ToString().Split('.')[0].Replace("-", "") : RecipeCookingTime.Value.ToString());
                ExperienceData += RecipeExp.Value + ",";
                CookingtimeData += RecipeCookingTime.Value + ",";
                result = ExperienceData + CookingtimeData + "\"result\":\"minecraft:" + recipe_result + "\"";
                return result;
            }
            set { recipe_result = value; }
        }
        #endregion

        #region 最终数据
        public string RecipeData
        {
            get
            {
                string result = "{\"type\":\"minecraft:smoking\"," + (GroupId.Text.Trim() != "" ? "\"group\":\"" + GroupId.Text + "\"," : "");

                string IngredientData = "\"ingredient\":";
                if (SmokedItemList.Count > 1 && MultipleMode.IsChecked.Value)
                {
                    IngredientData += "[";
                    IngredientData += string.Join("", SmokedItemList.Select(item => "{\"item\":\"minecraft:" + item.Tag.ToString() + "\""+ (item.Name.Trim() != "" ? ",\"tag\":\"" + item.Name + "\"" : "") + "},"));
                    IngredientData = IngredientData.TrimEnd(',') + "],";
                }
                else
                if (SmokedItemList.Count >= 1)
                {
                    IngredientData += "{\"item\":\"minecraft:" + SmokedItemList.First().Tag.ToString() + "\""+ (SmokedItemList.First().Name.Trim() != "" ? ",\"tag\":\"" + SmokedItemList.First().Name + "\"" : "") + "},";
                }
                result += SmokedItemList.Count > 0 ? IngredientData + RecipeResult + "}" : RecipeResult + "}";
                return result;
            }
        }
        #endregion

        public Smoker()
        {
            InitializeComponent();

            ItemInfomationWindow.DataContext = this;

            #region 初始化知识之书
            KnowLedgeBook = new System.Drawing.Bitmap(AppDomain.CurrentDomain.BaseDirectory + "resources\\configs\\Recipe\\images\\knowledge_book.png");
            KnowLedgeBook = GeneralTools.ChangeBitmapSize.Magnifier(KnowLedgeBook, 10);
            KnowLedgeBookImage = GeneralTools.BitmapImageConverter.ToBitmapImage(KnowLedgeBook);
            #endregion
        }

        /// <summary>
        /// 移除Tag中的非法字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RemoveIllegalCharacter(object sender, KeyEventArgs e)
        {
            TextBox current_box = sender as TextBox;
            current_box.Text = Regex.Replace(current_box.Text, @"[\\/:*?" + "\"" + "<>|]", "").ToString();
            //current_box.Text = Regex.Replace(current_box.Text, @"^\d+", "").ToString();
        }

        /// <summary>
        /// 被烟熏物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmokedItemLoaded(object sender, RoutedEventArgs e)
        {
            SmokedItem = sender as Image;
        }

        /// <summary>
        /// 是否开启多选模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultipleModeLoaded(object sender, RoutedEventArgs e)
        {
            MultipleMode = sender as TextToggleButtons;
        }

        /// <summary>
        /// 绑定单个物品的tag数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemTagBinder(object sender, MouseButtonEventArgs e)
        {
            #region 动态绑定每个槽位的标签信息
            CurrentSubItem = sender as Image;
            Binding tag_binder = new Binding()
            {
                Path = new PropertyPath("CurrentSubItem.Name"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                FallbackValue = ""
            };
            BindingOperations.SetBinding(ItemInfomationWindow.TagBox, TextBox.TextProperty, tag_binder);
            #endregion
        }

        /// <summary>
        /// 删除对应槽位的物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCacheImage(object sender, MouseButtonEventArgs e)
        {
            Image current_image = sender as Image;
            modifyTypes = recipe_datacontext.RecipeModifyTypes.Delete;
            if (current_image.Uid == "0")
            {
                UpdateMultipleItemView(current_image, modifyTypes, SmokedItemList);
                if (SmokedItemList.Count == 0)
                    SmokedItem.Source = empty_image;
                else
                    if (SmokedItemList.Count == 1)
                {
                    SmokedItem.Source = (ItemInfomationWindow.CurrentItemInfomation.Children[0] as Image).Source;
                }
            }
        }

        /// <summary>
        /// 检测预览视图是否有内容重叠
        /// </summary>
        private int CheckOverLap(Image image)
        {
            int index = -1;
            int count = ItemInfomationWindow.CurrentItemInfomation.Children.Count;
            Image current_image;
            for (int i = 0; i < count; i++)
            {
                current_image = ItemInfomationWindow.CurrentItemInfomation.Children[i] as Image;
                if (current_image.Tag == image.Tag)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// 更新右侧多选预览视图
        /// </summary>
        /// <param name="image">图像控件</param>
        /// <param name="delete">是否删除</param>
        /// <param name="cover">是否覆盖</param>
        public void UpdateMultipleItemView(Image image, recipe_datacontext.RecipeModifyTypes modifyTypes, List<Image> images)
        {
            int index = CheckOverLap(image);
            switch (modifyTypes)
            {
                case recipe_datacontext.RecipeModifyTypes.Add:
                    if (MultipleMode.IsChecked.Value || ItemInfomationWindow.CurrentItemInfomation.Children.Count == 0)
                        ItemInfomationWindow.CurrentItemInfomation.Children.Add(image);
                    else
                    {
                        ItemInfomationWindow.CurrentItemInfomation.Children.Clear();
                        ItemInfomationWindow.CurrentItemInfomation.Children.Add(image);
                    }
                    break;
                case recipe_datacontext.RecipeModifyTypes.Delete:
                    images.Remove(image);
                    ItemInfomationWindow.CurrentItemInfomation.Children.RemoveAt(index);
                    break;
            }
        }

        /// <summary>
        /// 更新物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemUpdate(object sender, DragEventArgs e)
        {
            recipe_datacontext.IsGrabingItem = !recipe_datacontext.IsGrabingItem;

            #region 放大当前图像
            Image current_item = sender as Image;
            string item_id = recipe_datacontext.GrabedImage.Tag.ToString();
            string image_path = AppDomain.CurrentDomain.BaseDirectory + "resources\\data_sources\\item_and_block_images\\" + item_id + ".png";
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image_path);
            bitmap = GeneralTools.ChangeBitmapSize.Magnifier(bitmap, 10);
            BitmapImage bitmapImage = GeneralTools.BitmapImageConverter.ToBitmapImage(bitmap);
            #endregion

            #region 获取空图像引用
            if (empty_image == null)
                empty_image = current_item.Source;
            #endregion

            #region 右侧预览新增图像
            Image cache_image = new Image()
            {
                Height = current_item.Height,
                Cursor = Cursors.Hand,
                Width = current_item.Width,
                Tag = item_id,
                ToolTip = item_id + " 右击删除",
                Uid = current_item.Uid
            };
            ToolTipService.SetInitialShowDelay(cache_image, 0);
            ToolTipService.SetShowDuration(cache_image, 1000);
            cache_image.MouseRightButtonDown += DeleteCacheImage;
            cache_image.MouseLeftButtonDown += ItemTagBinder;
            #endregion

            current_item.Tag = item_id;
            modifyTypes = recipe_datacontext.RecipeModifyTypes.Add;
            switch (current_item.Uid)
            {
                case "0":
                    if (MultipleMode.IsChecked.Value || SmokedItemList.Count == 0)
                    {
                        if (SmokedItemList.Where(item => item.Tag == cache_image.Tag).Count() == 0)
                            SmokedItemList.Add(cache_image);
                        current_item.Source = SmokedItemList.Count < 2 ? bitmapImage : KnowLedgeBookImage;
                        cache_image.Source = bitmapImage;
                        UpdateMultipleItemView(cache_image, modifyTypes, SmokedItemList);
                    }
                    else
                    {
                        current_item.Source = bitmapImage;
                        SmokedItemList[0] = cache_image;
                        cache_image.Source = current_item.Source;
                        UpdateMultipleItemView(cache_image, modifyTypes, SmokedItemList);
                    }
                    break;
                case "1":
                    current_item.Source = bitmapImage;
                    RecipeResult = current_item.Tag.ToString();
                    break;
            }
        }

        /// <summary>
        /// 选中物品,更新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectItemClick(object sender, MouseButtonEventArgs e)
        {
            DisplayCurrentItem = !DisplayCurrentItem;
            Image image = sender as Image;
            Border border = image.Parent as Border;
            if (DisplayCurrentItem)
            {
                Border parent = image.Parent as Border;
                parent.BorderThickness = new Thickness(0);
            }
            border.BorderThickness = new Thickness(5);

            if (DisplayCurrentItem)
            {
                if (SmokedItemList.Count == 1 && ItemInfomationWindow.KeyBox.Text.Trim() == "")
                    ItemInfomationWindow.KeyBox.Text = "m";
                Window window = Window.GetWindow(image);
                Point point = image.TransformToAncestor(window).Transform(new Point(0, 0));
                point = PointToScreen(point);
                ItemInfomationWindow.Left = point.X - 180;
                ItemInfomationWindow.Top = point.Y - 400;
                ItemInfomationWindow.Show();
            }
            else
                ItemInfomationWindow.Hide();
        }

        /// <summary>
        /// 载入背景图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundLoaded(object sender, RoutedEventArgs e)
        {
            Image current_image = sender as Image;
            current_image.Source = new BitmapImage(new Uri(background_path, UriKind.Absolute));
        }

        /// <summary>
        /// 获取文件名引用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecipeFileNameLoaded(object sender, RoutedEventArgs e)
        {
            RecipeFileName = sender as TextBox;
        }

        /// <summary>
        /// 获取经验引用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecipeExpLoaded(object sender, RoutedEventArgs e)
        {
            RecipeExp = sender as Slider;
        }

        /// <summary>
        /// 获取烧制时间引用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecipeCookingTimeLoaded(object sender, RoutedEventArgs e)
        {
            RecipeCookingTime = sender as Slider;
        }

        /// <summary>
        /// 抬起右键后删除合成结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRecipeResultClick(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            image.Source = empty_image;
            RecipeResult = "";
        }
    }
}
