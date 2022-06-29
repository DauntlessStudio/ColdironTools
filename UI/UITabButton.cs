// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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

        [System.Serializable]
        public struct TabPair
        {
            public GameObject tabButton;
            public GameObject tabPanel;

            public bool IsValid()
            {
                return !(tabButton == null || tabPanel == null);
            }
        }
        #endregion

        #region Fields
        [Header("Targets")]
        [Tooltip("The game object to use as a tab button, and the corresponding element to be tabbed.")]
        [SerializeField, NonReorderable] private List<TabPair> tabPairs = new List<TabPair>();
        private List<GameObject> buttonsFromPairs = new List<GameObject>();

        [Header("Transition Data")]
        [Tooltip("The effect used to represent active and inactive tabs.")]
        [SerializeField] private TabTransiton transitionType = TabTransiton.Color;

        [Tooltip("The active and inactive colors for the transition graphics.")]
        [SerializeField, ConditionalHide("transitionType", 0)] private TransitionColors transitionColors = new TransitionColors(Color.white, Color.grey);

        [Tooltip("The active and inactive sprites for the transition graphics.")]
        [SerializeField, ConditionalHide("transitionType", 1)] private TransitionSprites transitionSprites = new TransitionSprites();

        [Tooltip("The index of the target object that should be active when the game starts.")]
        [SerializeField] private int startingTabIndex = 0;
        #endregion

        #region Methods
        /// <summary>
        /// Sets the target graphic and object at the startingTabIndex to active and all the others to inactive.
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < tabPairs.Count; i++)
            {
                ToggleTab(tabPairs[i], startingTabIndex == i);
            }

            buttonsFromPairs = tabPairs.Select(x => x.tabButton).ToList();
        }

        /// <summary>
        /// Transitions active tabs if the pointer is over a target graphic when clicked.
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            GameObject buttonObject = buttonsFromPairs.Intersect(eventData.hovered).FirstOrDefault();
            if (!buttonObject) return;

            TabPair activePair = tabPairs[buttonsFromPairs.IndexOf(buttonObject)];
            if (activePair.IsValid())
            {
                for (int i = 0; i < tabPairs.Count; i++)
                {
                    ToggleTab(tabPairs[i], tabPairs[i].Equals(activePair));
                }
            }
        }

        /// <summary>
        /// Tranisitions the graphic data based on the tranistion type.
        /// </summary>
        /// <param name="graphic">The graphic to be transitioned</param>
        /// <param name="isActive">the state to which the graphic is being transitioned</param>
        protected virtual void ToggleTab(TabPair tab, bool isActive)
        {
            Graphic graphic = tab.tabButton.GetComponent<Graphic>();
            if (!graphic) return;

            tab.tabPanel.SetActive(isActive);

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