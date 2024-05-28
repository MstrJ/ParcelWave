using iText.IO.Image;
using ParcelProcessor.Models;
using ReportMaker.Services.Interfaces;
using ScottPlot;
using Image = iText.Layout.Element.Image;

namespace ReportMaker.Services;

public class PlotService : IPlotService
{
    public Image Convert(Plot plot,int width,int height)
    {
        var bmp = plot.GetImageBytes(width,height);
        var chartsData = ImageDataFactory.Create(bmp);
        var chart = new Image(chartsData);
        return chart;
    }
    public Plot CreateParcelPie(IEnumerable<ParcelEntity> parcels,Func<ParcelEntity,Color[],int,PieSlice> pieSlice)
    {
        var parcelCount = parcels.ToList().Count;
        Plot plot = new();
            
        List<PieSlice> slices = new();
        var c = Colors.RandomHue(parcelCount);
        int colorI = 0;
        foreach (var parcel in parcels)
        {
            var slice = pieSlice(parcel,c,colorI);
            slices.Add(slice);
            colorI++;
        }

        var pie = plot.Add.Pie(slices);
        pie.ExplodeFraction = .05;
        pie.ShowSliceLabels = true;
        pie.SliceLabelDistance = 1.3;

        return plot;
    }
}