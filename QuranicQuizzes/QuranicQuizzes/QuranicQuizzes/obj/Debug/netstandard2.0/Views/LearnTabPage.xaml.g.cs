//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("QuranicQuizzes.Views.LearnTabPage.xaml", "Views/LearnTabPage.xaml", typeof(global::QuranicQuizzes.Views.LearnTabPage))]

namespace QuranicQuizzes.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views/LearnTabPage.xaml")]
    public partial class LearnTabPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::QuranicQuizzes.CustomControls.AuthroiseWebView LearnWebView;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label ErrorMsg;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(LearnTabPage));
            LearnWebView = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::QuranicQuizzes.CustomControls.AuthroiseWebView>(this, "LearnWebView");
            ErrorMsg = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "ErrorMsg");
        }
    }
}
