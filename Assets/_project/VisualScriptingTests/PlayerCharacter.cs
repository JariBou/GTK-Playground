using System;
using Unity.VisualScripting;

namespace _project.VisualScriptingTests
{
    [Serializable, Inspectable]
    public class PlayerCharacter
    {
        [Inspectable]
        public string name; 
        [Inspectable]
        public string type;
        [Inspectable]
        public string color;
        [Inspectable]
        public int level;
    }
}