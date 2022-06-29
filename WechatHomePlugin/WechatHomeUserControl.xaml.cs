using MyLibraryBase.Model.Article;
using MyLibraryBase.View.UserControlView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WechatHomePlugin
{
    /// <summary>
    /// WechatHomeUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class WechatHomeUserControl : UserControl
    {
        private StyleWechatModel styleModel;
        private BaseWindow window;

        public WechatHomeUserControl(ArticleAllModel articleAllModel, BaseWindow window)
        {
            InitializeComponent();

            this.window = window;

            StyleWechatModel wechatStyleModels = new StyleWechatModel();
            foreach (var item in articleAllModel.Content)
            {
                if (item is ArticleTextModel)
                {
                    if (string.IsNullOrEmpty(wechatStyleModels.Title))
                    {
                        wechatStyleModels.Title = ((ArticleTextModel)item).Text;
                    }
                    else
                    {
                        wechatStyleModels.Child.Add(
                            new StyleWechatModel.StyleWechatChildModel() { Text = ((ArticleTextModel)item).Text, Link = ((ArticleTextModel)item).Link }
                        );
                    }
                }
            }

            this.styleModel = wechatStyleModels;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbTitle.Text = styleModel.Title;

            bTop.Height = ActualHeight / 4.0;
            ellipseTop.Width = ActualWidth * 2;
            ellipseTop.Height = ActualHeight / 1.5;
            ellipseTop.Margin = new Thickness(-ActualWidth * 0.5, -ellipseTop.Height * 0.4, 0, 0);

            for (int i = 0; i < styleModel.Child.Count; i++)
            {
                Border bContent = new Border
                {
                    Tag = styleModel.Child[i].Link,
                    Background = new SolidColorBrush(Colors.White),
                    CornerRadius = new CornerRadius(5),
                    Margin = new Thickness(20, 0, 20, 0)
                };
                bContent.Loaded += BContent_Loaded;
                bContent.MouseLeftButtonUp += BContent_MouseLeftButtonUp;
                bContent.MouseEnter += bDatabase_MouseEnter;
                bContent.MouseLeave += bDatabase_MouseLeave;
                gContent.Children.Add(bContent);

                Grid.SetColumn(bContent, i);

                TextBlock tbContent = new TextBlock();
                tbContent.Text = styleModel.Child[i].Text;
                tbContent.FontSize = 24;
                tbContent.Foreground = new SolidColorBrush(Colors.Black);
                tbContent.HorizontalAlignment = HorizontalAlignment.Center;
                tbContent.VerticalAlignment = VerticalAlignment.Center;
                tbContent.TextWrapping = TextWrapping.Wrap;
                tbContent.TextAlignment = TextAlignment.Center;
                bContent.Child = tbContent;
            }
        }

        private void BContent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty((sender as Border).Tag.ToString()))
            {
                return;
            }
            window.OpenFile((sender as Border).Tag.ToString());
            //ArticleNewUserControl articleNewUserControl = new ArticleNewUserControl((sender as Border).Tag.ToString());
            //MyStaticResource.mw.AddUserControl(articleNewUserControl);
        }

        private void BContent_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as Border).Height = (sender as Border).ActualWidth;
        }

        /// <summary>
        /// 缩放动画
        /// </summary>
        /// <param name="element">控件名</param>
        /// <param name="RenderX">变换起点X坐标</param>
        /// <param name="RenderY">变换起点Y坐标</param>
        /// <param name="Sizefrom">开始大小</param>
        /// <param name="Sizeto">结束大小</param>
        /// <param name="power">过渡强度</param>
        /// <param name="time">持续时间，例如3秒： TimeSpan(0,0,3) </param>
        public void ScaleEasingAnimationShow(UIElement element, double RenderX, double RenderY, double Sizefrom, double Sizeto, int power, TimeSpan time)
        {
            ScaleTransform scale = new ScaleTransform();  //旋转
            element.RenderTransform = scale;
            //定义圆心位置
            element.RenderTransformOrigin = new System.Windows.Point(RenderX, RenderY);
            //定义过渡动画,power为过度的强度
            EasingFunctionBase easeFunction = new PowerEase()
            {
                EasingMode = EasingMode.EaseInOut,
                Power = power
            };

            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                From = Sizefrom,                                   //起始值
                To = Sizeto,                                     //结束值
                FillBehavior = FillBehavior.HoldEnd,
                Duration = time,                                 //动画播放时间
                EasingFunction = easeFunction,                   //缓动函数
            };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        private void bDatabase_MouseEnter(object sender, MouseEventArgs e)
        {
            ScaleEasingAnimationShow(sender as Border, 0.5, 0.5, 1, 1.1, 5, new TimeSpan(0, 0, 0, 0, 500));
        }

        private void bDatabase_MouseLeave(object sender, MouseEventArgs e)
        {
            ScaleEasingAnimationShow(sender as Border, 0.5, 0.5, 1.1, 1, 5, new TimeSpan(0, 0, 0, 0, 500));
        }
    }
}
