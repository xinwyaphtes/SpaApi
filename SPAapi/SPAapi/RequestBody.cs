using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPAapi
{
  public class RequestBody
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public DateTime FromDT { get; set; }
    public DateTime ToDT { get; set; }
    public string User { get; set; }
  }
}
