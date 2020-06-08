using SPAModel;
using System;

namespace SPAService
{
  public class Service1 : IService1
  {
    public SPAModel.Client ServiceTest(int id)
    {
      var curd = new SqlBaseCURD<Client>();
      var result = curd.Select(id);
      curd.Dispose();
      return result;
    }
  }
}
