using SadConsole.Consoles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SadConsole.Game
{
    /// <summary>
    /// Represents a game world map. Maps are associated with a text surface.
    /// </summary>
    public class Map
    {
        public ITextSurfaceRendered Surface;

        public GameObjectCollection Objects;

        public List<Region> Regions;

        public void a()
        {
            
        }
    }
}
