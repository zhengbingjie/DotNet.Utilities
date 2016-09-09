﻿namespace YanZhiwei.DotNet.MyXls.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    
    using org.in2bits.MyXls;
    
    using DotNet2.Utilities.Common;
    
    /// <summary>
    ///Myxls 操作帮助类
    /// </summary>
    public class MyxlsExcel
    {
        #region Methods
        
        /// <summary>
        /// 将EXCEL导出到DataTable
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <param name="sheetIndex">Worksheets『从0开始』</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable(string excelPath, int sheetIndex)
        {
            CheckedHanlder.CheckedExcelFileParamter(excelPath, true);
            XlsDocument _excelDoc = new XlsDocument(excelPath);
            DataTable _table = new DataTable();
            Worksheet _sheet = _excelDoc.Workbook.Worksheets[sheetIndex];
            ushort _colCount = _sheet.Rows[1].CellCount;
            ushort _rowCount = (ushort)_sheet.Rows.Count;
            
            for(ushort j = 1; j <= _colCount; j++)
            {
                _table.Columns.Add(new DataColumn(j.ToString()));
            }
            
            for(ushort i = 1; i <= _rowCount; i++)
            {
                DataRow _row = _table.NewRow();
                
                for(ushort j = 1; j <= _colCount; j++)
                {
                    _row[j - 1] = _sheet.Rows[i].GetCell(j).Value;
                }
                
                _table.Rows.Add(_row);
            }
            
            return _table;
        }
        
        /// <summary>
        /// 将集合导出到excel
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="excelPath">保存路径</param>
        /// <param name="sheetName">sheet名称</param>
        public static void ToExecel<T>(IEnumerable<T> source, string excelPath, string sheetName)
        where T : class
        {
            CheckedHanlder.CheckedExcelFileParamter(excelPath, false);
            CheckedHanlder.CheckedExcelExportParamter(source, sheetName);
            
            int _recordCnt = source.Count();
            XlsDocument _xls = new XlsDocument();
            string _savePath = FileHelper.GetExceptName(excelPath);
            _xls.FileName = FileHelper.GetFileName(excelPath);
            
            XF _columnStyle = SetColumnStyle(_xls);
            
            Worksheet _sheet = _xls.Workbook.Worksheets.Add(sheetName);
            int _celIndex = 0, _rowIndex = 1;
            Cells _cells = _sheet.Cells;
            IDictionary<string, string> _fields = ReflectHelper.GetPropertyName<T>();
            string[] _colNames = new string[_fields.Count];
            _fields.Values.CopyTo(_colNames, 0);
            
            foreach(string col in _colNames)
            {
                _celIndex++;
                _cells.Add(1, _celIndex, col, _columnStyle);
            }
            
            foreach(T item in source)
            {
                _rowIndex++;
                _celIndex = 0;
                
                foreach(KeyValuePair<string, string> proItem in _fields)
                {
                    _celIndex++;
                    object _provalue = typeof(T).InvokeMember(proItem.Key, BindingFlags.GetProperty, null, item, null);
                    XF _cellStyle = SetCellStyle(_xls, _provalue.GetType());
                    _cells.Add(_rowIndex, _celIndex, _provalue.ToStringOrDefault(string.Empty), _cellStyle);
                }
            }
            
            _xls.Save(_savePath, true);
        }
        
        /// <summary>
        /// 将DataTable导出到Excel
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="excelPath">保存路径</param>
        /// <param name="sheetName">Sheet名字</param>
        /// 时间：2015-12-08 11:13
        /// 备注：
        public static void ToExecel(DataTable table, string excelPath, string sheetName)
        {
            CheckedHanlder.CheckedExcelFileParamter(excelPath, false);
            CheckedHanlder.CheckedExcelExportParamter(table, sheetName);
            int _recordCnt = table.Rows.Count;
            XlsDocument _xls = new XlsDocument();
            string _savePath = excelPath.Substring(0, excelPath.LastIndexOf(@"\") + 1);
            _xls.FileName = FileHelper.GetFileName(excelPath);
            XF _columnStyle = SetColumnStyle(_xls);
            Worksheet _sheet = _xls.Workbook.Worksheets.Add(sheetName);
            int _celIndex = 1, _rowIndex = 2;
            Cells _cells = _sheet.Cells;
            
            foreach(DataColumn column in table.Columns)
            {
                _cells.Add(1, _celIndex, column.ColumnName, _columnStyle);
                _celIndex++;
            }
            
            _celIndex = 1;
            
            foreach(DataRow t in table.Rows)
            {
                foreach(DataColumn column in table.Columns)
                {
                    XF _cellStyle = SetCellStyle(_xls, column.DataType);
                    Cell _cell = _cells.Add(_rowIndex, _celIndex, t[column.ColumnName], _cellStyle);
                    _celIndex++;
                }
                
                _celIndex = 1;
                _rowIndex++;
            }
            
            _xls.Save(_savePath, true);
        }
        
        private static XF SetCellStyle(XlsDocument xls, Type dataType)
        {
            XF _cellStyle = xls.NewXF();
            _cellStyle.HorizontalAlignment = HorizontalAlignments.Centered;
            _cellStyle.VerticalAlignment = VerticalAlignments.Centered;
            _cellStyle.UseBorder = true;
            _cellStyle.LeftLineStyle = 1;
            _cellStyle.LeftLineColor = Colors.Black;
            _cellStyle.BottomLineStyle = 1;
            _cellStyle.BottomLineColor = Colors.Black;
            _cellStyle.UseProtection = false; // 默认的就是受保护的，导出后需要启用编辑才可修改
            _cellStyle.TextWrapRight = true; // 自动换行
            _cellStyle.Format = TransCellType(dataType);
            return _cellStyle;
        }
        
        /// <summary>
        /// 设置列样式
        /// </summary>
        /// <param name="xls">XlsDocument</param>
        /// <returns>XF</returns>
        /// 时间：2015-12-08 10:03
        /// 备注：
        private static XF SetColumnStyle(XlsDocument xls)
        {
            XF _columnStyle = xls.NewXF();
            _columnStyle.HorizontalAlignment = HorizontalAlignments.Centered;
            _columnStyle.VerticalAlignment = VerticalAlignments.Centered;
            _columnStyle.UseBorder = true;
            _columnStyle.TopLineStyle = 1;
            _columnStyle.TopLineColor = Colors.Grey;
            _columnStyle.BottomLineStyle = 1;
            _columnStyle.BottomLineColor = Colors.Grey;
            _columnStyle.LeftLineStyle = 1;
            _columnStyle.LeftLineColor = Colors.Grey;
            _columnStyle.Pattern = 1; // 单元格填充风格。如果设定为0，则是纯色填充(无色)，1代表没有间隙的实色
            _columnStyle.PatternBackgroundColor = Colors.White;
            _columnStyle.PatternColor = Colors.Silver;
            _columnStyle.Font.Bold = true;
            _columnStyle.Font.Height = 12 * 20;
            return _columnStyle;
        }
        
        private static string TransCellType(Type dataType)
        {
            if(dataType == typeof(DateTime))
                return StandardFormats.Date_Time;
            else
                return StandardFormats.Text;
        }
        
        #endregion Methods
    }
}