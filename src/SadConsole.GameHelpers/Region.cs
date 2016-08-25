#if SFML

#elif MONOGAME
using Microsoft.Xna.Framework;
#endif

using SadConsole.Consoles;

using System;
using System.Collections.Generic;
using System.Text;

namespace SadConsole.Game
{
    /// <summary>
    /// An area of a console.
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Unique identifier for this region object.
        /// </summary>
        public Guid ID = Guid.NewGuid();

        /// <summary>
        /// The area covered by the region.
        /// </summary>
        public Rectangle Area = new Rectangle();

        /// <summary>
        /// Settings associated with the region.
        /// </summary>
        public Dictionary<string, string> Settings = new Dictionary<string, string>();

        /// <summary>
        /// Gets a list of cells that the region covers.
        /// </summary>
        /// <param name="surface">The surface to apply the region to.</param>
        /// <returns>An array of cells from the surface this region covers.</returns>
        public Cell[] GetCells(ITextSurface surface)
        {
            throw new NotImplementedException();
        }
    }
}
