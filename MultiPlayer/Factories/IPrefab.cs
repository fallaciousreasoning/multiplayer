using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;

namespace MultiPlayer.Factories
{
 public interface IPrefab
 {
  List<Entity> Build();
 }
}