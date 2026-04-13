using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _project.VisualScriptingTests
{
    [Serializable, Inspectable]
    public class TestClass
    {
        [SerializeField, Inspectable] private int _int;

        [Inspectable]
        public string TestString;

        public TestClass(){}
    
        public TestClass(int numberOfTimesTriggered)
        {
            _int = numberOfTimesTriggered;
        }

        public int numberOfTimesTriggered => _int;
    }
}