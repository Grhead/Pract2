using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using Root.Reports;

namespace WpfApp1.ViewModels
{
    public class AdminViewModel : StaticViewModel
    {
        private ObservableCollection<Order> _infoFromDb = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 4).Include(x => x.IdClientNavigation));

        private RelayCommand _generatePdf;
        public RelayCommand GeneratePDF
        {
            get
            {
                return _generatePdf ??
                    (_generatePdf = new RelayCommand(x =>
                    {
                        Report report = new Report(new PdfFormatter());
                        Page page = new Page(report);
                        FontDef fd = new FontDef(report, "Helvetica");
                        FontProp fp = new FontPropMM(fd, 4);
                        Double rX = 20;
                        Double rY = 20;
                        int AllSum = 0;
                        foreach (var item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString(item.Time)));
                            rY += 10;
                        }
                        rY = 20;
                        rX = rX + 60;
                        foreach (var item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString($"{item.Sum} $")));
                            AllSum += item.Sum;
                            rY += 10;
                        }
                        rY = 20;
                        rX = rX + 60;
                        foreach (var item in _infoFromDb)
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

    }
}
