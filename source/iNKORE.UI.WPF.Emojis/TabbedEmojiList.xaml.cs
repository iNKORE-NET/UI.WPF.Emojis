﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iNKORE.UI.WPF.Emojis
{
    public class EmojiPickedEventArgs : RoutedEventArgs
    {
        public EmojiPickedEventArgs() { RoutedEvent = TabbedEmojiList.EmojiPickedEvent; }
        public EmojiPickedEventArgs(string emoji) : this()
        {
            Emoji = emoji;
        }

        public string Emoji;
    }

    public delegate void EmojiPickedEventHandler(object sender, EmojiPickedEventArgs e);

    /// <summary>
    /// TabbedEmojiList.xaml 的交互逻辑
    /// </summary>
    public partial class TabbedEmojiList : UserControl
    {
        public TabbedEmojiList()
        {
            InitializeComponent();
        }

        public static readonly Style DefaultPopupBorderStyle = new Style()
        {
            TargetType = typeof(Border),
            Setters =
            {
                new Setter(BackgroundProperty, Color.FromArgb(255,249,249,249).ToBrush()),
                new Setter(BorderBrushProperty, Color.FromArgb(255,228,228,228).ToBrush()),
                new Setter(BorderThicknessProperty, new Thickness(1)),
                new Setter(Border.PaddingProperty, new Thickness(8)),
                new Setter(Border.CornerRadiusProperty, new CornerRadius(8)),
            }
        };

        public static readonly Style DefaultEmojiToggleButtonStyle = new Style()
        {
            TargetType = typeof(ToggleButton),
            Setters =
            {
                new Setter(BackgroundProperty, Brushes.Transparent),
                new Setter(BorderBrushProperty, null),
                new Setter(PaddingProperty, new Thickness(4)),
            }

        };

        public IList<EmojiData.Group> EmojiGroups => EmojiData.AllGroups;

        //public event EmojiPickedEventHandler EmojiPicked;

        public static readonly RoutedEvent EmojiPickedEvent = EventManager.RegisterRoutedEvent(nameof(EmojiPicked), RoutingStrategy.Direct, typeof(EmojiPickedEventHandler), typeof(TabbedEmojiList));
        public event EmojiPickedEventHandler EmojiPicked
        {
            add { this.AddHandler(EmojiPickedEvent, value); }
            remove { this.RemoveHandler(EmojiPickedEvent, value); }
        }

        public static readonly DependencyProperty PopupBorderStyleProperty = DependencyProperty.Register(nameof(PopupBorderStyle), typeof(Style), typeof(TabbedEmojiList), new PropertyMetadata(DefaultPopupBorderStyle));
        public Style PopupBorderStyle
        {
            get { return (Style)GetValue(PopupBorderStyleProperty); }
            set { SetValue(PopupBorderStyleProperty, value); }
        }

        public static readonly DependencyProperty EmojiToggleButtonStyleProperty = DependencyProperty.Register(nameof(EmojiToggleButtonStyle), typeof(Style), typeof(TabbedEmojiList), new PropertyMetadata(DefaultEmojiToggleButtonStyle));
        public Style EmojiToggleButtonStyle
        {
            get { return (Style)GetValue(EmojiToggleButtonStyleProperty); }
            set { SetValue(EmojiToggleButtonStyleProperty, value); }
        }


        public static readonly DependencyProperty EmojiItemSizeProperty = DependencyProperty.Register(nameof(EmojiItemSize), typeof(double), typeof(TabbedEmojiList), new PropertyMetadata(24d));
        public double EmojiItemSize
        {
            get { return (double)GetValue(EmojiItemSizeProperty); }
            set { SetValue(EmojiItemSizeProperty, value); }
        }


        //public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(string), typeof(TabbedEmojiList), new PropertyMetadata(null, SelectedItemProperty_ValueChanged));
        //public string SelectedItem
        //{
        //    get { return (string)GetValue(SelectedItemProperty); }
        //    set {  SetValue(SelectedItemProperty, value); }
        //}


        //private static void SelectedItemProperty_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as TabbedEmojiList)?.SelectedItemProperty_ValueChanged(d, e);
        //}

        //private void SelectedItemProperty_ValueChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{

        //}


        private void OnEmojiPicked(object sender, RoutedEventArgs e)
        {
            if (sender is Control control && control.DataContext is EmojiData.Emoji emoji)
            {
                if (emoji.VariationList.Count == 0 || control.Name != "VariationButton" || sender is Button)
                {
                    var selectedItem = emoji.Text;
                    //EmojiPicked?.Invoke(this, new EmojiPickedEventArgs(emoji.Text));
                    this.RaiseEvent(new EmojiPickedEventArgs(selectedItem));

                    if (sender is ToggleButton tgb)
                    {
                        tgb.IsChecked = false;
                    }
                }
            }
        }

    }
}
