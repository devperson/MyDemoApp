using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Utils.Extenstions
{
    public static class NodeExtenstions
    {

        /// <summary>
        /// iOS hidden property only make it hidden same as Android Visibility.Invisible.
        /// This method is similar to Android Visibility.Gone.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="visible"></param>
        /// <exception cref="System.Exception"></exception>
        public static void SetVisibility(this ASDisplayNode node, bool visible) 
        {
            if (node.Style.PreferredSize.Width > 1 || node.Style.PreferredSize.Height > 1)
                throw new System.Exception("Can not use SetVisibilityGone for this node as it has fixed value set for width or height and this method will override it to auto when node gets visibility.");

            if (visible)
            {
                node.Style.PreferredLayoutSize = new ASLayoutSize
                {
                    width = new ASDimension { unit = ASDimensionUnit.Auto },
                    height = new ASDimension { unit = ASDimensionUnit.Auto }
                };
                node.Hidden = false;
            }
            else
            {
                node.Style.PreferredSize = new CGSize(1, 1);
                node.Hidden = true;
            }
        }
    }
}
