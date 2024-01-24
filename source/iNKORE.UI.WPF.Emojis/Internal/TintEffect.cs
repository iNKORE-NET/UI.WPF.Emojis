﻿//
//  iNKORE.UI.WPF.Emojis — Emoji support for WPF
//
//  Copyright © 2017–2021 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace iNKORE.UI.WPF.Emojis
{
    public class TintEffect : ShaderEffect
    {
        static TintEffect()
        {
            //var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //var stream = assembly.GetManifestResourceStream("/Resources/Shaders/TintEffect.ps");
            m_shader = new PixelShader();

            m_shader.UriSource = new Uri(@"/iNKORE.UI.WPF.Emojis;component/Resources/Shaders/TintEffect.ps", UriKind.Relative);
        }

        public TintEffect()
        {
            PixelShader = m_shader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(TintProperty);
        }

        public static readonly DependencyProperty TintProperty =
            DependencyProperty.Register(nameof(Tint), typeof(Color), typeof(TintEffect),
                new UIPropertyMetadata(Colors.Red, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty(nameof(Input), typeof(TintEffect), 0);

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public Color Tint
        {
            get => (Color)GetValue(TintProperty);
            set => SetValue(TintProperty, value);
        }

        private static PixelShader m_shader;
    }
}
