// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace ColdironTools.UI
{
    /// <summary>
    /// Contains a static method to set the alpha hit test minimum threshold of an object if possible.
    /// </summary>
    public class AlphaHitTest
    {
        #region Methods
        /// <summary>
        /// If the GameObject contains an Image component, who's sprite has read/write enabled,
        /// the hit test minimum will be set. This means only non-transparent parts of the sprite can be clicked.</summary>
        /// <param name="gameObject">The GameObject to set the hit test value of</param>
        public static void AttemptTransparencyHitTest(GameObject gameObject)
        {
            Image image = gameObject.GetComponent<Image>();
            if (image && image.sprite)
            {
                if (image.sprite.texture.isReadable)
                {
                    image.alphaHitTestMinimumThreshold = 1.0f;
                }
                else
                {
                    Debug.LogWarning(image.sprite.name + " is not set read/write enabled, and cannot alter it's alpha hit test.", image.sprite);
                }
            }
        }
        #endregion
    }
}