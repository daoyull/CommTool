using System.Windows;
using System.Windows.Media;

namespace Comm.WPF.Common;

public static class VisualTreeHelperExtensions
{
    /// <summary>
    /// 查找视觉树中的所有指定类型的子元素。
    /// </summary>
    public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
    {
        if (depObj == null) yield break;

        int childrenCount = VisualTreeHelper.GetChildrenCount(depObj);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);

            if (child is T t)
                yield return t;

            foreach (T childOfChild in child.FindVisualChildren<T>())
                yield return childOfChild;
        }
    }

    /// <summary>
    /// 查找视觉树中的第一个指定类型的子元素。
    /// </summary>
    public static T FindVisualChild<T>(this DependencyObject depObj) where T : DependencyObject
    {
        if (depObj == null) return null;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);

            if (child is T t)
                return t;

            var childOfChild = child.FindVisualChild<T>();
            if (childOfChild != null)
                return childOfChild;
        }

        return null;
    }
}