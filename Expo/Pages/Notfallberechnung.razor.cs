﻿using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace Expo.Pages
{
    public partial class Notfallberechnung : ComponentBase
    {
        private string currentChartPath;

        protected override async Task OnInitializedAsync()
        {
            string sub = Path.Combine(AppContext.BaseDirectory, "wwwroot", "chart");
            if (Directory.Exists(sub))
            {
                var fileName = Path.Combine(sub, "Mode-5000.bmp");
                if (File.Exists(fileName))
                    currentChartPath = "./chart/Mode-5000.bmp";
            }
        }
        
        public async void CreateChartAsync()
        {
            var n = new Bitmap(314, 314);
            using (var g = Graphics.FromImage(n))
            {
                g.DrawString("hallo test", new System.Drawing.Font("Tahoma", 8), new SolidBrush(System.Drawing.Color.Red), 200, 200);
            }
            int length = AppDomain.CurrentDomain.BaseDirectory.LastIndexOf(AppDomain.CurrentDomain.FriendlyName) + AppDomain.CurrentDomain.FriendlyName.Length + 1;
            //string sub = AppDomain.CurrentDomain.BaseDirectory.Substring(0, length);
            string sub = Path.Combine(AppContext.BaseDirectory, "wwwroot", "chart");
            if (!Directory.Exists(sub))
            {
                Directory.CreateDirectory(sub);
            }
            var fileName = Path.Combine(sub, "Mode-5000.bmp");
            n.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            currentChartPath = "./chart/Mode-5000.bmp";
            StateHasChanged();
        }
    }
}
