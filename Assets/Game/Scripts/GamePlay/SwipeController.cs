using UnityEngine;
using UnityEngine.Events;

namespace TechJuego
{
	public class SwipeController : MonoBehaviour
	{
		float tSensitivity = 15;
		public static UnityAction OnSwipeRight;
		public static UnityAction OnSwipeLeft;                              
		public static UnityAction OnSwipeUp;
		public static UnityAction OnSwipeDown;

		private float swipe_Initial_X, swipe_Final_X;
		private float swipe_Initial_Y, swipe_Final_Y;
		private int toucheCount;
		private float present_Input_X, present_Input_Y;
		private float angle;
		private float swipe_Distance;

		void Start()
		{
			swipe_Initial_X = 0.0f;
			swipe_Initial_Y = 0.0f;
			swipe_Final_X = 0.0f;
			swipe_Final_Y = 0.0f;
			present_Input_X = 0.0f;
			present_Input_Y = 0.0f;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0) && toucheCount == 0)
			{
				swipe_Initial_X = Input.mousePosition.x;
				swipe_Initial_Y = Input.mousePosition.y;
				toucheCount = 1;
			}
			if (toucheCount == 1)
			{
				swipe_Final_X = Input.mousePosition.x;
				swipe_Final_Y = Input.mousePosition.y;
			}
			swipeDirection();
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				toucheCount = 0;
			}
		}
		void swipeDirection()
		{

			if (toucheCount != 1)
				return;
			present_Input_X = swipe_Final_X - swipe_Initial_X;
			present_Input_Y = swipe_Final_Y - swipe_Initial_Y;
			angle = present_Input_Y / present_Input_X;

			swipe_Distance = Mathf.Sqrt(Mathf.Pow((swipe_Final_Y - swipe_Initial_Y), 2) + Mathf.Pow((swipe_Final_X - swipe_Initial_X), 2));

			if (swipe_Distance <= (Screen.width / tSensitivity))
				return;

			if ((present_Input_X >= 0 || present_Input_X <= 0) && present_Input_Y > 0 && (angle > 1 || angle < -1))
			{ //...... Swipe Jump  
				OnSwipeUp?.Invoke();
				toucheCount = -1;
			}
			else
			if (present_Input_X > 0 && (present_Input_Y >= 0 || present_Input_Y <= 0) && (angle < 1 && angle >= 0 || angle > -1 && angle <= 0))
			{//.........Swipe Right 
				OnSwipeRight?.Invoke();
				toucheCount = -1;
			}
			else
			if (present_Input_X < 0 && (present_Input_Y >= 0 || present_Input_Y <= 0) && (angle > -1 && angle <= 0 || angle >= 0 && angle < 1))
			{//........Swipe Left
				OnSwipeLeft?.Invoke();
				toucheCount = -1;
			}
			else
			if ((present_Input_X >= 0 || present_Input_X <= 0) && present_Input_Y < 0 && (angle < -1 || angle > 1))
			{   //..........Swipe Down 
				OnSwipeDown?.Invoke();
				toucheCount = -1;
			}
			else
			{
				toucheCount = 0;
			}
		}
	}
}