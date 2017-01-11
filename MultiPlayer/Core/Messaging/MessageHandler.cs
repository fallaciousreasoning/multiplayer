using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Messaging
{
 public class MessageHandler
 {
  public Type SystemType { get; set; }
  public Action<object, object> Handle { get; set; }
 }
}
