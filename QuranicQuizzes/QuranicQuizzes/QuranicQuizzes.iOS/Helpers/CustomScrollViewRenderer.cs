using System;
using QuranicQuizzes.CustomControls;
using QuranicQuizzes.iOS.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomScrollView), typeof(CustomScrollViewRenderer))]
namespace QuranicQuizzes.iOS.Helpers
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            UpdateScrollView();
        }

        private void UpdateScrollView()
        {
            ContentInset = new UIKit.UIEdgeInsets(0, 0, 0, 0);
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                ContentInsetAdjustmentBehavior = UIKit.UIScrollViewContentInsetAdjustmentBehavior.Never;
            Bounces = false;
            ScrollIndicatorInsets = new UIKit.UIEdgeInsets(0, 0, 0, 0);
        }
    }
}