using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Components
{
    internal interface IKnowsNode<T>
    {
        LinkedListNode<T> Node { get; set; }
    }
}
