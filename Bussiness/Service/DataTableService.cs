using DataAccess.Models.Datatable;

namespace Bussiness.Service
{
    public class DataTableService
    {
        private Datatable _datatable { get; }
        public DataTableService(Datatable datatable)
        {
            _datatable = datatable;
        }

        public DataTableService ConvertData<TEntity>()
        {

        }
    }
}
