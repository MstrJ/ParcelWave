using iText.IO.Image;
using ReportMaker.Services.Interfaces;
using ScottPlot;
using Image = iText.Layout.Element.Image;

namespace ReportMaker.Services;

public class PlotToImage : IPlotToImage
{
    public Image Convert(Plot plot,int width,int height)
    {
        var bmp = plot.GetImageBytes(width,height);
        var chartsData = ImageDataFactory.Create(bmp);
        var chart = new Image(chartsData);
        return chart;
    }
}