using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Datatable
{
    public class DatatableResponse<T>
    {
        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public IEnumerable<T> data { get; set; }
    }

}
