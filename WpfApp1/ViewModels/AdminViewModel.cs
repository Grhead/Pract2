using Microsoft.EntityFrameworkCore;
using Root.Reports;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp1.ViewModels
{
    public class AdminViewModel : StaticViewModel
    {
        private readonly ObservableCollection<Order> _infoFromDb = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 4).Include(x => x.IdClientNavigation));

        private RelayCommand _generatePdf;
        public RelayCommand GeneratePDF => _generatePdf ??
                    (_generatePdf = new RelayCommand(x =>
                    {
                        Report report = new Report(new PdfFormatter());
                        Page page = new Page(report);
                        FontDef fd = new FontDef(report, "Helvetica");
                        FontProp fp = new FontPropMM(fd, 4);
                        double rX = 20;
                        double rY = 20;
                        int AllSum = 0;
                        foreach (Order? item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString(item.Time)));
                            rY += 10;
                        }
                        rY = 20;
                        rX = rX + 60;
                        foreach (Order? item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString($"{item.Sum} $")));
                            AllSum += item.Sum;
                            rY += 10;
                        }
                        rY = 20;
                        rX = rX + 60;
                        foreach (Order? item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString(item.IdClientNavigation.Login)));
                            rY += 10;
                        }
                        rX = 20;
                        rY = rY + 10;
                        page.AddMM(rX, rY, new RepString(fp, Convert.ToString($"{AllSum} $")));

                        RT.ViewPDF(report, "Report.pdf");
                    }));

    }
}
