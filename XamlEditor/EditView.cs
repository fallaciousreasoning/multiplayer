using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiPlayer;
using MultiPlayer.Core.InputMethods;
using XamlEditor.Annotations;
using XamlEditor.Interop;
using XamlEditor.Scenes;
using MouseButton = MultiPlayer.Core.MouseButton;

namespace XamlEditor
{
    public class EditView : D3D11Host, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SceneProperty =
        DependencyProperty.Register("Scene", typeof(EditScene),
        typeof(EditView), new FrameworkPropertyMetadata(null));

        public EditScene Scene
        {
            get { return (EditScene)GetValue(SceneProperty); }
            set { SetValue(SceneProperty, value); }
        }

        private readonly ManualMouse mouse;
        private readonly ManualKeyboard keyboard;

        public EditView()
        {
            mouse = new ManualMouse();
            keyboard = new ManualKeyboard();
        }

        protected override void Initialize()
        {
            Focusable = true;
            Focus();

            var window = this;
            window.MouseMove += (o, args) => UpdateMouse(args);
            window.MouseDown += (o, args) => UpdateMouse(args);
            window.MouseUp += (o, args) => UpdateMouse(args);
            window.KeyUp += (o, args) => UpdateKeyBoard(args, false);
            window.KeyDown += (o, args) => UpdateKeyBoard(args, true);

            base.Initialize();
        }

        /// <summary>
        /// Updates the keyboard state when a key is pressed
        /// </summary>
        /// <param name="args">The key</param>
        /// <param name="p1">Whether the key is down</param>
        private void UpdateKeyBoard(KeyEventArgs args, bool p1)
        {
            Keys key;
            Enum.TryParse(args.Key.ToString(), true, out key);

            keyboard.SetKeyState(key , p1);
        }

        /// <summary>
        /// Sets the mouse state based on some mouse event
        /// </summary>
        /// <param name="args"></param>
        private void UpdateMouse(MouseEventArgs args)
        {
            var mousePos = args.GetPosition(this);
            mouse.MousePosition = new Vector2((float)mousePos.X, (float)mousePos.Y);

            mouse.SetButtonState(MouseButton.Left, (ButtonState)Enum.Parse(typeof(ButtonState), args.LeftButton.ToString()));
            mouse.SetButtonState(MouseButton.Right, (ButtonState)Enum.Parse(typeof(ButtonState), args.RightButton.ToString()));
            mouse.SetButtonState(MouseButton.Middle, (ButtonState)Enum.Parse(typeof(ButtonState), args.MiddleButton.ToString()));          
        }

        protected override void Load()
        {
            Scene = new EditScene(mouse, keyboard);
            Scene.Device = GraphicsDevice;
            Scene.Start();
            base.Load();
        }

        protected override void Unload()
        {
            base.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            var seconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
            Scene.Update(seconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);
            
            Scene.Draw();

            base.Draw(gameTime);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
