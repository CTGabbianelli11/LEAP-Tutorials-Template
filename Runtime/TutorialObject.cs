using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace LEAPGroup.Tutorials
{
    [CreateAssetMenu(fileName = "Tutorial Object", menuName = "Tutorial/Tutorial Object")]
    public class TutorialObject : ScriptableObject
    {
        public void OnEnable()
        {
        }

        #region Events
        public delegate void TutorialCompleted(bool completed);
        public TutorialCompleted TutorialCompletedEvent;

        public UnityEvent TutorialEnteredEvent;
        public UnityEvent TutorialExitedEvent;
        #endregion

        public bool CanGoBack = true;
        public bool Completed
        {
            get { return completed; }
            set
            {
                completed = value;
                TutorialCompletedEvent?.Invoke(completed);
            }
        }
        [SerializeField]
        bool completed;
        public string title;
        public string description;
        public int index;

        public virtual void EnterTutorial()
        {
            Reset();
            TutorialEnteredEvent?.Invoke();
        }

        public virtual void ExitTutorial()
        {
            TutorialExitedEvent?.Invoke();
        }

        public virtual void CheckCondition()
        {
        }

        public void ConditionalCompleted(bool condition)
        {
        }

        public virtual void Reset()
        {
            Completed = false;
        }

        public void TestEnter()
        {
        }
    }
}
