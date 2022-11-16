using System.Collections;
using System.Windows.Input;

namespace GameEngine
{
    /// <summary>
    /// Register the InputController.KeyDown and InputController.KeyUp to a System.Windows.Controls
    /// element to handle when multiply keys are pressed at the same time
    /// </summary>
    public class InputController : System.Collections.IEnumerable
    {
        /// <summary>
        /// Array with all keys. Determ if pressed or not
        /// </summary>
        private bool[] pressedKeys;

        /// <summary>
        /// Contruct the InputController
        /// </summary>
        public InputController()
        {
            // Thanks to https://stackoverflow.com/a/856165/7626221
            int totalKeys = Enum.GetNames(typeof(Key)).Length;

            pressedKeys = new bool[totalKeys];

            for(int i = 0; i < totalKeys; i++)
                pressedKeys[i] = false;
        }

        /// <summary>
        /// Get all pressed keys in a string
        /// </summary>
        /// <returns>String with all pressed keys</returns>
        public override string ToString()
        {
            string keys = "";

            for (int i = 0; i < pressedKeys.Length; i++)
                if (pressedKeys[i])
                    keys += (Key)i;

            return keys;
        }

        /// <summary>
        /// Determ if the Key is pressed
        /// </summary>
        /// <param name="key">Element from Key enum</param>
        /// <returns>true if the Key is pressed</returns>
        public bool IsPressed(Key key) => pressedKeys[(int)key];

        /// <summary>
        /// Must be registerd on a windows controls object to detect all key down events
        /// </summary>
        /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.keydown"/>
        /// <param name="sender">Sender object from a windows controls element</param>
        /// <param name="e">KeyEventArgs parameter</param>
        public void KeyDown(object sender, KeyEventArgs e) => pressedKeys[(int)e.Key] = true;

        /// <summary>
        /// Must be registerd on a windows controls object to detect all key up events
        /// </summary>
        /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.keyup"/>
        /// <param name="sender">Sender object from a windows controls element</param>
        /// <param name="e">KeyEventArgs parameter</param>
        public void KeyUp(object sender, KeyEventArgs e) => pressedKeys[(int)e.Key] = false;

        /// <summary>
        /// Get the list of all pressed Keys
        /// </summary>
        /// <returns>List of Key elements</returns>
        public IEnumerator GetEnumerator()
        {
            for(int i = 0; i < pressedKeys.Length; i++)
                if(pressedKeys[i])
                    yield return (Key)i;
        }
    }
}
