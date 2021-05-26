using System;
using Xamarin.Forms;
namespace QuranicQuizzes.CustomControls
{
    public class AuthroiseWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
                returnType: typeof(string),
                declaringType: typeof(AuthroiseWebView),
                defaultValue: default(string));
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}