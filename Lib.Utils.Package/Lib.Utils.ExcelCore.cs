namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using OfficeOpenXml;
    using OfficeOpenXml.Drawing;
    using OfficeOpenXml.Style;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    #endregion Using System Library (.NET Framework v4.5.2)

    public sealed class ExcelCore
    {
        #region Export methods

        public static bool ExportExample(DataTable dt, FileInfo printTemplate, string printFromCell, string fromDate, string toDate, string filePath)
        {
            try
            {
                using (ExcelPackage pkg = new ExcelPackage(printTemplate))
                {
                    ExcelWorksheet ws = pkg.Workbook.Worksheets[dt.TableName];

                    ws.Cells[printFromCell].LoadFromDataTable(dt, false);

                    if (ws.Dimension.NotNull())
                    {
                        int StartRow = ws.Dimension.Start.Row;
                        int StartColumn = ws.Dimension.Start.Column;
                        int EndRow = ws.Dimension.End.Row;
                        int EndColumn = ws.Dimension.End.Column;
                        int PrintFromRow = printFromCell.TakeNumsPart().ToInt(StartRow).Value;

                        for (int i = StartRow; i <= EndRow; i++)
                        {
                            // Set header rows
                            if (i < PrintFromRow)
                            {
                                for (int j = StartColumn; j <= EndColumn; j++)
                                {
                                    if (ws.Cells[i, j].Value.ToTrim().Contains("{0}"))
                                    {
                                        ws.Cells[i, j].Value = ws.Cells[i, j].Value.ToTrim().Replace("{0}", fromDate);
                                    }
                                    if (ws.Cells[i, j].Value.ToTrim().Contains("{1}"))
                                    {
                                        ws.Cells[i, j].Value = ws.Cells[i, j].Value.ToTrim().Replace("{1}", toDate);
                                    }
                                }
                            }

                            // Set special rows
                            if (i >= PrintFromRow)
                            {
                                if (ws.Cells[i, StartColumn].Value.ToTrim().Length == 0)
                                {
                                    for (int j = StartColumn + 1; j <= EndColumn; j++)
                                    {
                                        SetCellFontType(ws, i, j, "B");
                                        ws.Cells[i, j].Style.Font.Color.SetColor(Color.FromName("Blue"));
                                    }
                                }
                            }
                        }

                        // Set excel border
                        SetBorder(ws, StartRow, StartColumn, EndRow, EndColumn, "A");
                    }

                    // Save file
                    pkg.SaveAs(new FileInfo(filePath));
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ExportDataSet(DataSet ds, string filePath)
        {
            if (ds.NotNull() && ds.Tables.Count > 0)
            {
                try
                {
                    using (ExcelPackage pkg = new ExcelPackage())
                    {
                        int i = 1;
                        foreach (DataTable dt in ds.Tables)
                        {
                            string sheetName = dt.TableName.IsNull() ? ("Sheet" + i) : dt.TableName;
                            ExcelWorksheet ws = pkg.Workbook.Worksheets.Add(sheetName);
                            ws.Cells.Style.Font.Name = Constants.DefaultFontName;
                            ws.Cells.Style.Font.Size = Constants.DefaultFontSize;
                            ws.Cells[Constants.DefaultPrintFromCell].LoadFromDataTable(dt, true);
                            if (ws.Dimension.NotNull())
                            {
                                int StartRow = ws.Dimension.Start.Row;
                                int StartColumn = ws.Dimension.Start.Column;
                                int EndRow = ws.Dimension.End.Row;
                                int EndColumn = ws.Dimension.End.Column;
                                for (int j = StartColumn; j <= EndColumn; j++)
                                    ws.Column(j).AutoFit();
                            }
                            i++;
                        }
                        pkg.SaveAs(new FileInfo(filePath));
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static bool Export(DataTable dt, string filePath)
        {
            if (dt.NotNull() && dt.Rows.Count > 0)
            {
                try
                {
                    using (ExcelPackage pkg = new ExcelPackage())
                    {
                        string sheetName = dt.TableName.IsNull() ? Constants.DefaultSheetName : dt.TableName;
                        ExcelWorksheet ws = pkg.Workbook.Worksheets.Add(sheetName);
                        ws.Cells.Style.Font.Name = Constants.DefaultFontName;
                        ws.Cells.Style.Font.Size = Constants.DefaultFontSize;
                        ws.Cells[Constants.DefaultPrintFromCell].LoadFromDataTable(dt, true);
                        if (ws.Dimension.NotNull())
                        {
                            int StartRow = ws.Dimension.Start.Row;
                            int StartColumn = ws.Dimension.Start.Column;
                            int EndRow = ws.Dimension.End.Row;
                            int EndColumn = ws.Dimension.End.Column;
                            for (int j = StartColumn; j <= EndColumn; j++)
                                ws.Column(j).AutoFit();
                        }
                        pkg.SaveAs(new FileInfo(filePath));
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }


        public static DataTable ReadExcel2DataTable(Stream stream, int index = 1)
        {
            try
            {
                if (stream.NotNull())
                {
                    using (ExcelPackage pkg = new ExcelPackage(stream))
                    {
                        DataRow dr = null;
                        object cellObj = null;
                        string cellVal = null;
                        string cellText = null;

                        var colsName = new List<string>();
                        var dataTable = new DataTable(pkg.Workbook.Worksheets[index].Name);
                        int totalRows = pkg.Workbook.Worksheets[index].Dimension.End.Row;
                        int totalCols = pkg.Workbook.Worksheets[index].Dimension.End.Column;

                        for (int i = 1; i <= totalRows; i++)
                        {
                            if (i > 1) dr = dataTable.Rows.Add();
                            for (int j = 1; j <= totalCols; j++)
                            {
                                if (i == 1)
                                {
                                    cellObj = pkg.Workbook.Worksheets[index].Cells[i, j].Value;
                                    cellVal = cellObj.IsNull() ? "" : cellObj.ToString().Trim();
                                    dataTable.Columns.Add(cellVal);
                                    colsName.Add(cellVal);
                                }
                                else
                                {
                                    DateTime result;
                                    cellObj = pkg.Workbook.Worksheets[index].Cells[i, j].Value;
                                    cellVal = cellObj.IsNull() ? "" : cellObj.ToString().Trim();
                                    cellText = pkg.Workbook.Worksheets[index].Cells[i, j].Text.Trim();
                                    if (cellVal.IsNumeric())
                                    {
                                        if (IsDateTime(cellText, Constants.PopularDateFormats, out result)) // Date
                                        {
                                            dr[j - 1] = result.ToString(Constants.SqlDateFormat);
                                        }
                                        else if (IsDateTime(cellText, Constants.PopularDateTimeFormats, out result)) // DateTime
                                        {
                                            if (Constants.ExcelDefaultDateForTime.Contains(cellText.Left(10))) // Time
                                            {
                                                dr[j - 1] = result.ToString(Constants.Time24Format);
                                            }
                                            else
                                            {
                                                dr[j - 1] = result.ToString(Constants.SqlDateTimeFormat);
                                            }
                                        }
                                        else
                                        {
                                            dr[j - 1] = cellVal;
                                        }
                                    }
                                    else if (IsDateTime(cellVal, Constants.PopularDateFormats, out result)) // Date
                                    {
                                        dr[j - 1] = result.ToString(Constants.SqlDateFormat);
                                    }
                                    else if (IsDateTime(cellVal, Constants.PopularDateTimeFormats, out result)) // DateTime
                                    {
                                        if (Constants.ExcelDefaultDateForTime.Contains(cellVal.Left(10))) // Time
                                        {
                                            dr[j - 1] = result.ToString(Constants.Time24Format);
                                        }
                                        else
                                        {
                                            dr[j - 1] = result.ToString(Constants.SqlDateTimeFormat);
                                        }
                                    }
                                    else
                                    {
                                        dr[j - 1] = cellVal;
                                    }
                                }
                            }
                        }

                        //outcols = colsName;
                        //outmsg = null;
                        return dataTable;
                    }
                }
                else
                {
                    //outcols = new List<string>();
                    //outmsg = "The input data specified was not found in the input file.";
                }
            }
            catch (Exception ex)
            {
                //outcols = new List<string>();
                //outmsg = ex.Message;
            }

            return new DataTable();
        }


        #endregion Export methods

        #region Import methods

        public static DataTable ReadExcel2DataTable(string path, out List<string> outcols, out string outmsg, int index = 1)
        {
            try
            {
                var newFile = new FileInfo(path);

                if (newFile.Exists)
                {
                    using (ExcelPackage pkg = new ExcelPackage(newFile))
                    {
                        DataRow dr = null;
                        object cellObj = null;
                        string cellVal = null;
                        string cellText = null;

                        var colsName = new List<string>();
                        var dataTable = new DataTable(pkg.Workbook.Worksheets[index].Name);
                        int totalRows = pkg.Workbook.Worksheets[index].Dimension.End.Row;
                        int totalCols = pkg.Workbook.Worksheets[index].Dimension.End.Column;

                        for (int i = 1; i <= totalRows; i++)
                        {
                            if (i > 1) dr = dataTable.Rows.Add();
                            for (int j = 1; j <= totalCols; j++)
                            {
                                if (i == 1)
                                {
                                    cellObj = pkg.Workbook.Worksheets[index].Cells[i, j].Value;
                                    cellVal = cellObj.IsNull() ? "" : cellObj.ToString().Trim();
                                    dataTable.Columns.Add(cellVal);
                                    colsName.Add(cellVal);
                                }
                                else
                                {
                                    DateTime result;
                                    cellObj = pkg.Workbook.Worksheets[index].Cells[i, j].Value;
                                    cellVal = cellObj.IsNull() ? "" : cellObj.ToString().Trim();
                                    cellText = pkg.Workbook.Worksheets[index].Cells[i, j].Text.Trim();
                                    if (cellVal.IsNumeric())
                                    {
                                        if (IsDateTime(cellText, Constants.PopularDateFormats, out result)) // Date
                                        {
                                            dr[j - 1] = result.ToString(Constants.SqlDateFormat);
                                        }
                                        else if (IsDateTime(cellText, Constants.PopularDateTimeFormats, out result)) // DateTime
                                        {
                                            if (Constants.ExcelDefaultDateForTime.Contains(cellText.Left(10))) // Time
                                            {
                                                dr[j - 1] = result.ToString(Constants.Time24Format);
                                            }
                                            else
                                            {
                                                dr[j - 1] = result.ToString(Constants.SqlDateTimeFormat);
                                            }
                                        }
                                        else
                                        {
                                            dr[j - 1] = cellVal;
                                        }
                                    }
                                    else if (IsDateTime(cellVal, Constants.PopularDateFormats, out result)) // Date
                                    {
                                        dr[j - 1] = result.ToString(Constants.SqlDateFormat);
                                    }
                                    else if (IsDateTime(cellVal, Constants.PopularDateTimeFormats, out result)) // DateTime
                                    {
                                        if (Constants.ExcelDefaultDateForTime.Contains(cellVal.Left(10))) // Time
                                        {
                                            dr[j - 1] = result.ToString(Constants.Time24Format);
                                        }
                                        else
                                        {
                                            dr[j - 1] = result.ToString(Constants.SqlDateTimeFormat);
                                        }
                                    }
                                    else
                                    {
                                        dr[j - 1] = cellVal;
                                    }
                                }
                            }
                        }

                        outcols = colsName;
                        outmsg = null;
                        return dataTable;
                    }
                }
                else
                {
                    outcols = new List<string>();
                    outmsg = "The input data specified was not found in the input file.";
                }
            }
            catch (Exception ex)
            {
                outcols = new List<string>();
                outmsg = ex.Message;
            }

            return new DataTable();
        }

        public static DataTable ReadExcel2DataTable(Stream stream, out List<string> outcols, out string outmsg, int index = 1)
        {
            try
            {
                if (stream.NotNull())
                {
                    using (ExcelPackage pkg = new ExcelPackage(stream))
                    {
                        DataRow dr = null;
                        object cellObj = null;
                        string cellVal = null;
                        string cellText = null;

                        var colsName = new List<string>();
                        var dataTable = new DataTable(pkg.Workbook.Worksheets[index].Name);
                        int totalRows = pkg.Workbook.Worksheets[index].Dimension.End.Row;
                        int totalCols = pkg.Workbook.Worksheets[index].Dimension.End.Column;

                        for (int i = 1; i <= totalRows; i++)
                        {
                            if (i > 1) dr = dataTable.Rows.Add();
                            for (int j = 1; j <= totalCols; j++)
                            {
                                if (i == 1)
                                {
                                    cellObj = pkg.Workbook.Worksheets[index].Cells[i, j].Value;
                                    cellVal = cellObj.IsNull() ? "" : cellObj.ToString().Trim();
                                    dataTable.Columns.Add(cellVal);
                                    colsName.Add(cellVal);
                                }
                                else
                                {
                                    DateTime result;
                                    cellObj = pkg.Workbook.Worksheets[index].Cells[i, j].Value;
                                    cellVal = cellObj.IsNull() ? "" : cellObj.ToString().Trim();
                                    cellText = pkg.Workbook.Worksheets[index].Cells[i, j].Text.Trim();
                                    if (cellVal.IsNumeric())
                                    {
                                        if (IsDateTime(cellText, Constants.PopularDateFormats, out result)) // Date
                                        {
                                            dr[j - 1] = result.ToString(Constants.SqlDateFormat);
                                        }
                                        else if (IsDateTime(cellText, Constants.PopularDateTimeFormats, out result)) // DateTime
                                        {
                                            if (Constants.ExcelDefaultDateForTime.Contains(cellText.Left(10))) // Time
                                            {
                                                dr[j - 1] = result.ToString(Constants.Time24Format);
                                            }
                                            else
                                            {
                                                dr[j - 1] = result.ToString(Constants.SqlDateTimeFormat);
                                            }
                                        }
                                        else
                                        {
                                            dr[j - 1] = cellVal;
                                        }
                                    }
                                    else if (IsDateTime(cellVal, Constants.PopularDateFormats, out result)) // Date
                                    {
                                        dr[j - 1] = result.ToString(Constants.SqlDateFormat);
                                    }
                                    else if (IsDateTime(cellVal, Constants.PopularDateTimeFormats, out result)) // DateTime
                                    {
                                        if (Constants.ExcelDefaultDateForTime.Contains(cellVal.Left(10))) // Time
                                        {
                                            dr[j - 1] = result.ToString(Constants.Time24Format);
                                        }
                                        else
                                        {
                                            dr[j - 1] = result.ToString(Constants.SqlDateTimeFormat);
                                        }
                                    }
                                    else
                                    {
                                        dr[j - 1] = cellVal;
                                    }
                                }
                            }
                        }

                        outcols = colsName;
                        outmsg = null;
                        return dataTable;
                    }
                }
                else
                {
                    outcols = new List<string>();
                    outmsg = "The input data specified was not found in the input file.";
                }
            }
            catch (Exception ex)
            {
                outcols = new List<string>();
                outmsg = ex.Message;
            }

            return new DataTable();
        }

        #endregion Import methods

        #region Support methods

        #region Public methods

        public static void SetBookInfo(ExcelWorkbook ws, string author, string title)
        {
            ws.Properties.Author = author;
            ws.Properties.Title = title;
        }

        public static void SetCellComment(ExcelWorksheet ws, int rowIndex, int colIndex, string comment, string author) => ws.Cells[rowIndex, colIndex].AddComment(comment, author);

        public static void SetCellComment(ExcelWorksheet ws, string cellAddress, string comment, string author) => ws.Cells[cellAddress].AddComment(comment, author);

        public static void SetBackgroundColor(ExcelWorksheet ws, int rowIndex, int colIndex, string colorName)
        {
            Color color = Color.FromName(colorName);
            ws.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
        }

        public static void SetBackgroundColor(ExcelWorksheet ws, string ArgbOrColorName, int rowIndex, int colIndex)
        {
            if (ArgbOrColorName.NotNull())
            {
                if (ArgbOrColorName.Contains('|'))
                {
                    string[] sar = ArgbOrColorName.Split(new char[] { '|' });
                    if (sar.NotNull() && sar.Length == 4)
                    {
                        byte a = (byte)(Convert.ToUInt32(sar[0], 16));
                        byte r = (byte)(Convert.ToUInt32(sar[1], 16));
                        byte g = (byte)(Convert.ToUInt32(sar[2], 16));
                        byte b = (byte)(Convert.ToUInt32(sar[3], 16));
                        ws.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(a, r, g, b);
                    }
                }
                else
                {
                    Color color = Color.FromName(ArgbOrColorName);
                    ws.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
                }
            }
        }

        public static void SetBackgroundColor(ExcelWorksheet ws, string cellRange, string ArgbOrColorName)
        {
            if (ArgbOrColorName.NotNull())
            {
                if (ArgbOrColorName.Contains('|'))
                {
                    string[] sar = ArgbOrColorName.Split(new char[] { '|' });
                    if (sar.NotNull() && sar.Length == 4)
                    {
                        byte a = (byte)(Convert.ToUInt32(sar[0], 16));
                        byte r = (byte)(Convert.ToUInt32(sar[1], 16));
                        byte g = (byte)(Convert.ToUInt32(sar[2], 16));
                        byte b = (byte)(Convert.ToUInt32(sar[3], 16));
                        if (cellRange.NotNull())
                        {
                            if (cellRange == "Cells")
                            {
                                ws.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells.Style.Fill.BackgroundColor.SetColor(a, r, g, b);
                            }
                            else
                            {
                                ws.Cells[cellRange].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[cellRange].Style.Fill.BackgroundColor.SetColor(a, r, g, b);
                            }
                        }
                    }
                }
                else
                {
                    Color color = Color.FromName(ArgbOrColorName);
                    if (cellRange.NotNull())
                    {
                        if (cellRange == "Cells")
                        {
                            ws.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells.Style.Fill.BackgroundColor.SetColor(color);
                        }
                        else
                        {
                            ws.Cells[cellRange].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[cellRange].Style.Fill.BackgroundColor.SetColor(color);
                        }
                    }
                }
            }
        }

        public static void SetCellMerging(ExcelWorksheet ws, string value, int fromRow, int fromCol, int toRow, int toCol)
        {
            ws.Cells[fromRow, fromCol].Value = value;
            if (!(fromRow == toRow && fromCol == toCol))
                ws.Cells[fromRow, fromCol, toRow, toCol].Merge = true;
            ws.Cells[fromRow, fromCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[fromRow, fromCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        public static void SetCellMerging(ExcelWorksheet ws, string value, string fromCell, string toCell)
        {
            ws.Cells[fromCell].Value = value;
            if (!(fromCell == toCell))
                ws.Cells[fromCell + ":" + toCell].Merge = true;
            ws.Cells[fromCell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[fromCell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        public static void SetCellFontFamily(ExcelWorksheet ws, int rowIndex, int colIndex, string fontType, string fontName, double? fontSize)
        {
            if (fontType.NotNull())
                SetCellFontType(ws, rowIndex, colIndex, fontType);

            if (fontName.NotNull())
                ws.Cells[rowIndex, colIndex].Style.Font.Name = fontName;

            if (fontSize.HasValue)
                ws.Cells[rowIndex, colIndex].Style.Font.Size = (float)fontSize.Value;
        }

        public static void SetCellFontFamily(ExcelWorksheet ws, string cellRange, string fontType, string fontName, double? fontSize)
        {
            if (fontType.NotNull())
                SetCellFontType(ws, cellRange, fontType);

            if (fontName.NotNull())
                ws.Cells[cellRange].Style.Font.Name = fontName;

            if (fontSize.HasValue)
                ws.Cells[cellRange].Style.Font.Size = (float)fontSize.Value;
        }

        public static void SetCellFontType(ExcelWorksheet ws, int rowIndex, int colIndex, string fontType)
        {
            switch (fontType.UpperCase())
            {
                case "BOLD":
                case "B":
                    {
                        ws.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                        break;
                    }
                case "ITALIC":
                case "I":
                    {
                        ws.Cells[rowIndex, colIndex].Style.Font.Italic = true;
                        break;
                    }
                case "UNDERLINE":
                case "U":
                    {
                        ws.Cells[rowIndex, colIndex].Style.Font.UnderLine = true;
                        break;
                    }
            }
        }

        public static void SetCellFontType(ExcelWorksheet ws, string cellRange, string fontType)
        {
            switch (fontType.UpperCase())
            {
                case "BOLD":
                case "B":
                    {
                        ws.Cells[cellRange].Style.Font.Bold = true;
                        break;
                    }
                case "ITALIC":
                case "I":
                    {
                        ws.Cells[cellRange].Style.Font.Italic = true;
                        break;
                    }
                case "UNDERLINE":
                case "U":
                    {
                        ws.Cells[cellRange].Style.Font.UnderLine = true;
                        break;
                    }
            }
        }

        public static void SetCellFormula(ExcelWorksheet ws, int rowIndex, int colIndex, string formula)
        {
            if (formula.NotNull())
                ws.Cells[rowIndex, colIndex].Formula = formula;
        }

        public static void SetCellFormula(ExcelWorksheet ws, string cellAddress, string formula)
        {
            if (formula.NotNull())
                ws.Cells[cellAddress].Formula = formula;
        }

        public static void SetBorder(ExcelWorksheet ws, int fromRow, int fromCol, int toRow, int toCol, string borderType, ExcelBorderStyle lineStyle = ExcelBorderStyle.Thin)
        {
            string type = borderType.UpperCase();

            if (type.NotNull())
            {
                if (type.Length == 1)
                {
                    switch (type)
                    {
                        case "T":
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = lineStyle;
                            break;

                        case "R":
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = lineStyle;
                            break;

                        case "B":
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = lineStyle;
                            break;

                        case "L":
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = lineStyle;
                            break;

                        case "A":
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = lineStyle;
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = lineStyle;
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = lineStyle;
                            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = lineStyle;
                            break;
                    }
                }
                else if (type != "ALL")
                {
                    foreach (char c in type)
                    {
                        switch (c)
                        {
                            case 'T':
                                ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = lineStyle;
                                break;

                            case 'R':
                                ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = lineStyle;
                                break;

                            case 'B':
                                ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = lineStyle;
                                break;

                            case 'L':
                                ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = lineStyle;
                                break;
                        }
                    }
                }
                else
                {
                    ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = lineStyle;
                    ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = lineStyle;
                    ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = lineStyle;
                    ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = lineStyle;
                }
            }
        }

        public static void SetBorder(ExcelWorksheet ws, string cellRange, string borderType, ExcelBorderStyle lineStyle = ExcelBorderStyle.Thin)
        {
            string type = borderType.UpperCase();

            if (type.NotNull())
            {
                if (type.Length == 1)
                {
                    switch (type)
                    {
                        case "T":
                            ws.Cells[cellRange].Style.Border.Top.Style = lineStyle;
                            break;

                        case "R":
                            ws.Cells[cellRange].Style.Border.Right.Style = lineStyle;
                            break;

                        case "B":
                            ws.Cells[cellRange].Style.Border.Bottom.Style = lineStyle;
                            break;

                        case "L":
                            ws.Cells[cellRange].Style.Border.Left.Style = lineStyle;
                            break;

                        case "A":
                            ws.Cells[cellRange].Style.Border.Top.Style = lineStyle;
                            ws.Cells[cellRange].Style.Border.Right.Style = lineStyle;
                            ws.Cells[cellRange].Style.Border.Bottom.Style = lineStyle;
                            ws.Cells[cellRange].Style.Border.Left.Style = lineStyle;
                            break;
                    }
                }
                else if (type != "ALL")
                {
                    foreach (char c in type)
                    {
                        switch (c)
                        {
                            case 'T':
                                ws.Cells[cellRange].Style.Border.Top.Style = lineStyle;
                                break;

                            case 'R':
                                ws.Cells[cellRange].Style.Border.Right.Style = lineStyle;
                                break;

                            case 'B':
                                ws.Cells[cellRange].Style.Border.Bottom.Style = lineStyle;
                                break;

                            case 'L':
                                ws.Cells[cellRange].Style.Border.Left.Style = lineStyle;
                                break;
                        }
                    }
                }
                else
                {
                    ws.Cells[cellRange].Style.Border.Top.Style = lineStyle;
                    ws.Cells[cellRange].Style.Border.Right.Style = lineStyle;
                    ws.Cells[cellRange].Style.Border.Bottom.Style = lineStyle;
                    ws.Cells[cellRange].Style.Border.Left.Style = lineStyle;
                }
            }
        }

        public static void SetNamedRange(ExcelWorksheet ws, string namedRange, string rangeBase, string formula = null)
        {
            var name = ws.Names.Add(namedRange, ws.Cells[rangeBase]); // eg. rangeBase = "C7:E7";
            if (formula.NotNull()) name.Formula = formula; // eg. formula = "SUBTOTAL(9, C2:C6)";
        }

        public static void InsertPicture(ExcelWorksheet ws, int rowIndex, int colIndex, string imagePath, string picName = "Unknown", int pixelWidth = 100, int pixelHeight = 100)
        {
            Bitmap image = new Bitmap(imagePath);
            if (image.NotNull())
            {
                ExcelPicture excelImage = ws.Drawings.AddPicture(picName, image);
                excelImage.From.Column = colIndex;
                excelImage.From.Row = rowIndex;
                excelImage.SetSize(pixelWidth, pixelHeight);
                excelImage.From.RowOff = Pixel2EMU(2); // 2x2 px space for better alignment
                excelImage.From.ColumnOff = Pixel2EMU(2);
            }
        }

        public static void InsertShape(ExcelWorksheet ws, int rowIndex, int colIndex, eShapeStyle shapeStyle, string text, string objName = "Unknown", int pixelWidth = 100, int pixelHeight = 100)
        {
            ExcelShape excelShape = ws.Drawings.AddShape(objName, shapeStyle);
            excelShape.From.Column = colIndex;
            excelShape.From.Row = rowIndex;
            excelShape.SetSize(pixelWidth, pixelHeight);
            excelShape.From.RowOff = Pixel2EMU(5); // 5x5 px space for better alignment
            excelShape.From.ColumnOff = Pixel2EMU(5);
            excelShape.RichText.Add(text); // adding text into the shape
        }

        public static bool IsDateTime(string date, string format, out DateTime result) => DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out result);

        public static bool IsDateTime(string date, string[] formats, out DateTime result) => DateTime.TryParseExact(date, formats, null, System.Globalization.DateTimeStyles.None, out result);

        #endregion Public methods

        #region Private methods

        private static int Pixel2EMU(int pixels) => (pixels * 9525); // 1px ~ 9525EMU

        #endregion Private methods

        #region Other methods

        public static bool ChangeColumnDataType(DataTable dt, string columnName, Type newType)
        {
            if (dt.Columns.Contains(columnName) == false)
                return false;

            DataColumn column = dt.Columns[columnName];
            if (column.DataType == newType)
                return true;

            bool IsDateColumn = false;
            if ((column.DataType == typeof(DateTime) || column.DataType == typeof(DateTime?)) && newType == typeof(string))
                IsDateColumn = true;

            try
            {
                DataColumn newColumn = new DataColumn("temporary", newType);
                dt.Columns.Add(newColumn);
                foreach (DataRow row in dt.Rows)
                {
                    try { row["temporary"] = Convert.ChangeType(row[columnName], newType); }
                    catch { }
                }
                newColumn.SetOrdinal(column.Ordinal);
                dt.Columns.Remove(columnName);
                newColumn.ColumnName = columnName;
                if (IsDateColumn)
                    foreach (DataRow row in dt.Rows)
                    {
                        string sDate = row[columnName].ToDateTime().ToString("dd/MM/yyyy");
                        row[columnName] = Constants.NullDate.Contains(sDate) ? "" : sDate;
                    }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static List<string> ParseHiddenRowsCols(string input)
        {
            try
            {
                // Syntax:  col1; col2; col3  OR  row1; row2; row6-row8
                // Separator characters:  ','  ';'  '/'  '|'  '\'
                // Example: 1; 2; 3  OR  1; 2; 6-8
                if (input.NotNull())
                {
                    List<string> lstOrg = new List<string>();
                    List<string> lstRange = new List<string>();
                    List<string> lstResult = new List<string>();
                    string[] strArr = input.Split(new char[] { ',', ';', '/', '|', '\\' });
                    if (strArr.NotNull()) foreach (string s in strArr) lstOrg.Add(s.ToTrim());
                    lstRange = lstOrg.Where(x => x.NotNull() && x.Contains('-')).ToList();
                    lstResult = lstOrg.Where(x => x.NotNull() && !x.Contains('-')).ToList();
                    foreach (string s in lstRange)
                    {
                        string[] strTmp = s.Split(new char[] { '-' });
                        for (int i = int.Parse(strTmp[0].Trim()); i <= int.Parse(strTmp[1].Trim()); i++)
                        {
                            lstResult.Add(i.ToString());
                        }
                    }
                    lstResult = lstResult.Where(x => x.NotNull()).OrderBy(x => int.Parse(x)).ToList();
                    return lstResult;
                }
            }
            catch { }
            return new List<string>();
        }

        public static SpecialDict<string, string> ParseBackgroundColor(string input)
        {
            try
            {
                // Syntax:  CellRange | Color
                // Separator characters:  ','  ';'  '/'  '|'  '\'
                // Example: B4:L9 | Yellow  OR  B4:L9 | #D5DE17CC
                if (input.NotNull())
                {
                    List<string> lstOrg = new List<string>();
                    SpecialDict<string, string> result = new SpecialDict<string, string>();
                    string[] strArr = input.Split(new char[] { ',', ';', '/', '|', '\\' });
                    if (strArr.NotNull()) foreach (string s in strArr) lstOrg.Add(s.ToTrim());
                    lstOrg = lstOrg.Where(x => x.NotNull()).ToList();
                    if (lstOrg.Count == 2)
                    {
                        string cellRange = lstOrg[0].Replace(" ", "");
                        if (lstOrg[1].Length >= 7 && char.Parse(lstOrg[1].Substring(0, 1)) == '#')
                        {
                            if (lstOrg[1].Length == 7) // color hex 6 digits #RRGGBB
                            {
                                string sTmp = lstOrg[1].Remove(0, 1); // FF-transparency 100% | CC-transparency 80%
                                result.Add(cellRange, string.Concat("FF", "|", sTmp.Substring(0, 2), "|", sTmp.Substring(2, 2), "|", sTmp.Substring(4, 2)));
                            }
                            else if (lstOrg[1].Length == 9) // color hex 8 digits #RRGGBBAA (web browsers uses #RRGGBBAA)
                            {
                                string sTmp = lstOrg[1].Remove(0, 1); // changes format to #AARRGGBB
                                result.Add(cellRange, string.Concat(sTmp.Substring(6, 2), "|", sTmp.Substring(0, 2), "|", sTmp.Substring(2, 2), "|", sTmp.Substring(4, 2)));
                            }
                        }
                        else
                        {
                            result.Add(cellRange, lstOrg[1]);
                        }
                    }
                    return result;
                }
            }
            catch { }
            return new SpecialDict<string, string>();
        }

        #endregion Other methods

        #endregion Support methods
    }

    public static class ExcelExtensions
    {
        public static void ColumnWidth(this ExcelColumn column, double width)
        {
            // Deduce what the column width would really get set to
            var z = width >= (1 + 2 / 3)
                ? Math.Round((Math.Round(7 * (width - 1 / 256), 0) - 5) / 7, 2)
                : Math.Round((Math.Round(12 * (width - 1 / 256), 0) - Math.Round(5 * width, 0)) / 12, 2);

            // How far off? (will be less than 1)
            var errorAmt = width - z;

            // Calculate what amount to tack onto the original amount to result in the closest possible setting
            var adj = width >= 1 + 2 / 3
                ? Math.Round(7 * errorAmt - 7 / 256, 0) / 7
                : Math.Round(12 * errorAmt - 12 / 256, 0) / 12 + (2 / 12);

            // Set width to a scaled-value that should result in the nearest possible value to the true desired setting
            if (z > 0)
            {
                column.Width = width + adj;
                return;
            }

            column.Width = 0d;
        }
    }
}