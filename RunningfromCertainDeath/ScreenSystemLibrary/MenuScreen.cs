using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ScreenSystemLibrary
{
    public abstract class MenuScreen : GameScreen
    {
        #region Fields and Properties

        /// <summary>
        /// The parent screen to revert back to when the menu is done.
        /// </summary>
        public GameScreen Parent
        {
            get;
            set;
        }

        /// <summary>
        /// List of menu entries to be displayed
        /// </summary>
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        public List<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        /// <summary>
        /// The spritefont object that controls the text for this menu
        /// </summary>
        public SpriteFont SpriteFont
        {
            get;
            set;
        }

        /// <summary>
        /// The texture to use as the background
        /// </summary>
        public Texture2D BackgroundTexture
        {
            get;
            set;
        }

        /// <summary>
        /// The position of the background texture
        /// </summary>
        public Vector2 BackgroundPosition
        {
            get;
            set;
        }

        /// <summary>
        /// The texture to use as the title
        /// </summary>
        public Texture2D TitleTexture
        {
            get;
            set;
        }

        public Vector2 InitialTitlePosition
        {
            get;
            set;
        }


        /// <summary>
        /// The position of the title texture
        /// </summary>
        public Vector2 TitlePosition
        {
            get;
            set;
        }

        /// <summary>
        /// The texture to use as the mouse
        /// </summary>
        public Texture2D MouseTexture
        {
            get;
            set;
        }

        /// <summary>
        /// The bounding box for the mouse
        /// </summary>
        public Rectangle MouseBounds
        {
            get;
            private set;
        }

        /// <summary>
        /// The opacity of the title texture
        /// </summary>
        float titleOpacity = 1.0f;
        public float TitleOpacity
        {
            get { return titleOpacity; }
            set { titleOpacity = value; }
        }

        //The position of our items
        public Vector2 Position
        {
            get;
            set;
        }

        //The color of the item when it is highlighted
        Color highlighted = Color.White;
        public Color Highlighted
        {
            get { return highlighted; }
            set { highlighted = value; }
        }

        //The color of the item when it is selected
        Color selected = Color.White;
        public Color Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        //The color of the text when it is not selected
        Color normal = Color.White;
        public Color Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        Texture2D descriptionTexture;

        Color descriptionColor, descriptionBoxColor;

        public Rectangle DescriptionPosition
        {
            get;
            private set;
        }

        public Point DescriptionPadding
        {
            get;
            set;
        }

        float descriptionScale;

        public abstract string PreviousEntryActionName { get; }
        public abstract string NextEntryActionName { get; }
        public abstract string SelectedEntryActionName { get; }
        public abstract string MenuCancelActionName { get; }

        //The selected menu item
        int selectedEntry = 0;

        public event EventHandler Cancel;
        #endregion

        #region Menu Operations
        //When we cancel the menu, this method is called
        public virtual void MenuCancel()
        {
            ExitScreen();
            if (Cancel != null)
                Cancel(this, EventArgs.Empty);
        }
        #endregion

        protected MenuScreen()
        {
            Removing += new EventHandler(MenuScreen_Removing);
        }

        public override bool AcceptsInput
        {
            get { return true; }
        }

        public abstract override void InitializeScreen();
        public abstract override void LoadContent();

        protected override void UpdateScreen(GameTime gameTime)
        {
            if (menuEntries.Count == 0) return;
            if (menuEntries[selectedEntry].State
                != MenuEntry.EntryState.Highlight)
            {
                menuEntries[selectedEntry].Highlight();
            }
            for (int i = 0; i < menuEntries.Count; i++)
            {
                menuEntries[i].UpdateEntry(gameTime);

                menuEntries[selectedEntry].AnimateHighlighted(gameTime);
            }

            if (MouseTexture != null)
            {
                Vector2 mousepos = InputMap.GetMousePosition();
                MouseBounds = new Rectangle((int)mousepos.X, (int)mousepos.Y, MouseTexture.Width, MouseTexture.Height);
            }
        }

        void MenuScreen_Removing(object sender, EventArgs e)
        {
            menuEntries.Clear();
            if (Parent != null && Parent.State == ScreenState.Frozen)
                Parent.ActivateScreen();
        }

        public override void HandleInput()
        {
            //If we move up or down, select a different entry
            if (InputMap.NewActionPress(PreviousEntryActionName))
            {
                menuEntries[selectedEntry].Normal();
                selectedEntry--;
                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
                menuEntries[selectedEntry].Highlight();

            }
            if (InputMap.NewActionPress(NextEntryActionName))
            {
                menuEntries[selectedEntry].Normal();
                selectedEntry++;
                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
                menuEntries[selectedEntry].Highlight();
            }

            if (MouseTexture != null)
            {
                for (int i = menuEntries.Count - 1; i >= 0; i--)
                {
                    if (menuEntries[i].BoundingRectangle.Intersects(MouseBounds))
                    {
                        menuEntries[selectedEntry].Normal();
                        selectedEntry = i;
                        menuEntries[selectedEntry].Highlight();
                    }
                }
            }

            //If we press the select button, call the MenuSelect method
            if (InputMap.NewActionPress(SelectedEntryActionName))
            {
                menuEntries[selectedEntry].Select();
            }

            //If we press the cancel button, call the MenuCancel method
            if (InputMap.NewActionPress(MenuCancelActionName))
            {
                MenuCancel();
            }
        }

        protected override void DrawScreen(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenSystem.SpriteBatch;
            if (BackgroundTexture != null)
                spriteBatch.Draw(BackgroundTexture, BackgroundPosition, Color.White);

            if (TitleTexture != null)
                spriteBatch.Draw(TitleTexture, TitlePosition, Color.White * titleOpacity);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                menuEntries[i].Draw(gameTime, i == selectedEntry);
            }

            DrawDescriptionArea(spriteBatch, gameTime);
            DrawMouse(spriteBatch, gameTime);
        }

        private void DrawDescriptionArea(SpriteBatch spriteBatch, GameTime gameTime)
        {
            String s = menuEntries[selectedEntry].EntryDescription;
            if (!String.IsNullOrEmpty(s) && descriptionTexture != null)
            {
                spriteBatch.Draw(descriptionTexture, DescriptionPosition, descriptionBoxColor);
                menuEntries[selectedEntry].DrawDescription(gameTime,
                    DescriptionPosition, DescriptionPadding, 
                    descriptionColor, descriptionScale);
            }
        }

        private void DrawMouse(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (MouseTexture != null)
            {
                spriteBatch.Draw(MouseTexture, InputMap.GetMousePosition(), Color.White);
            }
        }

        public void SetDescriptionArea(Rectangle position, 
            Color boxColor, Color TextColor, float scale)
        {
            SetDescriptionArea(position, boxColor, TextColor, new Point(0, 0), scale);
        }

        public void SetDescriptionArea(Rectangle position,
            Color boxColor, Color TextColor, Point padding, float scale)
        {
            DescriptionPosition = position;
            descriptionTexture = new Texture2D(ScreenSystem.GraphicsDevice, 1, 1);
            Color[] cArray = new Color[] { boxColor };
            descriptionTexture.SetData<Color>(cArray);
            descriptionBoxColor = boxColor;
            descriptionColor = TextColor;
            DescriptionPadding = padding;
            descriptionScale = scale;
        }

        public void EnableMouse(Texture2D texture)
        {
            MouseTexture = texture;
        }
    }
}
