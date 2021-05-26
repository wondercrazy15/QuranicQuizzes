using System;
using Xamarin.Forms;

namespace QuranicQuizzes.CustomControls
{
    /// <summary>
    /// Custom Picker
    /// </summary>
    public class CustomPicker : Picker
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(CustomPicker), string.Empty);
        public string Icon
        {
            get
            {
                return (string)GetValue(ImageProperty);
            }
            set
            {
                SetValue(ImageProperty, value);
            }
        }
    }
}