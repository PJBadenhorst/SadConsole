﻿#if SFML
using Point = SFML.System.Vector2i;
using Rectangle = SFML.Graphics.IntRect;
using Matrix = SFML.Graphics.Transform;
#elif MONOGAME
using Microsoft.Xna.Framework;
#endif

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SadConsole.Game
{
    /// <summary>
    /// A positionable and animated game object.
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// A translation matrix of 0, 0, 0.
        /// </summary>
#if SFML
        public static Matrix NoMatrix = Matrix.Identity;
#elif MONOGAME
        public static Matrix NoMatrix = Matrix.CreateTranslation(0f, 0f, 0f);
#endif
        /// <summary>
        /// Renderer used for drawing the game object.
        /// </summary>
        protected Consoles.ITextSurfaceRenderer renderer;

        /// <summary>
        /// Reposition the rects of the animation.
        /// </summary>
        protected bool repositionRects;

        /// <summary>
        /// Pixel positioning flag for position.
        /// </summary>
        protected bool usePixelPositioning;

        /// <summary>
        /// Where the console should be located on the screen.
        /// </summary>
        protected Point position;

        /// <summary>
        /// Animation for the game object.
        /// </summary>
        protected Consoles.AnimatedTextSurface animation;

        /// <summary>
        /// Font for the game object.
        /// </summary>
        protected Font font;

        /// <summary>
        /// Gets the name of this animation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Font for the game object.
        /// </summary>
        public Font Font
        {
            get { return font; }
            set { font = value; UpdateRects(position, true); }
        }

        /// <summary>
        /// An offset of where the object is rendered.
        /// </summary>
        protected Point renderOffset;

        /// <summary>
        /// Renderer used to draw the animation of the game object to the screen.
        /// </summary>
        public Consoles.ITextSurfaceRenderer Renderer { get { return renderer; } set { renderer = value; } }

        /// <summary>
        /// Offset applied to drawing the game object.
        /// </summary>
        public Point RenderOffset
        {
            get { return renderOffset; }
            set { renderOffset = value; UpdateRects(value); }
        }

        /// <summary>
        /// Gets or sets the position to render the cells.
        /// </summary>
        public Point Position
        {
            get { return position; }
            set { Point previousPosition = position; position = value; UpdateRects(value); OnPositionChanged(previousPosition); }
        }

        /// <summary>
        /// Treats the <see cref="Position"/> of the console as if it is pixels and not cells.
        /// </summary>
        public bool UsePixelPositioning { get { return usePixelPositioning; } set { usePixelPositioning = value; UpdateRects(position); } }

        /// <summary>
        /// The current animation.
        /// </summary>
        public Consoles.AnimatedTextSurface Animation { get { return animation; } set { animation = value; animation.Font = font; UpdateRects(position, true); } }

        /// <summary>
        /// Collection of animations associated with this game object.
        /// </summary>
        public Dictionary<string, Consoles.AnimatedTextSurface> Animations { get; protected set; } = new Dictionary<string, Consoles.AnimatedTextSurface>();

        /// <summary>
        /// When false, this <see cref="GameObject"/> won't be rendered.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// When true, the position of the game object will offset all of the surface rects instead of using a positioning matrix for rendering.
        /// </summary>
        public bool RepositionRects
        {
            get { return repositionRects; }
            set
            {
                repositionRects = value;
                UpdateRects(position, true);
            }
        }


        /// <summary>
        /// Creates a new GameObject.
        /// </summary>
        public GameObject(Font font)
        {
            renderer = new Consoles.TextSurfaceRenderer();
            animation = new Consoles.AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
            var frame = animation.CreateFrame();
            frame[0].GlyphIndex = 1;
            this.font = animation.Font = font;
        }

        /// <summary>
        /// Called when the <see cref="Position" /> property changes.
        /// </summary>
        /// <param name="oldLocation">The location before the change.</param>
        protected virtual void OnPositionChanged(Point oldLocation) { }

        /// <summary>
        /// Resets all of the rects of the animation based on <see cref="UsePixelPositioning"/> and if <see cref="RepositionRects"/> is true.
        /// </summary>
        /// <param name="position">The position of the game object.</param>
        /// <param name="force">When true, always repositions rects.</param>
        protected void UpdateRects(Point position, bool force = false)
        {
            if (repositionRects || force)
            {
                var width = Animation.Width;
                var height = Animation.Height;
                var font = Animation.Font;
                Point offset;

                var rects = new Rectangle[width * height];

                if (repositionRects && usePixelPositioning)
                {
                    offset = position + renderOffset - new Point(animation.Center.X * font.Size.X, animation.Center.Y * font.Size.Y);
                }
                else if (repositionRects)
                {
                    offset = position + renderOffset - animation.Center;
                    offset = new Point(offset.X * font.Size.X, offset.Y * font.Size.Y);
                }
                else
                {
                    offset = new Point();
                }

#if SFML
                animation.AbsoluteArea = new Rectangle(offset.X, offset.Y, (width * font.Size.X) + offset.X, (height * font.Size.Y) + offset.Y);
#elif MONOGAME
                animation.AbsoluteArea = new Rectangle(offset.X, offset.Y, width * font.Size.X, height * font.Size.Y);
#endif

                int index = 0;
                
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
#if SFML
                        rects[index] = new Rectangle(x * font.Size.X + offset.X, y * font.Size.Y + offset.Y, font.Size.X + (x * font.Size.X + offset.X), font.Size.Y + (y * font.Size.Y + offset.Y));
#elif MONOGAME
                        rects[index] = new Rectangle(x * font.Size.X + offset.X, y * font.Size.Y + offset.Y, font.Size.X, font.Size.Y);
#endif
                        index++;
                    }
                }

                animation.RenderRects = rects;
            }
        }

        /// <summary>
        /// Forces the rendering rectangles to update with positioning information.
        /// </summary>
        public void UpdateAnimationRectangles()
        {
            UpdateRects(position, true);
        }
        
        /// <summary>
        /// Draws the game object.
        /// </summary>
        public virtual void Render()
        {
            if (IsVisible)
            {
                if (repositionRects)
                    renderer.Render(Animation, NoMatrix);   
                else
                    renderer.Render(Animation, position + renderOffset - animation.Center, usePixelPositioning);
            }
        }

        /// <summary>
        /// Updates the animation.
        /// </summary>
        public virtual void Update()
        {
            Animation.Update();
        }

        /// <summary>
        /// Saves this <see cref="GameObject"/> to a file.
        /// </summary>
        /// <param name="file">The file to save.</param>
        public void Save(string file)
        {
            Serialized.Save(this, file);
        }

        /// <summary>
        /// Loads a <see cref="GameObject"/> from a file.
        /// </summary>
        /// <param name="file">The file to load.</param>
        /// <returns></returns>
        public static GameObject Load(string file)
        {
            return Serialized.Load(file);
        }

        /// <summary>
        /// Serialized instance of a <see cref="GameObject"/>.
        /// </summary>
        [DataContract]
        public class Serialized
        {
            [DataMember]
            public string storedAnimationName;
            [DataMember]
            public bool storedAsName;
            [DataMember]
            public Consoles.AnimatedTextSurface.Serialized Animation;
            [DataMember]
            public List<Consoles.AnimatedTextSurface.Serialized> Animations;
            [DataMember]
            public string FontName;
            [DataMember]
            public Font.FontSizes FontSize;
            [DataMember]
            public bool IsVisible;
            [DataMember]
            public Point Position;
            [DataMember]
            public bool RepositionRects;
            [DataMember]
            public bool UsePixelPositioning;
            [DataMember]
            public string Name;
            [DataMember]
            public Point RenderOffset;

            public Serialized(GameObject gameObject)
            {
                Animations = new List<Consoles.AnimatedTextSurface.Serialized>();

                foreach (var item in gameObject.Animations)
                    Animations.Add(new Consoles.AnimatedTextSurface.Serialized(item.Value));

                IsVisible = gameObject.IsVisible;
                Position = gameObject.position;
                RepositionRects = gameObject.repositionRects;
                UsePixelPositioning = gameObject.usePixelPositioning;
                Name = gameObject.Name;
                FontName = gameObject.font.Name;
                FontSize = gameObject.font.SizeMultiple;
                RenderOffset = gameObject.renderOffset;

                if (gameObject.Animations.ContainsValue(gameObject.animation) && gameObject.Animations.ContainsKey(gameObject.animation.Name))
                {
                    storedAsName = true;
                    storedAnimationName = gameObject.animation.Name;
                }
                else
                {
                    storedAsName = false;
                    storedAnimationName = gameObject.animation.Name;
                    Animation = new Consoles.AnimatedTextSurface.Serialized(gameObject.animation);
                }
            }

            public static void Save(GameObject gameObject, string file)
            {
                var animation = new Serialized(gameObject);
                Serializer.Save(animation, file, new Type[] { typeof(Consoles.AnimatedTextSurface[]), typeof(Consoles.AnimatedTextSurface) });
            }

            public static GameObject Load(string file)
            {
                var loadedGameObject = Serializer.Load<Serialized>(file, new Type[] { typeof(Consoles.AnimatedTextSurface[]), typeof(Consoles.AnimatedTextSurface) });
                return Get(loadedGameObject);
                
            }

            public static GameObject Get(Serialized serializedObject)
            {
                Font font;

                // Try to find font
                if (Engine.Fonts.ContainsKey(serializedObject.FontName))
                    font = Engine.Fonts[serializedObject.FontName].GetFont(serializedObject.FontSize);
                else
                    font = Engine.DefaultFont;

                var gameObject = new GameObject(font);

                gameObject.Animations = new Dictionary<string, Consoles.AnimatedTextSurface>();

                foreach (var item in serializedObject.Animations)
                {
                    var animation = Consoles.AnimatedTextSurface.Serialized.Get(item);
                    gameObject.Animations.Add(animation.Name, animation);
                }

                gameObject.IsVisible = serializedObject.IsVisible;
                gameObject.position = serializedObject.Position;
                gameObject.usePixelPositioning = serializedObject.UsePixelPositioning;
                gameObject.Name = serializedObject.Name;
                gameObject.font = font;
                gameObject.repositionRects = serializedObject.RepositionRects;
                gameObject.renderOffset = serializedObject.RenderOffset;

                if (serializedObject.storedAsName)
                    gameObject.animation = gameObject.Animations[serializedObject.storedAnimationName];
                else
                    gameObject.animation = Consoles.AnimatedTextSurface.Serialized.Get(serializedObject.Animation);

                gameObject.animation.Font = gameObject.font;
                gameObject.UpdateAnimationRectangles();

                return gameObject;
            }
        }
    }
}
