using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPAModel
{
  public partial class Client
  {
    [Key]
    public int CLIENT_ID { get; set; }
    public string CLIENTCD { get; set; }
    public string Clientnm { get; set; }
    public string Clientloc { get; set; }
    public string CREATE_USER { get; set; }
    public DateTime CREATE_TM { get; set; }
  }
}
