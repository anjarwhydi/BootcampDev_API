using API.ViewModels;
using System.Collections.Generic;

namespace API.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<GetDataVM> Get();
        Employee Get(string NIK);
        GetDataVM GetData(string NIK);
        //int Insert(Employee employee);
        int Insert(RegisterVM register);
        int Update(Employee employee);
        int Delete(string NIK);
    }
}
