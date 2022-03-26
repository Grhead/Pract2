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
        private ObservableCollection<Order> getRep = new ObservableCollection<Order>(Service.db.Orders.Include(q => q.StatusNavigation).Where(x => x.StatusNavigation.Id == 4));

        private RelayCommand generatePDF;
        public RelayCommand GeneratePDF
        {
            get
            {
                return generatePDF ??
                    (generatePDF = new RelayCommand(x =>
                    {
                        Report report = new Report(new PdfFormatter());
                        Page page = new Page(report);
                        FontDef fd = new FontDef(report, "Helvetica");
                        FontProp fp = new FontPropMM(fd, 4);
                        Double rX = 20;
                        Double rY = 20;
                        foreach (var item in getRep)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString(item.Time)));
                            rY += 10;
                        }
                        rY = 20;
                        rX = rX + 60;
                        foreach (var item in getRep)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString(item.StatusNavigation.Title)));
                            rY += 10;
                        }
                        RT.ViewPDF(report, "Report.pdf");
                    }));
            }
        }

    }
}
