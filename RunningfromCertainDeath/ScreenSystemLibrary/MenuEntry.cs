using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScreenSystemLibrary
{
    public abstract class MenuEntry
    {
        #region State Enum, Field, and Property
        public enum EntryState
        {
            Normal,
            Highlight,
            Selected,
        }

        EntryState state = EntryState.Normal;
        public EntryState State
        {
            get { return state; }
        }

        #endregion

        #region Fields and Properties
        /// <summary>
        /// Title of the entry
        /// </summary>
        public string EntryTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Description of what this entry does
        /// </summary>
        public string EntryDescription
        {
            get;
            set;
        }

        public Point EntryPadding
        {
            get;
            private set;
        }

        /// <summary>
        /// The texture of this entry
        /// </summary>
        public Texture2D EntryTexture
        {
            get;
            private set;
        }

        public Rectangle BoundingRectangle
        {
            get;
            private set;
        }

        //This will essential let us know if it is a sheet or not (1x1 is not a sheet)
        public Point numberOfEntries;

        //If your texture is a sheet, we need display regions
        Dictionary<bool, Rectangle> displayRegion;

        public bool IsSheet
        {
            get
            {
                return (numberOfEntries.X == 1 && numberOfEntries.Y == 1);
            }
        }

        /// <summary>
        /// The initial position of the menu entry
        /// Useful if we are doing a lot of movements
        /// </summary>
        public Vector2 InitialPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// Current relative position of the menu entry.
        /// If there is no texture, this will be relative to the
        /// game window (like normal positioning).
        /// 
        /// If there is a texture, this will be relative to the
        /// texture's center.  Leave at Vector2.Zero if you have a texture
        /// to just have the text centered in the texture
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// The menu entry's velocity
        /// </summary>
        public Vector2 Velocity
        {
            get;
            set;
        }

        /// <summary>
        /// Acceleration of the menu entry
        /// </summary>
        public Vector2 Acceleration
        {
            get;
            set;
        }

        /// <summary>
        /// The menu screen that this entry belongs to
        /// </summary>
        public MenuScreen ParentMenu
        {
            get;
            set;
        }

        public MenuScreen SubMenu
        {
            get;
            private set;
        }

        bool hasSubMenu = false;
        public bool HasSubMenu
        {
            get { return hasSubMenu; }
        }

        Color color = Color.White;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        float scale = 1.0f;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        float opacity = 1.0f;
        public float Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }
        #endregion

        #region Events
        //Use events for dynamic entry commands

        /// <summary>
        /// When the entry is highlighted, perform a task
        /// Useful for sub-menus on highlight
        /// </summary>
        public event EventHandler OnHighlight;

        public virtual void Highlight()
        {
            Color = ParentMenu.Highlighted;
            state = EntryState.Highlight;
            if (OnHighlight != null)
                OnHighlight(this, EventArgs.Empty);
        }

        public event EventHandler OnNormal;

        public virtual void Normal()
        {
            Color = ParentMenu.Normal;
            state = EntryState.Normal;
            if (OnNormal != null)
                OnNormal(this, EventArgs.Empty);
        }

        /// <summary>
        /// When the entry is selected, do something like
        /// load a new screen or load its sub-menu.
        /// </summary>
        public event EventHandler Selected;

        public virtual void Select()
        {
            if (SubMenu != null)
            {
                state = EntryState.Selected;
                Color = ParentMenu.Selected;
                SubMenu.ActivateScreen();
                ParentMenu.ScreenSystem.AddScreen(SubMenu);
                ParentMenu.FreezeScreen();
            }
            if (Selected != null)
            {
                state = EntryState.Selected;
                Color = ParentMenu.Selected;
                Selected(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Initialization
        protected MenuEntry() { BoundingRectangle = new Rectangle(0, 0, 0, 0); }

        protected MenuEntry(MenuScreen screen, string title)
        {
            color = screen.Normal;
            ParentMenu = screen;
            EntryTitle = title;
            BoundingRectangle = new Rectangle(0, 0, 0, 0);
        }
        protected MenuEntry(MenuScreen screen, Texture2D texture)
        {
            color = screen.Normal;
            ParentMenu = screen;
            EntryTexture = texture;
            BoundingRectangle = new Rectangle(0, 0, 0, 0);
        }
        protected MenuEntry(MenuScreen screen, string title, Texture2D texture)
        {
            color = screen.Normal;
            ParentMenu = screen;
            EntryTitle = title;
            EntryTexture = texture;
            BoundingRectangle = new Rectangle(0, 0, 0, 0);
        }
        #endregion

        #region Update and Draw
        public void UpdateEntry(GameTime gameTime)
        {
            Update(gameTime);

            SpriteFont spriteFont = ParentMenu.SpriteFont;
            Vector2 measure = spriteFont == null ? Vector2.Zero : spriteFont.MeasureString(EntryTitle);
            int width = (int)(EntryTexture == null ? measure.X : EntryTexture.Width);
            int height = (int)(EntryTexture == null ? measure.Y : EntryTexture.Height);
            BoundingRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }
        public abstract void Update(GameTime gameTime);

        public abstract void AnimateHighlighted(GameTime gameTime);

        public virtual void Draw(GameTime gameTime, bool isSelected)
        {
            SpriteBatch spriteBatch = ParentMenu.ScreenSystem.SpriteBatch;
            SpriteFont spriteFont = ParentMenu.SpriteFont;

            Vector2 entryPosition = new Vector2(Position.X, Position.Y);

            //Not a sheet, draw it like so
            if (EntryTexture != null)
            {
                if (IsSheet)
                {
                    spriteBatch.Draw(EntryTexture, Position, Color.White * opacity);
                }
                else if(displayRegion != null)
                {
                    spriteBatch.Draw(EntryTexture, Position, displayRegion[isSelected], Color.White * opacity);
                }

                if (spriteFont != null && EntryTitle.Length > 0)
                {
                    Vector2 textDims = spriteFont.MeasureString(EntryTitle);

                    float x = EntryPadding.X == 0 ? (EntryTexture.Width / 2) - (textDims.X / 2) : EntryPadding.X;
                    float y = EntryPadding.Y == 0 ? (EntryTexture.Height / 2) - (textDims.Y / 2) : EntryPadding.Y;

                    entryPosition += new Vector2(x, y);

                    spriteBatch.DrawString(spriteFont, EntryTitle, entryPosition, color * opacity);
                }
            }
            else if (spriteFont != null && EntryTitle.Length > 0)
            {
                spriteBatch.DrawString(spriteFont, EntryTitle, entryPosition, color * opacity, 0, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }

        }

        public virtual void DrawDescription(GameTime gameTime, 
            Rectangle boxPosition, Point padding, Color textColor, float scale)
        {
            SpriteBatch spriteBatch = ParentMenu.ScreenSystem.SpriteBatch;
            SpriteFont spriteFont = ParentMenu.SpriteFont;

            if (spriteFont != null)
            {
                Vector2 textDims = spriteFont.MeasureString(EntryDescription) * scale;
                float x = padding.X;
                float y = padding.Y;

                Vector2 descriptionPosition = new Vector2(boxPosition.X + x, boxPosition.Y + y);
                spriteBatch.DrawString(spriteFont, EntryDescription, 
                    descriptionPosition, textColor, 0.0f, Vector2.Zero, scale, 
                    SpriteEffects.None, 1.0f);
            }
        }
        #endregion

        #region Methods
        public void AddSubMenu(MenuScreen subMenu)
        {
            this.hasSubMenu = true;
            this.SubMenu = subMenu;
        }

        /// <summary>
        /// Sets the position relative to the passed entry.  
        /// Vector2.Zero here will result in the exact same position
        /// as the passed entry.
        /// </summary>
        /// <param name="position">The position relative to the passed entry</param>
        /// <param name="entry">The entry you wish to use as a base</param>
        /// <param name="initialPosition">True if the passed position is an initial position for this entry, as in the entry will default to this position after animations and effects</param>
        public void SetRelativePosition(Vector2 relativePosition, MenuEntry entry, bool initialPosition)
        {
            Vector2 position = new Vector2(entry.Position.X, entry.Position.Y);
            if (entry.EntryTexture != null)
                position.Y += entry.EntryTexture.Height;
            SetPosition(Vector2.Add(position, relativePosition), initialPosition);
        }

        /// <summary>
        /// Sets the position of the entry
        /// </summary>
        /// <param name="position">The desired position</param>
        /// <param name="initialPosition">True if the passed position is an initial position for this entry, as in the entry will default to this position after animations and effects</param>
        public void SetPosition(Vector2 position, bool initialPosition)
        {
            if (initialPosition)
            {
                InitialPosition = position;
            }
            Position = position;
        }

        public void AddTexture(Texture2D texture)
        {
            EntryTexture = texture;
            numberOfEntries = new Point(1, 1);
        }

        public void AddTexture(Texture2D texture, int numX, int numY,
            Rectangle selectedEntry, Rectangle normalEntry)
        {
            EntryTexture = texture;
            numberOfEntries = new Point(numX, numY);
            displayRegion = new Dictionary<bool, Rectangle>();
            displayRegion.Add(true, selectedEntry);
            displayRegion.Add(false, normalEntry);
        }

        //Uses a single argument for both top and bottom padding
        public void AddPadding(int all)
        {
            EntryPadding = new Point(all, all);
        }

        //Uses two arguments for padding
        public void AddPadding(int left, int top)
        {
            EntryPadding = new Point(left, top);
        }
        #endregion
    }
}
