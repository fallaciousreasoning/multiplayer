using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;

namespace MultiPlayer.Factories
{
 public class BasicPrefab : IPrefab
 {
  private readonly Func<EntityBuilder> builder;

  public BasicPrefab(Func<EntityBuilder> builder)
  {
   this.builder = builder;
  }

  public List<Entity> Build()
  {
   return builder().Create();
  }
 }
}
