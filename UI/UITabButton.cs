// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ColdironTools.EditorExtensions;

namespace ColdironTools.UI
{
    /// <summary>
    /// Controls behaviour for displaying certain objects based on a selected tab.
    /// </summary>
    public class UITabButton : MonoBehaviour, IPointerClickHandler
    {
        #region StructsAndEnums
        /// <summary>
        /// Enum containing color or sprite options.
        /// </summary>
        [System.Serializable]
        public enum TabTransiton
        {
            Color, SpriteSwap
        }

        /// <summary>
        /// Color values for the color transition option.
        /// </summary>
        [System.Serializable]
        public struct TransitionColors
        {
            public Color inactiveColor;
            public Color activeColor;

            public TransitionColors(Color active, Color inactive)
            {
                inactiveColor = inactive;
                activeColor = active;
            }
        }

        /// <summary>
        /// Sprite values for the sprite swap transition option.
        /// </summary>
        [System.Serializable]
        public struct TransitionSprites
        {
            public Sprite inactiveSprite;
            public Sprite activeSprite;
        }
        #endregion

        #region Fields
        [Header("Targets")]
        [Tooltip("The graphics that represent the tab options.")]
        [SerializeField] private List<Graphic> targetGraphics = new List<Graphic>();

        [Tooltip("The game objects that contain the tab options")]
        [SerializeField] private List<GameObject> targetTabObjects = new List<GameObject>();

        [Header("Transition Data")]
        [Tooltip("The effect used to represent active and inactive tabs.")]
        [SerializeField] private TabTransiton transitionType = TabTransiton.Color;

        [Tooltip("The active and inactive colors for the transition graphics.")]
        [SerializeField, ConditionalHide("transitionType", 1)] private TransitionColors transitionColors = new TransitionColors(Color.white, Color.grey);

        [Tooltip("The active and inactive sprites for the transition graphics.")]
        [SerializeField, ConditionalHide("transitionType", 0)] private TransitionSprites transitionSprites = new TransitionSprites();

        [Tooltip("The index of the target object that should be active when the game starts.")]
        [SerializeField] private int startingTabIndex = 0;
        #endregion

        #region Methods
        /// <summary>
        /// Sets the target graphic and object at the startingTabIndex to active and all the others to inactive.
        /// </summary>
        private void Awake()
        {
            for (int i = 0; i < targetTabObjects.Count; i++)
            {
                if (targetGraphics.Count >= i && targetGraphics[i] != null)
                {
                    if (i == startingTabIndex)
                    {
                        TargetTransition(targetGraphics[i], true);
                        targetTabObjects[i].gameObject.SetActive(true);
                        continue;
                    }
                    else
                    {
                        TargetTransition(targetGraphics[i], false);
                        targetTabObjects[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Transitions active tabs if the pointer is over a target graphic when clicked.
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            for (int i = 0; i < targetTabObjects.Count; i++)
            {
                if (targetGraphics.Count >= i && targetGraphics[i] != null)
                {
                    if (eventData.hovered.Contains(targetGraphics[i].gameObject))
                    {
                        TargetTransition(targetGraphics[i], true);
                        targetTabObjects[i].gameObject.SetActive(true);
                        continue;
                    }
                    else
                    {
                        TargetTransition(targetGraphics[i], false);
                        targetTabObjects[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Tranisitions the graphic data based on the tranistion type.
        /// </summary>
        /// <param name="graphic">The graphic to be transitioned</param>
        /// <param name="isActive">the state to which the graphic is being transitioned</param>
        protected virtual void TargetTransition(Graphic graphic, bool isActive)
        {
            switch (transitionType)
            {
                case TabTransiton.Color:
                    graphic.color = isActive ? transitionColors.activeColor : transitionColors.inactiveColor;
                    break;
                case TabTransiton.SpriteSwap:
                    Image image = (Image)graphic;
                    if (image)
                    {
                        image.sprite = isActive ? transitionSprites.activeSprite : transitionSprites.inactiveSprite;
                    }
                    break;
            }
        }
    }
    #endregion
}