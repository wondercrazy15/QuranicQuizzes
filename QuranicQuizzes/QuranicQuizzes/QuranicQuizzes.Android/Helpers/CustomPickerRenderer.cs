using System;
using QuranicQuizzes.CustomControls;
using QuranicQuizzes.Droid.Helpers;
using Xamarin.Forms;
using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace QuranicQuizzes.Droid.Helpers
{
    [Obsolete]
    public class CustomPickerRenderer : PickerRenderer
    {
        CustomPicker element;
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            var picker = e.NewElement;
            CustomPicker bp = (CustomPicker)this.Element;

            if (Control != null)
            {
                Control.SetSingleLine(true);
                // Remove borders
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.LightGray);
                Control.SetBackgroundDrawable(gd);
            }
        }
        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.Gray;
            border.SetPadding(0, 0, 0, 0);
            border.Paint.SetStyle(Paint.Style.Stroke);
            Drawable[] layers = {
                            border,
                            GetDrawable(imagePath)
                        };
           LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            //SetLayerInset(0, 0, 0, 0, 0);  
                        return layerDrawable;  
        }
        private BitmapDrawable GetDrawable(string imagePath)
        {
            var drawable = Resources.GetDrawable(imagePath);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;
            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 70, 70, true));
            result.Gravity = Android.Views.GravityFlags.Right;
            return result;
        }
    }
}