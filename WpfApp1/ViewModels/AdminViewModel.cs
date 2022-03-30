using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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
        private Random rand = new Random();
        private RelayCommand _generatePdf;
        public RelayCommand GeneratePDF => _generatePdf ??
                    (_generatePdf = new RelayCommand(x =>
                    {
                        Report report = new Report(new PdfFormatter());
                        Root.Reports.Page page = new Root.Reports.Page(report);
                        FontDef fd = new FontDef(report, "Helvetica");
                        FontProp fp = new FontPropMM(fd, 3);
                        double rX = 20;
                        double rY = 20;
                        int AllSum = 0;
                        foreach (Order? item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString(item.Time)));
                            rY += 6;
                        }
                        rY = 20;
                        rX = rX + 60;
                        foreach (Order? item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString($"{item.Sum} $")));
                            AllSum += item.Sum;
                            rY += 6;
                        }
                        rY = 20;
                        rX = rX + 60;
                        foreach (Order? item in _infoFromDb)
                        {
                            page.AddMM(rX, rY, new RepString(fp, Convert.ToString(item.IdClientNavigation.Login)));
                            rY += 6;
                        }
                        rX = 20;
                        rY = rY + 5;
                        page.AddMM(rX, rY, new RepString(fp, Convert.ToString($"{AllSum} $")));

                        RT.ViewPDF(report, "Report.pdf");
                    }));
        private RelayCommand _generateExcel;
        public RelayCommand GenerateExcel => _generateExcel ??
                    (_generateExcel = new RelayCommand(x =>
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            string name = "Report" + rand.Next().ToString();
                            var worksheet = workbook.Worksheets.Add("Report sheet");
                            worksheet.Column("B").CellsUsed().SetDataType(XLDataType.DateTime);
                            worksheet.Column("C").CellsUsed().SetDataType(XLDataType.Number);
                            worksheet.Column("D").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            worksheet.Column("B").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            
                            int x1 = 2;
                            int y1 = 2;
                            int AllSum = 0;
                            worksheet.Cell(1, 2).Value = Convert.ToString("Время заказа");
                            worksheet.Cell(1, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                            worksheet.Cell(1, 3).Value = Convert.ToString("Сумма заказа");
                            worksheet.Cell(1, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                            worksheet.Cell(1, 4).Value = Convert.ToString("Официант");
                            worksheet.Cell(1, 4).Style.Border.BottomBorder = XLBorderStyleValues.Thick;

                            foreach (Order item in _infoFromDb)
                            {
                                worksheet.Cell(y1, x1).Value = Convert.ToString(item.Time);
                                y1 += 1;
                            }
                            y1 = 2;
                            x1 = 3;
                            foreach (Order item in _infoFromDb)
                            {
                                worksheet.Cell(y1, x1).Value = Convert.ToString(item.Sum);
                                if (item.Sum < 500)
                                {
                                    worksheet.Cell(y1, x1).Style.Fill.BackgroundColor = XLColor.FromHtml("#fff85f");
                                    worksheet.Cell(y1, x1).Style.Border.LeftBorder = XLBorderStyleValues.Thick;

                                }
                                else
                                {
                                    worksheet.Cell(y1, x1).Style.Fill.BackgroundColor = XLColor.FromHtml("#9dff97");
                                    worksheet.Cell(y1, x1).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                }
                                AllSum += item.Sum;
                                y1 += 1;
                            }
                            y1 = 2;
                            x1 = 4;
                            foreach (Order item in _infoFromDb)
                            {
                                worksheet.Cell(y1, x1).Value = Convert.ToString(item.IdClientNavigation.Login);
                                worksheet.Cell(y1, x1).Style.Border.LeftBorder = XLBorderStyleValues.Thick;

                                y1 += 1;
                            }
                            y1 = y1 + 1;
                            worksheet.Cell(y1, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#c7ffe7");
                            worksheet.Cell(y1, 2).Value = Convert.ToString(AllSum);
                            worksheet.Column("B").AdjustToContents();
                            worksheet.Column("C").AdjustToContents();
                            worksheet.Column("D").AdjustToContents();
                            workbook.SaveAs($"Reports/{name}.xlsx");
                        }
                    }));

    }
}
