using System;
using System.ComponentModel;
using CoreGraphics;
using QuranicQuizzes.CustomControls;
using QuranicQuizzes.iOS.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using SizeF = CoreGraphics.CGSize;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]
namespace QuranicQuizzes.iOS.Helpers
{
    public class CustomProgressBarRenderer : ViewRenderer<ProgressBar, UIProgressView>
    {
        [Xamarin.Forms.Internals.Preserve(Conditional = true)]
        public CustomProgressBarRenderer()
        {

        }

        public override SizeF SizeThatFits(SizeF size)
        {
            // progress bar will size itself to be as wide as the request, even if its inifinite
            // we want the minimum need size
            var result = base.SizeThatFits(size);
            return new SizeF(20, 20);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                    SetNativeControl(new UIProgressView(UIProgressViewStyle.Default));

                //Control.Transform = Transform;
                UpdateProgressColor();
                UpdateProgress();
            }

            base.OnElementChanged(e);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            CGAffineTransform transform = CGAffineTransform.MakeScale(1.0f,10.0f);
            transform.TransformSize(this.Frame.Size);
            this.Transform = transform;

            //this.Control.Transform = Transform;
          
            // CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            // this.Transform = transform;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
           // Control.Transform = Transform;
            if (e.PropertyName == ProgressBar.ProgressColorProperty.PropertyName)
                UpdateProgressColor();
            else if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName)
                UpdateProgress();
        }

        protected override void SetBackgroundColor(Color color)
        {
            base.SetBackgroundColor(color);

            if (Control == null)
                return;

            Control.TrackTintColor = color != Color.Default ? color.ToUIColor() : null;
        }

        void UpdateProgressColor()
        {
            Control.ProgressTintColor = Element.ProgressColor == Color.Default ? null : Element.ProgressColor.ToUIColor();
        }

        void UpdateProgress()
        {
            Control.Progress = (float)Element.Progress;
        }
    }
}