using System;
using System.Collections.Generic;
using CoreGraphics;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.iOS.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace QuranicQuizzes.iOS.Helpers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    this.Control.LeftView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                    this.Control.RightView = new UIView(new CGRect(0, 0, 8, this.Control.Frame.Height));
                    this.Control.LeftViewMode = UITextFieldViewMode.Always;
                    this.Control.RightViewMode = UITextFieldViewMode.Always;

                    this.Control.BorderStyle = UITextBorderStyle.None;
                    this.Element.HeightRequest = 30;
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }

        }

    }
}