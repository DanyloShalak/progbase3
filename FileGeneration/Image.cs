using System;
using ScottPlot;
using RepositoriesAndData;
using System.Drawing;
using System.Linq;

namespace FileGeneration
{
    public static class Image
    {
        public static void GenerateImage(User user, string saveFilePath)
        {
            var plt = new ScottPlot.Plot(600, 400);

            double[] values = GetHistogramInfo(user);
            var hist = new ScottPlot.Statistics.Histogram(values, min: 0, max: values.Max());

            plt.AddBar(hist.countsFrac, hist.bins);
            
            plt.AddScatter(hist.bins, hist.countsFracCurve, Color.Black, lineWidth: 2, markerSize: 0);
            plt.Title(user.fullname);
            plt.YLabel("Frequency (fraction)");
            plt.XLabel("Comments");
            plt.SetAxisLimits(null, null, 0, null);
            plt.Grid(lineStyle: LineStyle.Dot);
            plt.SaveFig(saveFilePath);
        }

        private static double[] GetHistogramInfo(User user)
        {
            double[] histInfo = new double[user.posts.Count + 1];
            Random rand = new Random();
            
            for(int i = 0; i < user.posts.Count; i++)
            {
                histInfo[i + 1] = user.posts[i].comments.Count;
            }
            histInfo[0] = rand.Next((int)histInfo.Min(), (int)histInfo.Max() + 1);
            return histInfo;
        }

        private static double[] Art(double[] fractions, double max)
        {
            int n = (int)Math.Ceiling(1 / max) + 18;
            double[] smArray = new double[fractions.Length];

            for(int i = 0; i < fractions.Length; i++)
            {
                smArray[i] = fractions[i] / n;
            }
            return smArray;
        }
    }
}


