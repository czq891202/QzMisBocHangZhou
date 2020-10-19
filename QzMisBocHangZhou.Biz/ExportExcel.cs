using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QzMisBocHangZhou.Model;
using System;
using System.Data;
using System.IO;

namespace QzMisBocHangZhou.Biz
{
    public class ExportExcel
    {
        /// <summary>
        /// 导出待审批
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static byte[] ExportTransfer(string tmpFile, UserListViewModel user,string orgId = "")
        {
            if (string.IsNullOrEmpty(orgId)) orgId = user.OrgId;
            var infoList = ArchiveTransferInfoBiz.ExportTransfer(orgId);
            
            XSSFWorkbook excel;
            using (var file = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
            {
                excel = new XSSFWorkbook(file);
                file.Close();
            }

            var sheet = excel.GetSheetAt(0);


            //写入表头数据
            sheet.GetRow(1).GetCell(1).SetCellValue(user.OrgName);
            sheet.GetRow(1).GetCell(5).SetCellValue(DateTime.Now.ToString("yyyy-MM-dd"));

            //移动表尾
            var bodyStartIdx = 3;
            int i = 1;
            sheet.ShiftRows(3, 7, infoList.Count, true, false);

            //写入列表数据
            foreach (var item in infoList)
            {
                var row = sheet.CreateRow(bodyStartIdx);
                CreatCell(row, 0).SetCellValue(i);
                CreatCell(row, 1).SetCellValue(item.LabelCode);
                CreatCell(row, 2).SetCellValue(item.LoanAccount);
                CreatCell(row, 3).SetCellValue(item.QuotaNo);
                CreatCell(row, 4).SetCellValue(item.Borrower);
                CreatCell(row, 5).SetCellValue(1);
                CreatCell(row, 6).SetCellValue(0);
                bodyStartIdx++;
                i++;
            }

            //写入表尾数据
            sheet.GetRow(bodyStartIdx).GetCell(5).SetCellValue(infoList.Count);
            sheet.GetRow(bodyStartIdx + 1).GetCell(2).SetCellValue(user.RealName);
            sheet.GetRow(bodyStartIdx + 1).GetCell(4).SetCellValue(user.Mobile);
            //sheet.GetRow(bodyStartIdx + 1).GetCell(6).SetCellValue(user.Receiver);

            //返回内存流
            using (var ms = new MemoryStream())
            {
                excel.Write(ms);
                ms.Position = 0;

                return ms.ToArray();
            }
        }
        /// <summary>
        /// 导出可移交的
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public static byte[] ExportTransferList(string tmpFile, UserListViewModel user, string orgId, string keywords)
        {
            var infoList = ArchiveTransferInfoBiz.ExportTransferList(orgId, keywords);

            XSSFWorkbook excel;
            using (var file = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
            {
                excel = new XSSFWorkbook(file);
                file.Close();
            }

            var sheet = excel.GetSheetAt(0);


            //写入表头数据
            sheet.GetRow(1).GetCell(1).SetCellValue(user.OrgName);
            sheet.GetRow(1).GetCell(5).SetCellValue(DateTime.Now.ToString("yyyy-MM-dd"));

            //移动表尾
            var bodyStartIdx = 3;
            int i = 1;
            sheet.ShiftRows(3, 7, infoList.Count, true, false);

            //写入列表数据
            foreach (var item in infoList)
            {
                var row = sheet.CreateRow(bodyStartIdx);
                CreatCell(row, 0).SetCellValue(i);
                CreatCell(row, 1).SetCellValue(item.LabelCode);
                CreatCell(row, 2).SetCellValue(item.LoanAccount);
                CreatCell(row, 3).SetCellValue(item.QuotaNo);
                CreatCell(row, 4).SetCellValue(item.Borrower);
                CreatCell(row, 5).SetCellValue(1);
                CreatCell(row, 6).SetCellValue(0);
                bodyStartIdx++;
                i++;
            }

            //写入表尾数据
            sheet.GetRow(bodyStartIdx).GetCell(5).SetCellValue(infoList.Count);
            sheet.GetRow(bodyStartIdx + 1).GetCell(2).SetCellValue(user.RealName);
            sheet.GetRow(bodyStartIdx + 1).GetCell(4).SetCellValue(user.Mobile);
            //sheet.GetRow(bodyStartIdx + 1).GetCell(6).SetCellValue(user.Receiver);

            //返回内存流
            using (var ms = new MemoryStream())
            {
                excel.Write(ms);
                ms.Position = 0;

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 借阅待审核清单导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static byte[] ExportBorrow(string tmpFile, UserListViewModel user)
        {
            var infoList = ArchiveBorrowInfoBiz.GetBorrow(user.OrgId);

            XSSFWorkbook excel;
            using (var file = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
            {
                excel = new XSSFWorkbook(file);
                file.Close();
            }

            var sheet = excel.GetSheetAt(0);

            //写入表头数据
            sheet.GetRow(1).GetCell(1).SetCellValue(user.OrgName);
            sheet.GetRow(1).GetCell(4).SetCellValue(user.RealName);
            sheet.GetRow(1).GetCell(6).SetCellValue(user.Mobile);

            //写入列表数据
            var bodyStartIdx = 4;
            int i = 1;

            sheet.ShiftRows(4, 6, infoList.Count, true, false);

            foreach (var item in infoList)
            {
                var row = sheet.CreateRow(bodyStartIdx);
                CreatCell(row, 0).SetCellValue(i);
                CreatCell(row, 1).SetCellValue(item.LoanAccount);
                CreatCell(row, 2).SetCellValue(item.QuotaNo);
                CreatCell(row, 3).SetCellValue(item.LoanBorrower);
                //CreatCell(row, 4).SetCellValue(item.GuaranteeCrdNo);
                CreatCell(row, 4).SetCellValue("");
                CreatCell(row, 5).SetCellValue(item.UsedBy);
                CreatCell(row, 6).SetCellValue(item.PreReturnDate?.ToString("yyyy-MM-dd"));
                CreatCell(row, 7).SetCellValue(item.RealReturnDate?.ToString("yyyy-MM-dd"));
                //CreatCell(row, 8).SetCellValue(item.ReturnPepole);
                CreatCell(row, 8).SetCellValue("");
                CreatCell(row, 9).SetCellValue("");
                CreatCell(row, 10).SetCellValue("");

                bodyStartIdx++;
                i++;
            }

            //返回内存流
            using (var ms = new MemoryStream())
            {
                excel.Write(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 借阅待审核清单导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static byte[] ExportBorrowListExcel(string tmpFile, UserListViewModel user, string orgId, string keyWords)
        {
            var infoList = ArchiveBorrowInfoBiz.GetBorrowList(orgId, keyWords);

            XSSFWorkbook excel;
            using (var file = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
            {
                excel = new XSSFWorkbook(file);
                file.Close();
            }

            var sheet = excel.GetSheetAt(0);

            //写入表头数据
            sheet.GetRow(1).GetCell(1).SetCellValue(user.OrgName);
            sheet.GetRow(1).GetCell(4).SetCellValue(user.RealName);
            sheet.GetRow(1).GetCell(6).SetCellValue(user.Mobile);

            //写入列表数据
            var bodyStartIdx = 4;
            int i = 1;

            sheet.ShiftRows(4, 6, infoList.Count, true, false);

            foreach (var item in infoList)
            {
                var row = sheet.CreateRow(bodyStartIdx);
                CreatCell(row, 0).SetCellValue(i);
                CreatCell(row, 1).SetCellValue(item.LoanAccount);
                CreatCell(row, 2).SetCellValue(item.QuotaNo);
                CreatCell(row, 3).SetCellValue(item.LoanBorrower);
                //CreatCell(row, 4).SetCellValue(item.GuaranteeCrdNo);
                CreatCell(row, 4).SetCellValue("");
                CreatCell(row, 5).SetCellValue(item.UsedBy);
                CreatCell(row, 6).SetCellValue(item.PreReturnDate?.ToString("yyyy-MM-dd"));
                CreatCell(row, 7).SetCellValue(item.RealReturnDate?.ToString("yyyy-MM-dd"));
                //CreatCell(row, 8).SetCellValue(item.ReturnPepole);
                CreatCell(row, 8).SetCellValue("");
                CreatCell(row, 9).SetCellValue("");
                CreatCell(row, 10).SetCellValue("");

                bodyStartIdx++;
                i++;
            }

            //返回内存流
            using (var ms = new MemoryStream())
            {
                excel.Write(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        public static MemoryStream ExportSettle(string id)
        {
            return null;
        }

        private static ICell CreatCell(IRow row, int idx)
        {
            var cell = row.CreateCell(idx);
            cell.CellStyle.BorderBottom = BorderStyle.Thin;
            cell.CellStyle.BorderLeft = BorderStyle.Thin;
            cell.CellStyle.BorderRight = BorderStyle.Thin;
            cell.CellStyle.BorderTop = BorderStyle.Thin;

            return cell;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MemoryStream DataTableToExcel(DataTable dt, string sheetName = "Sheet1")
        {
            var ms = new NpoiMemoryStream();
            ms.AllowClose = false;
            XSSFWorkbook workbook = new XSSFWorkbook();

            if (!string.IsNullOrEmpty(dt.TableName))
            {
                sheetName = dt.TableName;
            }
            ISheet sheet = workbook.CreateSheet(sheetName);
            #region 【列头及样式】
            XSSFRow headRow = (XSSFRow)sheet.CreateRow(0);
            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            foreach(DataColumn column in dt.Columns)
            {
                headRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                headRow.GetCell(column.Ordinal).CellStyle = headStyle;
            }
            #endregion
            #region【填充内容】
            int rowIndex = 0;
            foreach(DataRow dr in dt.Rows)
            {
                rowIndex++;
                XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                foreach(DataColumn column in dt.Columns)
                {
                    string cellValue = dr[column].ToString();
                    dataRow.CreateCell(column.Ordinal).SetCellValue(cellValue);
                }
            }
            #endregion

            //返回内存流
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }
    }

    public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}
