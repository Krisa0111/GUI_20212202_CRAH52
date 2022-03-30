﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game
{
    public class MainMenuDisplay : FrameworkElement
    {
        public Brush BackGroundBrush
        {
            get
            {
                return new ImageBrush(new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(),("Images"),"background.jpg"))));
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (ActualWidth >0 && ActualHeight >0)
            {
                drawingContext.DrawRectangle(BackGroundBrush, null, new Rect(0, 0, ActualWidth, ActualHeight));
            }
            
        }
    }
}
