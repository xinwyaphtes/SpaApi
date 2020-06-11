using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeihanLi;
namespace sp.wuxiapptec.com
{
  public class ExcelHandler
  {
    /// <summary>
    /// 将sheet转换为Datatable，默认第一行为行头,如果要指定行头，则输入columnNames
    /// </summary>
    /// <param name="fileName">filename</param>
    /// <param name="sheetName">excelsheetName</param>
    /// <param name="columnNames">columnNames</param>
    /// <returns></returns>
    public DataTable ConvertSheetToDataTable(string fileName, string sheetName, List<string> columnNames = null)
    {
      using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
      {
        IWorkbook workbook = null;

        if (fileName.IndexOf(".xlsx") > 0)
          workbook = new XSSFWorkbook(fs);
        else if (fileName.IndexOf(".xls") > 0) // 2003
          workbook = new HSSFWorkbook(fs);

        return WorksheetToTable(workbook.GetSheet(sheetName));
      }
    }

    public DataTable ConverSheetToTableSpecificCol(string fileName, string sheetName)
    {
      IWorkbook wb = WeihanLi.Npoi.ExcelHelper.LoadExcel(fileName);
      ISheet st = wb.GetSheet(sheetName);
      WeihanLi.Npoi.NpoiRowCollection rows = WeihanLi.Npoi.NpoiExtensions.GetRowCollection(st);
      int rowindex = 0;
      foreach (IRow item in rows)
      {
        item.Cells.ForEach(x =>
        {
          x.SetCellType(CellType.String);
        });
        if (item.Cells.Select(x => x.StringCellValue).SequenceEqual(new string[] {
"ID"
,"Description"
,"Output File"
,"QC Method"
,"Spec Author"
,"Date Spec Ready"
,"Production Program"
,"Production Programmer"
,"Date Program Ready for QC"
,"QC Program"
,"QC Programmer"
,"Status"
,"Data Status"
,"Comments for Internal Tracking"
,"ST Review Completed or not?"
,"Ready for Delivery or not?"
,"Date Pre-deliverable \nQC Completed"
,"Comments (e.g., outstanding issues) for Delivery"
,"Use or not?"
,"Spec Author History"
,"Production Programer History"
,"QC Programer History"}))
        { rowindex = item.RowNum; break; }
      }
      return WeihanLi.Npoi.NpoiExtensions.ToDataTable(st, rowindex);
    }
    /// <summary>
    /// 通过DataTable导出Excel
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sheetName"></param>
    //public void SaveExcelWithDataTable(DataTable dt, string fileName, string sheetName)
    //{
    //  using (ExcelPackage package = new ExcelPackage())
    //  {
    //    var worksheet = package.Workbook.Worksheets.Add(sheetName);
    //    worksheet.Cells.LoadFromDataTable(dt, true);

    //    package.SaveAs(new FileInfo(fileName));
    //  }
    //}

    /// <summary>
    /// 判断excel是否存在
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool OpenExcel(string fileName)
    {
      try
      {
        if (File.Exists(fileName) && (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx")))
        {
          using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
          {
            IWorkbook workbook = null;

            if (fileName.IndexOf(".xlsx") > 0)
              workbook = new XSSFWorkbook(fs);
            else if (fileName.IndexOf(".xls") > 0) // 2003
              workbook = new HSSFWorkbook(fs);

            return workbook != null;
          }
        }
        else
        {
          return false;

        }
      }
      catch (Exception e)
      {
        return false;
      }
    }

    /// <summary>
    /// 判断excelSheet是否存在
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sheetName"></param>
    /// <returns></returns>
    public bool OpenSheet(string fileName, string sheetName)
    {
      try
      {
        using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
          IWorkbook workbook = null;

          if (fileName.IndexOf(".xlsx") > 0)
            workbook = new XSSFWorkbook(fs);
          else if (fileName.IndexOf(".xls") > 0) // 2003
            workbook = new HSSFWorkbook(fs);
          var sheet = workbook.GetSheet(sheetName);
          return sheet != null;
        }
      }
      catch (Exception e)
      {
        return false;
      }
    }

    /// <summary>
    /// 将worksheet转成datatable
    /// </summary>
    /// <param name="worksheet">待处理的worksheet</param>
    /// <returns>返回处理后的datatable</returns>
    private DataTable WorksheetToTable(ISheet worksheet, bool isFirstRowColumn = true)
    {
      IRow firstRow = worksheet.GetRow(0);
      int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
      DataTable dt = new DataTable();
      int startRow = 0;
      if (isFirstRowColumn)
      {
        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
        {
          ICell cell = firstRow.GetCell(i);
          if (cell != null)
          {
            string cellValue = cell.StringCellValue;
            if (!string.IsNullOrEmpty(cellValue))
            {
              DataColumn column = new DataColumn(cellValue);
              dt.Columns.Add(column);
            }
          }
        }
        startRow = worksheet.FirstRowNum + 1;
      }
      else
      {
        startRow = worksheet.FirstRowNum;
      }

      //最后一列的标号
      int rowCount = worksheet.LastRowNum;
      for (int i = startRow; i <= rowCount; ++i)
      {
        IRow row = worksheet.GetRow(i);
        if (row == null) continue; //没有数据的行默认是null　　　　　　　

        DataRow dataRow = dt.NewRow();
        for (int j = row.FirstCellNum; j < cellCount; ++j)
        {
          if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
            dataRow[j] = row.GetCell(j).ToString();
        }
        dt.Rows.Add(dataRow);
      }
      return dt;
    }

    /// <summary>
    /// 将worksheet转成datatable并指定列头
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="columnNames"></param>
    /// <returns></returns>
    //private DataTable WorksheetToTable(ExcelWorksheet worksheet, List<string> columnNames)
    //{
    //  //获取worksheet的行数
    //  int rows = worksheet.Dimension.End.Row;
    //  //获取worksheet的列数
    //  int cols = worksheet.Dimension.End.Column;

    //  DataTable dt = new DataTable(worksheet.Name);
    //  DataRow dr = null;
    //  bool isHead = false;
    //  for (int i = 1; i <= rows; i++)
    //  {
    //    if (isHead)
    //      dr = dt.Rows.Add();
    //    var columnList = new List<string> { };
    //    for (int j = 1; j <= cols; j++)
    //    {
    //      var tempValue = worksheet.Cells[i, j].Value?.ToString();
    //      columnList.Add(tempValue);
    //      dt.Columns.Add(tempValue);
    //      if (isHead) dr[j - 1] = tempValue;
    //    }
    //    if (columnList.All(b => columnNames.Any(a => a == b)) && isHead == false) isHead = true;
    //    else if (isHead == false) dt = new DataTable(worksheet.Name);
    //  }
    //  return dt;
    //}
  }
}
