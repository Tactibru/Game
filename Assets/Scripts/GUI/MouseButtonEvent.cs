using UnityEngine;
using System.Collections;

namespace CustomGUI
{
   public class MouseButtonEvent
   {
       public enum ButtonState
       {
           Up,
           Down
       }

       public enum MouseButton
       {
           Left,
           Middle,
           Right
       }

       public ButtonState State;
       public MouseButton Button;
	}
}