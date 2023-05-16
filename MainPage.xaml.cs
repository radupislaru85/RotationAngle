using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App12
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {   //variabile membru
        private SpriteVisual _spriteVisual;
        private CompositionPropertySet _sharedProperties; 

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += this.MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var compositor = Window.Current.Compositor;

            _sharedProperties = compositor.CreatePropertySet();                                                         //folosim compositorul pentru a crea o expresie pentru animatie, folosind-o ca o parte din constructor
            _sharedProperties.InsertScalar("Radius", 200);
            _sharedProperties.InsertScalar("Angle", 0);
            _sharedProperties.InsertScalar("OffsetX", (float)VisualElement.ActualWidth / 2.0f);                         //centrarea obiectului in centrul ecranului
            _sharedProperties.InsertScalar("OffsetY", (float)VisualElement.ActualHeight / 2.0f);
            var offsetExpression = compositor.CreateExpressionAnimation(                                                //functiile matematice
                "Vector3(" +
                    "sharedProperties.OffsetX + sharedProperties.Radius * Cos(ToRadians(sharedProperties.Angle)) ," +   //orientarea pe x
                    "sharedProperties.OffsetY + sharedProperties.Radius * Sin(ToRadians(sharedProperties.Angle)), " +   //orientarea pe y
                    "0)");                                                                                              //orientarea pe z
            offsetExpression.SetReferenceParameter("sharedProperties", _sharedProperties);

            _spriteVisual = compositor.CreateSpriteVisual();                                                            //proprietatile obiectului
            _spriteVisual.Brush = compositor.CreateColorBrush(Colors.DarkGreen);
            _spriteVisual.Size = new Vector2(100, 100);
            _spriteVisual.AnchorPoint = new Vector2(0.5f, 0.5f);                                                        //ancorarea la centru permite o tranzitie fluida
            

            ElementCompositionPreview.SetElementChildVisual(VisualElement, _spriteVisual);

            var rotateExpression = compositor.CreateExpressionAnimation("sharedProperties.Angle");                      //schimbarea orientarii obiectului pentru a fi mereu perpendicular pe cercul pe care il descrie
            rotateExpression.SetReferenceParameter("sharedProperties", _sharedProperties);
            _spriteVisual.StartAnimation("Offset", offsetExpression);
            _spriteVisual.StartAnimation(nameof(Visual.RotationAngleInDegrees), rotateExpression);
            
        }

        private void OffsetSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           
            _sharedProperties.InsertScalar("Angle", (float)e.NewValue);
        }
    }
}
