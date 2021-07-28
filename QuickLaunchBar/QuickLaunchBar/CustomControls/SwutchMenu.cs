﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace QuickLaunchBar.CustomControls
{
    public class SwitchMenu : Selector
    {
        public static class ScrollViewerBehavior
        {
            public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerBehavior), new UIPropertyMetadata(0.0, OnHorizontalOffsetChanged));
            public static void SetHorizontalOffset(FrameworkElement target, double value) => target.SetValue(HorizontalOffsetProperty, value);
            public static double GetHorizontalOffset(FrameworkElement target) => (double)target.GetValue(HorizontalOffsetProperty);
            private static void OnHorizontalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e) => (target as ScrollViewer)?.ScrollToHorizontalOffset((double)e.NewValue);

            public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerBehavior), new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));
            public static void SetVerticalOffset(FrameworkElement target, double value) => target.SetValue(VerticalOffsetProperty, value);
            public static double GetVerticalOffset(FrameworkElement target) => (double)target.GetValue(VerticalOffsetProperty);
            private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e) => (target as ScrollViewer)?.ScrollToVerticalOffset((double)e.NewValue);
        }
        private Button PART_PreviousButton;
        private Button PART_NextButton;
        private Button PART_UpButton;
        private Button PART_DownButton;
        private ScrollViewer PART_ScrollViewer;
        private Rectangle PART_Rectangle;
        private double offset = 70;
        private double recordAnimationOffset = 0.0;

        #region 依赖属性

        #region Orientation
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SwitchMenu), new PropertyMetadata(Orientation.Horizontal));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SwitchMenu), new PropertyMetadata(string.Empty));

        public bool IsDragDrop
        {
            get { return (bool)GetValue(IsDragDropProperty); }
            set { SetValue(IsDragDropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDragDrop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDragDropProperty =
            DependencyProperty.Register("IsDragDrop", typeof(bool), typeof(SwitchMenu), new PropertyMetadata(false, UpdateDragDropPropertyMetadata));

        private static void UpdateDragDropPropertyMetadata(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {

            }
        }

        #endregion

        #endregion

        static SwitchMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchMenu), new FrameworkPropertyMetadata(typeof(SwitchMenu)));
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            ContentControl item = new ContentControl();
            item.MouseLeftButtonUp += item_MouseLeftButtonUp;
            return item;
        }
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                BeginScrollViewerAnimation(this.ActualHeight);
            }
            else
            {
                BeginScrollViewerAnimation(-this.ActualHeight);
            }
        }
        void item_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_PreviousButton = this.GetTemplateChild("PART_PreviousButton") as Button;
            this.PART_NextButton = this.GetTemplateChild("PART_NextButton") as Button;
            this.PART_UpButton = this.GetTemplateChild("PART_UpButton") as Button;
            this.PART_DownButton = this.GetTemplateChild("PART_DownButton") as Button;
            this.PART_ScrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            this.PART_Rectangle = this.GetTemplateChild("PART_Rectangle") as Rectangle;
            if (this.PART_PreviousButton != null)
            {
                this.PART_PreviousButton.Click += PART_PreviousButton_Click;
            }
            if (this.PART_NextButton != null)
            {
                this.PART_NextButton.Click += PART_NextButton_Click;
            }
            if (this.PART_UpButton != null)
            {
                this.PART_UpButton.Click += PART_UpButton_Click;
            }
            if (this.PART_DownButton != null)
            {
                this.PART_DownButton.Click += PART_DownButton_Click;
            }
            if (this.PART_ScrollViewer != null)
            {
                this.PART_ScrollViewer.ScrollChanged += PART_ScrollViewer_ScrollChanged;
                //this.PART_ScrollViewer.MouseMove += PART_ScrollViewer_MouseMove;
                //this.PART_ScrollViewer.MouseLeave += PART_ScrollViewer_MouseLeave;
            }
            this.MouseMove += SwitchMenu_MouseMove;
            this.MouseLeave += SwitchMenu_MouseLeave;
            if (this.PART_Rectangle != null)
            {
                this.PART_Rectangle.IsVisibleChanged += PART_Rectangle_IsVisibleChanged;
            }

            this.PART_ScrollViewer.SetValue(ScrollViewerBehavior.VerticalOffsetProperty, 0.0);

        }

        private void PART_Rectangle_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SwitchMenu_MouseLeave(null, null);
        }

        private void SwitchMenu_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.PART_ScrollViewer != null)
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    this.PART_PreviousButton.Visibility = Visibility.Collapsed;
                    this.PART_NextButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.PART_UpButton.Visibility = Visibility.Collapsed;
                    this.PART_DownButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SwitchMenu_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.PART_ScrollViewer != null && !IsDragDrop)
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    this.PART_PreviousButton.Visibility = (this.PART_ScrollViewer.HorizontalOffset == 0.0) ? Visibility.Hidden : Visibility.Visible;
                    this.PART_NextButton.Visibility = (this.PART_ScrollViewer.ScrollableWidth == this.PART_ScrollViewer.HorizontalOffset) ? Visibility.Hidden : Visibility.Visible;
                }
                else
                {
                    this.PART_UpButton.Visibility = (this.PART_ScrollViewer.VerticalOffset == 0.0) ? Visibility.Hidden : Visibility.Visible;
                    this.PART_DownButton.Visibility = (this.PART_ScrollViewer.ScrollableHeight == this.PART_ScrollViewer.VerticalOffset) ? Visibility.Hidden : Visibility.Visible;
                }
            }
        }

        private void PART_UpButton_Click(object sender, RoutedEventArgs e)
        {
            BeginScrollViewerAnimation(-this.ActualHeight);
            //this.ScrollToOffset(Orientation.Vertical, -this.offset);
        }

        private void PART_DownButton_Click(object sender, RoutedEventArgs e)
        {
            BeginScrollViewerAnimation(this.ActualHeight);
            //this.ScrollToOffset(Orientation.Vertical, this.offset);
        }
        void PART_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }
        void PART_PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            this.ScrollToOffset(Orientation.Horizontal, -this.offset);
        }
        void PART_NextButton_Click(object sender, RoutedEventArgs e)
        {
            this.ScrollToOffset(Orientation.Horizontal, this.offset);
        }
        void ScrollToOffset(Orientation orientation, double scrollOffset)
        {
            if (this.PART_ScrollViewer == null)
            {
                return;
            }
            switch (orientation)
            {
                case Orientation.Horizontal:
                    this.PART_ScrollViewer.ScrollToHorizontalOffset(this.PART_ScrollViewer.HorizontalOffset + scrollOffset);
                    break;
                case Orientation.Vertical:
                    this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset + scrollOffset);
                    break;
                default:
                    break;
            }
        }

        void BeginScrollViewerAnimation(double animationOffset)
        {
            EasingFunctionBase easeFunction = new CubicEase()
            {
                EasingMode = EasingMode.EaseOut,
            };
            var doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.6)),
                EasingFunction = easeFunction
            };

            recordAnimationOffset = recordAnimationOffset + animationOffset;
            if (recordAnimationOffset > this.PART_ScrollViewer.ExtentHeight
                ||
                recordAnimationOffset < 0)
            {
                recordAnimationOffset = recordAnimationOffset - animationOffset;
                return;
            }
            doubleAnimation.To = recordAnimationOffset;
            doubleAnimation.Completed += DoubleAnimation_Completed;

            this.PART_ScrollViewer.BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, doubleAnimation);
            this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset + recordAnimationOffset);


        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            //Console.WriteLine($"this.PART_ScrollViewer.VerticalOffset Completed {this.PART_ScrollViewer.VerticalOffset }");
            if (this.PART_ScrollViewer != null && !IsDragDrop)
            {
                if (this.Orientation == Orientation.Vertical)
                {
                    this.PART_UpButton.Visibility = (this.PART_ScrollViewer.VerticalOffset < this.ActualHeight) ? Visibility.Hidden : Visibility.Visible;
                    this.PART_DownButton.Visibility = (this.PART_ScrollViewer.VerticalOffset == this.PART_ScrollViewer.VerticalOffset) ? Visibility.Hidden : Visibility.Visible;
                }
            }
        }
    }
}
