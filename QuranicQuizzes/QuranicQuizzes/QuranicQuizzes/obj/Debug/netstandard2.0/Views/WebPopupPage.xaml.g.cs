//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("QuranicQuizzes.Views.WebPopupPage.xaml", "Views/WebPopupPage.xaml", typeof(global::QuranicQuizzes.Views.WebPopupPage))]

namespace QuranicQuizzes.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views/WebPopupPage.xaml")]
    public partial class WebPopupPage : global::Rg.Plugins.Popup.Pages.PopupPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::XF.Material.Forms.UI.MaterialCard FrameContainer;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label header;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::XF.Material.Forms.UI.MaterialIconButton btnClose;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::QuranicQuizzes.CustomControls.AuthroiseWebView webNote;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(WebPopupPage));
            FrameContainer = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::XF.Material.Forms.UI.MaterialCard>(this, "FrameContainer");
            header = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "header");
            btnClose = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::XF.Material.Forms.UI.MaterialIconButton>(this, "btnClose");
            webNote = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::QuranicQuizzes.CustomControls.AuthroiseWebView>(this, "webNote");
        }
    }
}
