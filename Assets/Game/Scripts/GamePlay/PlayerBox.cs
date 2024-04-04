using System;
using UnityEngine;
using System.Collections;

namespace TechJuego
{
    public class PlayerBox : MonoBehaviour
    {
        private bool canMove = true;
        private void Start()
        {
            SwipeController.OnSwipeDown += (()=> 
            {
                Move(Vector3.down);
            }); 

            SwipeController.OnSwipeLeft +=  (() =>
            {
                Move(Vector3.left);
            }); 

            SwipeController.OnSwipeRight +=  (() => 
            {
                Move(Vector3.right);
            });

            SwipeController.OnSwipeUp += (() =>
            {
                Move(Vector3.up);
            });
        }
        private void Move(Vector3 direction)
        {
            if (canMove)
            {
                canMove = false;
                StartCoroutine(MoveOverTime(gameObject, transform.position + direction * 3, 0.3f, () =>
                {
                    canMove = true;
                }));
            }
        }
        private float easeInSine(float start, float end, float val)
        {
            end -= start;
            return -end * Mathf.Cos(val / 1 * (Mathf.PI / 2)) + end + start;
        }
        float _tweenTimeLeft = 0;
        public IEnumerator MoveOverTime(GameObject objectToMove,  Vector3 endValue,float tweenTime, Action action = null)
        {
            _tweenTimeLeft = 0;
            var newPosition = Vector3.zero;
            float timePercent = 0;
            Vector3 startvalue = objectToMove.transform.position;
            while (_tweenTimeLeft < tweenTime)
            {
                timePercent = (_tweenTimeLeft / tweenTime);
                newPosition.x = easeInSine(startvalue.x, endValue.x, timePercent);
                newPosition.y = easeInSine(startvalue.y, endValue.y, timePercent);
                newPosition.z = easeInSine(startvalue.z, endValue.z, timePercent);
                objectToMove.transform.position = newPosition;
                _tweenTimeLeft += Time.deltaTime;
                yield return null;
            }
            objectToMove.transform.position = endValue;
            action?.Invoke();
        }
        public IEnumerator ValueTo(float startValue, float endValue, float tweenTime,Action<float> outValue, Action action = null)
        {
            _tweenTimeLeft = 0;
            float newPosition = 0;
            float timePercent = 0;
            while (_tweenTimeLeft < tweenTime)
            {
                timePercent = (_tweenTimeLeft / tweenTime);
                newPosition = easeInSine(startValue, endValue, timePercent);
                outValue?.Invoke(newPosition);
                _tweenTimeLeft += Time.deltaTime;
                yield return null;
            }
            outValue?.Invoke(endValue);
            action?.Invoke();
        }
    }
}