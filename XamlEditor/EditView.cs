using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core.InputMethods;
using XamlEditor.Interop;

namespace XamlEditor
{
    public class EditView : D3D11Host
    {
        private Scene scene;

        private readonly IMouse mouse;
        private readonly IKeyboard keyboard;

        public EditView()
        {
            mouse = new ManualMouse();
            keyboard = new ManualKeyboard();

            //Loaded += OnLoaded;
        }

        //private void OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    Focusable = true;
        //    Focus();

        //    var window = this;
        //    window.MouseMove += (o, args) => UpdateMouse(args);
        //    window.MouseDown += (o, args) => UpdateMouse(args);
        //    window.MouseUp += (o, args) => UpdateMouse(args);
        //    window.KeyUp += (o, args) => UpdateKeyBoard(args, false);
        //    window.KeyDown += (o, args) => UpdateKeyBoard(args, true);
        //}

        //protected override void Initialize()
        //{
        //    game = new EditGame(GraphicsDevice);

        //    game.Initialize();
        //    EnigmaGame.Input.MouseState = mouse;
        //    EnigmaGame.Input.KeyboardState = keyboard;

        //    base.Initialize();
        //}

        //protected override void LoadContent()
        //{
        //    game.LoadContent();
        //    base.LoadContent();
        //}

        //protected override void UnloadContent()
        //{
        //    game.UnloadContent();
        //    base.UnloadContent();
        //}

        //protected override void Update(GameTime gameTime)
        //{
        //    game.Update(gameTime);
        //    base.Update(gameTime);
        //}

        //protected override void Draw(GameTime gameTime)
        //{
        //    base.Draw(gameTime);
        //    game.Draw(gameTime);
        //}
    }
}
