using iText.IO.Image;
using Microsoft.AspNetCore.Mvc;
using iText.Layout.Element;
using ReportMaker.Repositories.Interfaces;
using ReportMaker.Services.Interfaces;
using ScottPlot;
using Image = iText.Layout.Element.Image;

namespace ReportMaker.Services;

public class ReportService : IReportService
{
    private IParcelRepository _parcelRepository { get; }
    private IPdfService _pdfService { get;}
    private IPlotService _plotService { get; }
    
    public ReportService(IParcelRepository parcelRepository, IPdfService pdfService,IPlotService plotService)
    {
        _parcelRepository = parcelRepository;
        _pdfService = pdfService;
        _plotService = plotService;

    }
    
    public async Task<IActionResult> GenerateRaport()
    {
        return await _pdfService.Create(async (document) =>
        {
            var parcels = await _parcelRepository.Get();
            foreach (var parcel in parcels)
            {
                document.Add(new AreaBreak()).SetFontSize(26);
                
                document.Add(new Paragraph($"Parcel UPID: {parcel.Identifiers.UPID}"));
                var info = new List();
                info.Add($"Barcode: {parcel.Identifiers?.Barcode}");
                info.Add($"Width: {_dataConverter(parcel.Attributes?.Width)}");
                info.Add($"Weight: {_dataConverter(parcel.Attributes?.Weight)}");
                info.Add($"Length: {_dataConverter(parcel.Attributes?.Length)}");
                info.Add($"Depth: {_dataConverter(parcel.Attributes?.Depth)}");
                info.Add($"Current State: {_dataConverter(parcel.CurrentState?.Facility)}");
                    
                document.Add(info.SetFontSize(20));
            }

            document.Add(new AreaBreak());

            var parcelCount = parcels.ToList().Count;

            var ysDepth = new double[parcelCount];
            var ysWidth = new double[parcelCount];
            var ysLength = new double[parcelCount];
            var ysWeight = new double[parcelCount];
            var xs = new double[parcelCount];

            int i = 0;
            foreach (var parcel in parcels)
            {
                xs[i] = i + 1;
                ysDepth[i] = parcel.Attributes.Depth ?? -1;
                ysWidth[i] = parcel.Attributes.Width ?? -1;
                ysLength[i] = parcel.Attributes.Length ?? -1;
                ysWeight[i] = parcel.Attributes.Weight ?? -1;
                    
                i++;
            }
            
            Plot plot = new();
            var sp1 = plot.Add.Scatter(xs, ysLength,Colors.Blue);
            sp1.LegendText = "Length";
            var sp2 = plot.Add.Scatter(xs, ysDepth,Colors.Yellow);
            sp2.LegendText = "Depth";
            var sp3 = plot.Add.Scatter(xs, ysWeight,Colors.Green);
            sp3.LegendText = "Weight";
            var sp4 = plot.Add.Scatter(xs, ysWidth,Colors.Black);
            sp4.LegendText = "Width";

            plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericFixedInterval();
            plot.ShowLegend();

            var chart1 = _plotService.Convert(plot,410,350);
            document.Add(chart1);
            
            //    
            //
            //
            var yf = new int[parcelCount];
            
            int j = 0;
            foreach (var parcel in parcels)
            {
                yf[j] = parcel.CurrentState.Facility != null ? (int)parcel.CurrentState.Facility : -1;
                j++;
            }
            
            Plot facilityPlot = new();
                
            var fp =facilityPlot.Add.Scatter(xs, yf,Colors.Brown);
            fp.LegendText = "Facility";
                
            facilityPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericFixedInterval();
            facilityPlot.ShowLegend();
                
            var chart2 = _plotService.Convert(facilityPlot, 410, 350);
                
            document.Add(chart2);
            
            //
            //
            //
            
            var piePlotWeight = _plotService.CreateParcelPie(parcels, (parcel, colors, colorI) =>
            {
                var data = parcel.Attributes.Weight;
                var slice = new PieSlice
                {
                    Value = parcel.Attributes.Weight ?? 0, FillColor = colors[colorI],
                    Label = data != null ? $"UPID {parcel.Identifiers.UPID}" : "others..."
                };
                return slice;
            });
            var pieChartWeight = _plotService.Convert(piePlotWeight, 530, 530);
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Parcel Weights"));
            document.Add(pieChartWeight);   
            
            //
            
            var piePlotWidth = _plotService.CreateParcelPie(parcels, (parcel, colors, colorI) =>
            {
                var data = parcel.Attributes.Width;
                var slice = new PieSlice
                {
                    Value = parcel.Attributes.Width ?? 0, FillColor = colors[colorI],
                    Label = data != null ? $"UPID {parcel.Identifiers.UPID}" : "others..."
                };
                return slice;
            });
            var pieChartWidth = _plotService.Convert(piePlotWidth, 530, 530);
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Parcel Widths"));
            document.Add(pieChartWidth);
            
            //
            
            var piePlotLength = _plotService.CreateParcelPie(parcels, (parcel, colors, colorI) =>
            {
                var data = parcel.Attributes.Length;
                var slice = new PieSlice
                {
                    Value =data ?? 0, FillColor = colors[colorI],
                    Label = data != null ? $"UPID {parcel.Identifiers.UPID}" : "others..."
                };
                return slice;
            });
            var pieChartLength = _plotService.Convert(piePlotLength, 530, 530);
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Parcel Lengths"));
            document.Add(pieChartLength);            
            
            //
            
            var piePlotDepth = _plotService.CreateParcelPie(parcels, (parcel, colors, colorI) =>
            {
                var data = parcel.Attributes.Depth;
                var slice = new PieSlice
                {
                    Value = data ?? 0, FillColor = colors[colorI],
                    Label = data != null ? $"UPID {parcel.Identifiers.UPID}" : "others..."
                };
                return slice;
            });
            var pieChartDepth = _plotService.Convert(piePlotDepth, 530, 530);
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Parcel Depths"));
            document.Add(pieChartDepth);

        });
    }
    
    private string _dataConverter(object message)
    {
        return message == null ? "N/A" : message.ToString();
    }
    
}