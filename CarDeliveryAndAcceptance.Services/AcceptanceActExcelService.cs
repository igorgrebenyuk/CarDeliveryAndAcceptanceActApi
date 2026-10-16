using CarDeliveryAndAcceptance.Services.Contracts;
using CarDeliveryAndAcceptance.Services.Contracts.Models.AcceptanceAct;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CarDeliveryAndAcceptance.Services;

/// <summary>
/// Сервис генерации Excel-файлов по актам приёма-передачи
/// </summary>
public class AcceptanceActExcelService : IAcceptanceActExcelService
{
    private const uint DefaultCellFormat = 0;
    private const uint HeaderCellFormat = 1;
    private const uint BorderedCellFormat = 2;
    private const uint HeaderBorderedCellFormat = 3;

    /// <summary>
    /// Генерирует Excel-файл по акту
    /// </summary>
    public Task<Stream> GenerateAcceptanceActExcelAsync(AcceptanceActModel act, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(act);

        var stream = new MemoryStream();

        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = CreateWorkbook(document);
            var (worksheetPart, sheetData) = CreateWorksheetWithData(workbookPart);

            AddActInfo(sheetData, act);
            AddPersonInfo(sheetData, "Продавец", act.Salesman.Name, act.Salesman.FirstName, act.Salesman.Surname);
            AddPersonInfo(sheetData, "Покупатель", act.Buyer.Name, act.Buyer.FirstName, act.Buyer.Surname);
            AddCarInfo(sheetData, act);

            workbookPart.Workbook!.Save();
            worksheetPart.Worksheet.Save();
        }

        stream.Position = 0;
        return Task.FromResult<Stream>(stream);
    }

    private WorkbookPart CreateWorkbook(SpreadsheetDocument document)
    {
        var workbookPart = document.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
        stylesPart.Stylesheet = CreateStylesheet();
        stylesPart.Stylesheet.Save();

        return workbookPart;
    }

    private (WorksheetPart worksheetPart, SheetData sheetData) CreateWorksheetWithData(WorkbookPart workbookPart)
    {
        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        var sheetData = new SheetData();
        var columns = new Columns(
            new Column { Min = 1, Max = 1, Width = 35, CustomWidth = true },
            new Column { Min = 2, Max = 2, Width = 45, CustomWidth = true });
        worksheetPart.Worksheet = new Worksheet(columns, sheetData);

        var sheets = workbookPart.Workbook!.AppendChild(new Sheets());
        sheets.Append(new Sheet
        {
            Id = workbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = "Акт"
        });

        return (worksheetPart, sheetData);
    }

    private void AddActInfo(SheetData sheetData, AcceptanceActModel act)
    {
        AddTitleRow(sheetData, "Акт приёма-передачи автомобиля");
        AddPropertyRow(sheetData, "Номер", act.Id.ToString());
        AddPropertyRow(sheetData, "Город", act.City.Name);
        AddPropertyRow(sheetData, "Дата составления", act.DateOfCreation.ToString("dd.MM.yyyy HH:mm"));
        AddEmptyRow(sheetData);
    }

    private void AddPersonInfo(SheetData sheetData, string role, string name, string firstName, string surname)
    {
        AddTitleRow(sheetData, role);
        AddPropertyRow(sheetData, "Наименование", name);
        AddPropertyRow(sheetData, "Имя", firstName);
        AddPropertyRow(sheetData, "Фамилия", surname);
        AddEmptyRow(sheetData);
    }

    private void AddCarInfo(SheetData sheetData, AcceptanceActModel act)
    {
        var car = act.Car;

        AddTitleRow(sheetData, "Автомобиль");
        AddRow(sheetData, HeaderBorderedCellFormat, "Параметр", "Значение");
        AddBorderedRow(sheetData, "Марка", car.Mark);
        AddBorderedRow(sheetData, "Модель", car.Model);
        AddBorderedRow(sheetData, "Регистрационный знак", car.LicensePlate);
        AddBorderedRow(sheetData, "VIN", car.VinCode);
        AddBorderedRow(sheetData, "Год выпуска", car.ManufactureYear.ToString());
        AddBorderedRow(sheetData, "Двигатель №", car.EngineNumber);
        AddBorderedRow(sheetData, "Шасси (рама)", car.ChassisNumber);
        AddBorderedRow(sheetData, "Кузов (коляска)", car.BodyNumber);
        AddBorderedRow(sheetData, "Цвет", car.Color);
    }

    private void AddPropertyRow(SheetData sheetData, string propertyName, string value)
    {
        AddRow(sheetData, DefaultCellFormat, propertyName, value);
    }

    private void AddBorderedRow(SheetData sheetData, string propertyName, string value)
    {
        AddRow(sheetData, BorderedCellFormat, propertyName, value);
    }

    private void AddTitleRow(SheetData sheetData, string title)
    {
        AddRow(sheetData, HeaderCellFormat, title);
    }

    private void AddEmptyRow(SheetData sheetData)
    {
        sheetData.AppendChild(new Row());
    }

    private void AddRow(SheetData sheetData, uint styleIndex, params string[] values)
    {
        var row = new Row();
        foreach (var value in values)
        {
            row.AppendChild(new Cell
            {
                StyleIndex = styleIndex,
                DataType = CellValues.String,
                CellValue = new CellValue(value ?? string.Empty)
            });
        }

        sheetData.AppendChild(row);
    }

    private Stylesheet CreateStylesheet()
    {
        var fonts = new Fonts(
            new Font(new FontName { Val = "Calibri" }, new FontSize { Val = 11 }),
            new Font(new Bold(), new FontName { Val = "Calibri" }, new FontSize { Val = 11 }));

        var fills = new Fills(
            new Fill(new PatternFill { PatternType = PatternValues.None }),
            new Fill(new PatternFill { PatternType = PatternValues.Gray125 }));

        var borders = new Borders(
            new Border(),
            new Border(
                new LeftBorder { Style = BorderStyleValues.Thin },
                new RightBorder { Style = BorderStyleValues.Thin },
                new TopBorder { Style = BorderStyleValues.Thin },
                new BottomBorder { Style = BorderStyleValues.Thin }));

        var cellFormats = new CellFormats(
            new CellFormat { FontId = 0, FillId = 0, BorderId = 0 },
            new CellFormat { FontId = 1, FillId = 0, BorderId = 0 },
            new CellFormat { FontId = 0, FillId = 0, BorderId = 1 },
            new CellFormat { FontId = 1, FillId = 0, BorderId = 1 });

        return new Stylesheet(fonts, fills, borders, cellFormats);
    }
}