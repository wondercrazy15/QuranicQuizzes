//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("QuranicQuizzes.Views.QuizzesByCategoriesPage.xaml", "Views/QuizzesByCategoriesPage.xaml", typeof(global::QuranicQuizzes.Views.QuizzesByCategoriesPage))]

namespace QuranicQuizzes.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views/QuizzesByCategoriesPage.xaml")]
    public partial class QuizzesByCategoriesPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::QuranicQuizzes.CustomControls.CustomPicker allQuizzes;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::XF.Material.Forms.UI.MaterialIconButton btnDropDown;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.ListView EntryList;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(QuizzesByCategoriesPage));
            allQuizzes = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::QuranicQuizzes.CustomControls.CustomPicker>(this, "allQuizzes");
            btnDropDown = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::XF.Material.Forms.UI.MaterialIconButton>(this, "btnDropDown");
            EntryList = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.ListView>(this, "EntryList");
        }
    }
}
