using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _project.VisualScriptingTests
{
    public class Triggerer : MonoBehaviour
    {
        private int numberOfTimesTriggered = 0;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                numberOfTimesTriggered++;
                EventBus.Trigger(EventNames.MyCustomCsharpEvent, new TestClass(numberOfTimesTriggered));
            }
        }
    }
}