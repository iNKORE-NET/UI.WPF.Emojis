﻿//
//  iNKORE.UI.WPF.Emojis — Emoji support for WPF
//
//  Copyright © 2017–2022 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using Stfu.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;

namespace iNKORE.UI.WPF.Emojis
{
    public static class EmojiImage
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(EmojiImage),
                new PropertyMetadata(default(string), OnSourceChanged));

        public static void SetSource(DependencyObject o, string value)
            => o.SetValue(SourceProperty, value);

        public static string GetSource(DependencyObject o)
            => (string)o.GetValue(SourceProperty);


        public static readonly DependencyProperty DrawPaddingProperty =
            DependencyProperty.RegisterAttached("DrawPadding", typeof(bool), typeof(EmojiImage),
                new PropertyMetadata(true, OnSourceChanged));


        public static void SetDrawPadding(DependencyObject o, bool value)
            => o.SetValue(DrawPaddingProperty, value);

        public static bool GetDrawPadding(DependencyObject o)
            => (bool)o.GetValue(DrawPaddingProperty);

        private static void OnSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var drawPadding = GetDrawPadding(o);
            var source = GetSource(o);

            if (e.Property == SourceProperty || !string.IsNullOrEmpty(source))
            {
                if (o is Image image)
                {
                    var di = new DrawingImage();
                    SetDrawPadding(di, drawPadding);
                    SetSource(di, source);
                    image.Source = di;
                }
                else if (o is DrawingImage di)
                {
                    di.Drawing = RenderEmoji(source, out var width, out var height, drawPadding);
                }
            }
        }

        private static ResourceDictionary m_win10_flags = new Win10Flags();
        private static ResourceDictionary m_win11_flags = new Win11Flags();

        // Metrics from Segoe UI Emoji, measured on 😄 U+1F600 GRINNING FACE.
        private const double FONT_EM_SIZE = 2048;
        private const double FONT_GLYPH_SIZE = 2300; // both horizontal and vertical
        private const double FONT_TOP_PADDING = 341;
        private const double FONT_BOTTOM_PADDING = 83;
        private const double FONT_SIDE_PADDING = 256;

        private static Rect PadRect(Rect bounds)
        {
            // Compute padding by assuming the glyph is full-height.
            double top = bounds.Height * FONT_TOP_PADDING / FONT_GLYPH_SIZE;
            double bottom = bounds.Height * FONT_BOTTOM_PADDING / FONT_GLYPH_SIZE;
            double sides = bounds.Height * FONT_SIDE_PADDING / FONT_GLYPH_SIZE;
            return new Rect(new Point(bounds.Left - sides, bounds.Top - top),
                            new Point(bounds.Right + sides, bounds.Bottom + bottom));
        }

        public static DrawingGroup RenderEmoji(string text, out double width, out double height, bool drawPadding)
        {
            var dg = new DrawingGroup();
            var flags = EmojiData.Typeface.HasWin11Emoji ? m_win11_flags : m_win10_flags;
            
            using (var dc = dg.Open())
            {
                if (EmojiData.GetDrawing(text) is Drawing d)
                {
                    // In case the user provided a bitmap image, we want high quality scaling
                    RenderOptions.SetBitmapScalingMode(dg, BitmapScalingMode.HighQuality);
                    dc.DrawDrawing(d);

                    var padding = PadRect(d.Bounds);
                    if (drawPadding)
                    {
                        dc.DrawRectangle(Brushes.Transparent, null, padding);
                    }

                    height = (FONT_TOP_PADDING + FONT_GLYPH_SIZE + FONT_BOTTOM_PADDING) / FONT_EM_SIZE;
                    width = height * padding.Width / padding.Height;
                }
                else if (EmojiData.EnableWindowsStyleFlags && flags[text] is DrawingGroup flag)
                {
                    var h = 0d;
                    var w = 0d;

                    flag.Dispatcher.Invoke(() =>
                    {
                        GeometryDrawing clip = null, outline;

                        // Switzerland and Vatican City have square flags, Nepal has a special shape
                        var style = text == "🇨🇭" || text == "🇻🇦" ? "square"
                                  : text == "🇳🇵" ? "nepal" : "rectangle";

                        var dg2 = new DrawingGroup();

                        using(var dc2 = dg2.Open())
                        {
                            if (EmojiData.Typeface.HasWin11Emoji)
                            {
                                clip = flags["clip_" + style] as GeometryDrawing;
                                if (clip != null)
                                    dc2.PushClip(clip.Geometry);
                            }

                            // Draw the actual flag geometry
                            foreach (var child in flag.Children)
                            {
                                dc2.DrawDrawing(child);
                            }

                            if (EmojiData.Typeface.HasWin11Emoji)
                            {
                                outline = flags["bounds_" + style] as GeometryDrawing;
                                if (clip != null)
                                    dc2.Pop();
                            }
                            else
                            {
                                // Draw the flag outline
                                outline = flags[style] as GeometryDrawing;
                                dc2.DrawDrawing(outline);
                                var pole = flags["pole"] as GeometryDrawing;
                                dc2.DrawDrawing(pole);
                            }

                            var padding = PadRect(outline.Bounds);
                            if (drawPadding)
                            {
                                dc2.DrawRectangle(Brushes.Transparent, null, padding);
                            }

                            h = (FONT_TOP_PADDING + FONT_GLYPH_SIZE + FONT_BOTTOM_PADDING) / FONT_EM_SIZE;
                            w = h * padding.Width / padding.Height;
                        }

                        dg = (DrawingGroup)dg2.GetAsFrozen();
                    });

                    height = h;
                    width = w;
                }
                else
                {
                    RenderText(dc, text, out width, out height, drawPadding);
                }
            }

            return dg;
        }

        private static void RenderText(DrawingContext dc, string text,
                                       out double width, out double height, bool drawPadding)
        {
            var font = EmojiData.Typeface;
            var glyphplanlist = font.MakeGlyphPlanList(text);

            var scale = font.GetScale(0.75); // 1px = 0.75pt
            width = glyphplanlist.Where(g => g.glyphIndex != font.ZwjGlyph)
                                 .Sum(g => g.AdvanceX) * scale;
            if (EmojiData.EnableZwjRenderingFallback)
            {
                width -= glyphplanlist.WithPreviousAndNext()
                                      .Skip(1)
                                      .Where(t => t.Current.glyphIndex == font.ZwjGlyph
                                               && t.Previous.AdvanceX != font.ZwjGlyph)
                                      .Sum(t => t.Previous.AdvanceX) * scale;
                // FIXME: width may be < 0 (reproduced on Windows 8), investigate one day
                width = Math.Max(width, 0);
            }
            height = font.Height;

            // Clip to the render area, and draw a transparent rectangle to avoid
            // automatic resizing. See https://stackoverflow.com/a/8824459/111461
            var area = new Rect(0, 0, width, height);
            dc.PushClip(new RectangleGeometry(area));

            if (drawPadding)
            {
                dc.DrawRectangle(Brushes.Transparent, null, area);
            }

            // Render our image
            int startx = 0;

            foreach (var t in glyphplanlist.WithPreviousAndNext())
            {
                var g = t.Current;
                var xpos = (startx + g.OffsetX) * scale;
                var ypos = font.Baseline + g.OffsetY * scale;
                double ds = 1.0;

                if (g.glyphIndex == font.ZwjGlyph)
                    continue;

                if (EmojiData.EnableZwjRenderingFallback)
                {
                    if (t.Next.glyphIndex == font.ZwjGlyph)
                    {
                        ds = 0.75;
                        ypos -= 0.25 / ds;
                    }
                    else if (t.Previous.glyphIndex == font.ZwjGlyph)
                    {
                        ds = 0.75;
                        xpos += 0.25 / ds;
                    }
                }

                dc.PushTransform(new MatrixTransform(ds, 0, 0, ds, xpos, ypos));
                foreach ((var gr, var br) in font.DrawGlyph(g.glyphIndex))
                {
                    if (EmojiData.EnableSubPixelRendering)
                        dc.DrawGlyphRun(br, gr);
                    else
                        dc.DrawGeometry(br, null, gr.BuildGeometry());
                }
                dc.Pop();

                if (EmojiData.EnableZwjRenderingFallback && t.Next.glyphIndex == font.ZwjGlyph)
                    continue;

                startx += g.AdvanceX;
            }
        }
    }
}
