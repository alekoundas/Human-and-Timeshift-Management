using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace WebApplication.Utilities
{
    public class ExcelHelper
    {
        public HSSFWorkbook Workbook;
        private HSSFCellStyle borderedCellStyle;
        public ExcelHelper CreateNewExcel() {

            this.Workbook = new HSSFWorkbook();
            HSSFFont myFont = (HSSFFont)this.Workbook.CreateFont();
            myFont.FontHeightInPoints = 11;
            myFont.FontName = "Tahoma";

            // Defining a border
            this.borderedCellStyle = (HSSFCellStyle)Workbook.CreateCellStyle();
            this.borderedCellStyle.SetFont(myFont);
            this.borderedCellStyle.BorderLeft = BorderStyle.Medium;
            this.borderedCellStyle.BorderTop = BorderStyle.Medium;
            this.borderedCellStyle.BorderRight = BorderStyle.Medium;
            this.borderedCellStyle.BorderBottom = BorderStyle.Medium;
            this.borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

            return this;
        }

        public ExcelHelper AddSheet(string sheetName,List<string> colTitles)
        {
            int counter = 0;
            ISheet Sheet = this.Workbook.CreateSheet(sheetName);
            IRow HeaderRow = Sheet.CreateRow(0);

            foreach (var colTitle in colTitles)
            CreateCell(HeaderRow, counter++, colTitle, this.borderedCellStyle);

            return this;
        }



        public void CreateCell(IRow CurrentRow, int CellIndex, string Value, HSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;

        }
    }
}
