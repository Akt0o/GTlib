using GTLib;
using System;
using System.Windows;
using System.Windows.Media;

namespace ShowCase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GlitchEffect testGlitch;
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += UpdateLoop;
            testGlitch = new GlitchEffect();
            testGlitch.setText("Hello World","Arial",30, "#1b3646");
            testGlitch.GenerateFrames(120,20,1);
        }

        public void UpdateLoop(object sender, EventArgs e)
        {
            testCanvas.Background = testGlitch.GetNextFrame();
        }

    }
}
