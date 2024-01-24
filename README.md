<p align="center">
  <a target="_blank" rel="noopener noreferrer">
    <img width="128" src="https://raw.githubusercontent.com/iNKORE-Public/.github/main/assets/Inkore_Badge.png?raw=true)" alt="iNKORE Logo">
  </a>
</p>

<p align="center">System emoji support for your WPF applications.</p>

<h1 align="center">
  iNKORE.UI.WPF.Emojis
</h1>

<p align="center">Give us a star if you like this!</p>

<p align="center">
  <a href="https://github.com/iNKORE-Public/UI.WPF.Emojis/releases"><img src="https://img.shields.io/github/downloads/iNKORE-Public/UI.WPF.Emojis/total?color=%239F7AEA" alt="Release Downloads"></a>
  <a href="#"><img src="https://img.shields.io/github/repo-size/iNKORE-Public/UI.WPF.Emojis?color=6882C4" alt="GitHub Repo Size"></a>
  <a href="#"><img src="https://img.shields.io/github/last-commit/iNKORE-Public/UI.WPF.Emojis?color=%23638e66" alt="Last Commit"></a>
  <a href="#"><img src="https://img.shields.io/github/issues/iNKORE-Public/UI.WPF.Emojis?color=f76642" alt="Issues"></a>
  <a href="#"><img src="https://img.shields.io/github/v/release/iNKORE-Public/UI.WPF.Emojis?color=%4CF4A8B4" alt="Latest Version"></a>
  <a href="#"><img src="https://img.shields.io/github/release-date/iNKORE-Public/UI.WPF.Emojis?color=%23b0a3e8" alt="Release Date"></a>
  <a href="https://github.com/iNKORE-Public/UI.WPF.Emojis/commits/"><img src="https://img.shields.io/github/commit-activity/m/iNKORE-Public/UI.WPF.Emojis" alt="Commit Activity"></a>
  <a href="https://www.nuget.org/packages/iNKORE.UI.WPF.Emojis"><img src="https://img.shields.io/nuget/v/iNKORE.UI.WPF.Emojis?color=blue&logo=nuget" alt="Nuget latest version"></a>
  <a href="https://www.nuget.org/packages/iNKORE.UI.WPF.Emojis"><img src="https://img.shields.io/nuget/dt/iNKORE.UI.WPF.Emojis?color=blue&logo=nuget" alt="Nuget download conut"></a>
</p>

<p align="center">
  <a href="https://github.com/iNKORE-Public/UI.WPF.Emojis/network/members"><img src="https://img.shields.io/github/forks/iNKORE-Public/UI.WPF.Emojis?style=social" alt="Forks"></a>
  <a href="https://github.com/iNKORE-Public/UI.WPF.Emojis/stargazers"><img src="https://img.shields.io/github/stars/iNKORE-Public/UI.WPF.Emojis?style=social" alt="Stars"></a>
  <a href="https://github.com/iNKORE-Public/UI.WPF.Emojis/watchers"><img src="https://img.shields.io/github/watchers/iNKORE-Public/UI.WPF.Emojis?style=social" alt="Watches"></a>
  <a href="https://github.com/iNKORE-Public/UI.WPF.Emojis/discussions"><img src="https://img.shields.io/github/discussions/iNKORE-Public/UI.WPF.Emojis?style=social" alt="Discussions"></a>
  <a href="https://discord.gg/m6NPNVk4bs"><img src="https://img.shields.io/discord/1092738458805608561?style=social&label=Discord&logo=discord" alt="Discord"></a>
  <a href="https://twitter.com/NotYoojun"><img src="https://img.shields.io/twitter/follow/NotYoojun?style=social" alt="NotYoojun's Twitter"></a>
</p>

![](docs/images/Screenshot%202024-01-24%20154647.png)

# ✨ Features

 - Provides drop-in replacements for `TextBlock` and `RichTextBox`, no additional code required.
 - **Colored emoji**! 😨 💩 🍰 ✈️ ✏️ 📞 ☘️
 - **Multiracial family emoji**! 👩🏿‍👩🏻‍👦🏽 👨🏻‍👩🏿‍👧🏽‍👦🏽 👩🏻‍👶🏽
 - **Emoji for flags**! <img src="https://github.com/iNKORE-Public/UI.WPF.Emojis/raw/main/docs/images/flags.png" height="24"/>
 - **Win11 style flags**! <img src="https://github.com/iNKORE-Public/UI.WPF.Emojis/raw/main/docs/images/newflags.png" height="16"/>
 - **Full vector emoji**! Render at huge sizes without quality loss.
 - Optional support for subpixel antialiasing.
 - **Lightweight**; does not embed a font or emoji images; just uses the system font.
 - Works with **old .NET versions** such as .NET Framework 4.0.
 - Can work on **Windows 7 or Windows 8** by installing the Segoe UI Emoji font in `C:/Windows/Fonts`.
 - Available as a [Nuget package](https://www.nuget.org/packages/iNKORE.UI.WPF.Emojis).

 # 🧾 Details

## Available classes

 - `EmojiedTextBlock`: an emoji-aware version of `System.Windows.Controls.TextBlock`.
 - `EmojiedRichTextBox`: an emoji-aware version of `System.Windows.Controls.RichTextBox`.
 - `EmojiPicker`: an emoji picker

## Available dependency properties

 - `EmojiImage.Source`: attach to either `System.Windows.Controls.Image` control or
   `System.Windows.Media.DrawingImage` object in order to manipulate emoji images

## Available runtime flags

 - `bool EmojiData.EnableSubPixelRendering`: enable subpixel rendering, defaults to `false`
 - `bool EmojiData.EnableWindowsStyleFlags`: enable flag rendering, defaults to autodetected

## How does it work

- Emoji.Wpf renders emoji as vector images, using the WPF text rendering engine. The geometry information is found in the Segoe UI Emoji font glyphs. The colour information is found in the same font, using Microsoft’s COLR/CPAL format extensions.

# 🤔 Examples

Here is how to use Emoji.Wpf in your XAML:

```xml
<Window x:Class="Demo.DemoWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:clr="clr-namespace:System;assembly=mscorlib"
        xmlns:emoji="http://schemas.inkore.net/lib/ui/wpf/emojis"
        mc:Ignorable="d"
        Title="iNKORE.UI.WPF.Emojis Demo" Width="640" Height="480">
        <Window.Resources>
            <DrawingImage x:Key="MyImageSource" emoji:Image.Source="👻"/>
        </Window.Resources>
        
        <StackPanel>
            <emoji:EmojiedRichTextBox FontSize="24" Margin="5"/>
            <emoji:EmojiedTextBlock FontSize="24" Text="Hello! 💖😁🐨🐱‍🐉👩🏿‍👩🏻‍👦🏽 lol"/>
            <emoji:EmojiPicker FontSize="40"/>
            <Image Source="{StaticResource MyImageSource}"/>
            <Image emoji:EmojiImage.Source="🦑"/>
        </StackPanel>
    </Window>
```

More classes are to come, but feedback on what is needed is welcome.

# 🙋🏻‍♂️ Contribution

- This repository is based on https://github.com/samhocevar/emoji.wpf

- Want to contribute? The team encourages community feedback and contributions.

- If the project is not working properly, please file a report. We welcome any issues and pull requests submitted on GitHub.

- Sponsor us at https://inkore.net/about/members/notyoojun#sponsor
