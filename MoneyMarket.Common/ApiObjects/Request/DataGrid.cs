using System;
using System.Web;

namespace MoneyMarket.Common.ApiObjects.Request
{
    /// <summary>
    /// request query string class for dataTables.js
    /// </summary>
    public class DataGridRequest
    {
        public int start { get; set; }
        public int length { get; set; }
        public int draw { get; set; }
        public int orderedColumnIndex
        {
            get
            {
                return Int32.Parse(HttpContext.Current.Request.QueryString["order[0][column]"]);
            }
        }
        public string orderedColumnName
        {
            get
            {
                var columnNameIdentifier = string.Format("columns[{0}][data]", orderedColumnIndex);
                return HttpContext.Current.Request.QueryString[columnNameIdentifier];
            }
        }
        public string orderBy
        {
            get
            {
                return HttpContext.Current.Request.QueryString["order[0][dir]"];
            }  //asc - desc?
        }

        /// <summary>
        ///  this fields will be set for response object
        /// </summary>
        public int recordsTotal { get; set; }
    }

    /// <summary>
    /// order types for dataTables.js
    /// </summary>
    public enum DataGridOrderType
    {
        Asc,
        Desc
    }

    public interface IDataTablesJs
    {
        string DT_RowId { get; } // dataTables.js row id property. We will use it for dataGrid <tr> element id attribute
    }

    public enum OrderNum
    {
        Order1 = 1,
        Order2 = 2,
        Order3 = 3,
        Order4 = 4,
        Order5 = 5,
        Order6 = 6,
        Order7 = 7,
        Order8 = 8,
    }
}