﻿using iNKORE.UI.WPF.Emojis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]


[assembly: AssemblyTitle("iNKORE.UI.WPF.Emojis")]

//[assembly: InternalsVisibleTo("iNKORE.UI.WPF.Modern.Controls, PublicKey=002400000480000094000000060200000024000052534131000400000100010079425c15485b66fccee121200091aa9712fb3044894e6c09e0278eac59e28d966fda1e6084f86fd80c08e946c5e9da07b7f88e82f936df2eb2a5c7e4ea154243502c17bf805002ebd1997873464ff7c8847b1e2aa02d470864d058ea1383ea3e6f8a0f02c4af08093c36beff569cf04a04a73054c0d3e52f6e2b18e1f98412ce")]
//[assembly: InternalsVisibleTo("iNKORE.UI.WPF.Modern.MahApps")]
//[assembly: InternalsVisibleTo("MUXControlsTestApp")]

[assembly: XmlnsPrefix(EmojiBehaviors.XmlNamespace, "emoji")]
[assembly: XmlnsDefinition(EmojiBehaviors.XmlNamespace, "iNKORE.UI.WPF.Emojis")]




