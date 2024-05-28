using ScottPlot;
using Image = iText.Layout.Element.Image;
    
namespace ReportMaker.Services.Interfaces;

public interface IPlotService
{
    public Image Convert(Plot plot, int width, int height);
}