﻿using System.Windows;
using System.Windows.Media;

namespace Comm.WPF.Common;

public static class BrushHelper
{
    private static readonly Dictionary<string, SolidColorBrush> BrushDict = new();

    public static SolidColorBrush Parse(string hexColor)
    {
        if (BrushDict.TryGetValue(hexColor, out var value))
        {
            return value;
        }

        // 验证格式是否正确
        if (hexColor == null || hexColor.Length != 9 && hexColor.Length != 7)
            throw new ArgumentException("Invalid hexadecimal color format", nameof(hexColor));
        // 如果没有包含透明度，默认为 FF (完全不透明)
        if (hexColor.Length == 7)
            hexColor = "#FF" + hexColor.Substring(1);
        byte a, r, g, b;
        if (!byte.TryParse(hexColor.Substring(1, 2), System.Globalization.NumberStyles.HexNumber, null, out a) ||
            !byte.TryParse(hexColor.Substring(3, 2), System.Globalization.NumberStyles.HexNumber, null, out r) ||
            !byte.TryParse(hexColor.Substring(5, 2), System.Globalization.NumberStyles.HexNumber, null, out g) ||
            !byte.TryParse(hexColor.Substring(7, 2), System.Globalization.NumberStyles.HexNumber, null, out b))
        {
            throw new ArgumentException("Invalid hexadecimal color format", nameof(hexColor));
        }

        Color color = Color.FromArgb(a, r, g, b);
        var brush = Application.Current.Dispatcher.Invoke(() => new SolidColorBrush(color));
        //var brush = new SolidColorBrush(color);
        BrushDict[hexColor] = brush;
        return brush;
    }
}