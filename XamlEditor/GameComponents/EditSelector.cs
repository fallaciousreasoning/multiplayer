using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.GameComponents;

namespace XamlEditor.GameComponents
{
    public class EditSelector : IKnowsGameObject
    {
        public GameObject GameObject { get; set; }
        public Dragger Dragger => GameObject.GetComponent<Dragger>();

        public void Select()
        {
            Dragger.Enabled = true;
        }

        public void Deselect()
        {
            Dragger.Enabled = false;
        }
    }
}
